using Bogus;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Cashflow.Domain.Features.FinancialBoundaries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Cashflow.Domain.Tests.Features.FinancialBoundaries;

public class GetCategoryHandlerTests
{
    private readonly Faker _faker = new Faker();

    private readonly Mock<ILogger<GetCategoryHandler>> _logger;
    private readonly Mock<ICashflowDbContext> _dbContext;
    private readonly Mock<IAuthenticatedUser> _authenticatedUser;
    private readonly GetCategoryHandler _handler;

    public GetCategoryHandlerTests()
    {
        _logger = new Mock<ILogger<GetCategoryHandler>>();
        _dbContext = new Mock<ICashflowDbContext>();
        _authenticatedUser = new Mock<IAuthenticatedUser>();

        _authenticatedUser.Setup(a => a.Id).Returns(1);

        _handler = new GetCategoryHandler(_logger.Object, _dbContext.Object, _authenticatedUser.Object);
    }

    [Fact]
    public async Task Should_Success_GettingCategory()
    {
        // Arrange
        var category = new Category("Test Category") 
        { 
            Id = 1, 
            OwnerId = _authenticatedUser.Object.Id
        };
        category.UseMaximumBudgetInvestmentOf(_faker.Random.Float(0, 1));

        var transactionMethod = new TransactionMethod
        {
            Id = 1,
            Name = "Receive"
        };

        var transaction = new Transaction
        {
            Id = 1,
            TransactionMethod = transactionMethod,
            TransactionMethodId = transactionMethod.Id
        };

        category.Transactions.Add(transaction);

        var transactionsMethods = new List<TransactionMethod> { transactionMethod }.AsQueryable().BuildMockDbSet();
        _dbContext.Setup(c => c.TransactionMethods).Returns(transactionsMethods.Object);
        
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();
        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var transactions = category.Transactions.AsQueryable().BuildMockDbSet();
        _dbContext.Setup(c => c.Transactions).Returns(transactions.Object);

        var bankAccounts = new List<BankAccount>().AsQueryable().BuildMockDbSet();
        _dbContext.Setup(c => c.BankAccounts).Returns(bankAccounts.Object);

        // Act
        var result = await _handler.HandleAsync(category.Id, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.MaximumBudgetInvestment.Should().Be(category.MaximumBudgetInvestment);
        result.MaximumMoneyInvestment.Should().Be(category.MaximumMoneyInvestment);
        result.TotalTransactionsRegistered.Should().Be(category.Transactions.Count);
    }

    [Fact]
    public async Task Should_Fail_GettingNonExistentCategory()
    {
        // Arrange
        var categories = new List<Category>().AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(1, default))
                           .Should()
                           .ThrowAsync<EntityNotFoundException<Category>>();
    }

    [Fact]
    public async Task Should_Fail_GettingCategoryNotOwnedByUser()
    {
        // Arrange
        var category = new Category("Test Category") { Id = 1, OwnerId = 2 };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(category.Id, default))
                           .Should()
                           .ThrowAsync<EntityNotFoundException<Category>>();
    }
}