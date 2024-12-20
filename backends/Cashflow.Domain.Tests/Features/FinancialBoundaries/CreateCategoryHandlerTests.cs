using Bogus;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions.FinancialBoundaries;
using Cashflow.Domain.Features.FinancialBoundaries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace Cashflow.Domain.Tests.Features.FinancialBoundaries;

public class CreateCategoryHandlerTests
{
    private readonly Faker _faker = new Faker();
    
    private readonly Mock<ILogger<CreateCategoryHandler>> _logger;
    private readonly Mock<ICashflowDbContext> _dbContext;
    private readonly Mock<IAuthenticatedUser> _authenticatedUser;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTests()
    {
        _logger = new Mock<ILogger<CreateCategoryHandler>>();
        _dbContext = new Mock<ICashflowDbContext>();
        _authenticatedUser = new Mock<IAuthenticatedUser>();
        
        _authenticatedUser.Setup(a => a.Id).Returns(1);
        
        _handler = new CreateCategoryHandler(_logger.Object, _dbContext.Object, _authenticatedUser.Object);
    }
    
    [Fact]
    public async Task Should_Success_CreatingCategoryWithMaximumBudgetInvestment()
    {
        //Arrange
        var existingCategories = new List<Category>().AsQueryable()
                                                                       .BuildMockDbSet();
        _dbContext.Setup(c => c.Categories)
                  .Returns(existingCategories.Object);


        var command = new CreateCategoryHandler.Request(_faker.Name.Random.String(), _faker.Random.Float(0, 1), null);

        //Act
        var result = await _handler.HandleAsync(command);
        
        //Assert
        result.Should().NotBeNull();
        
        existingCategories.Verify(c => c.Add(It.IsAny<Category>()), Times.Once);
        _dbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
    
    [Fact]
    public async Task Should_Success_CreatingCategoryWithMaximumMoneyInvestment()
    {
        //Arrange
        var existingCategories = new List<Category>().AsQueryable()
                                                                       .BuildMockDbSet();
        _dbContext.Setup(c => c.Categories)
                  .Returns(existingCategories.Object);


        var command = new CreateCategoryHandler.Request(_faker.Name.Random.String(), null, _faker.Random.Decimal(0, decimal.MaxValue));

        //Act
        var result = await _handler.HandleAsync(command);
        
        //Assert
        result.Should().NotBeNull();
        
        existingCategories.Verify(c => c.Add(It.IsAny<Category>()), Times.Once);
        _dbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_CreatingCategory_WhenNameAlreadyExists()
    {
        //Arrange
        var categoryToConflict = new Category("Name Abc");
        categoryToConflict.OwnerId = _authenticatedUser.Object.Id;
        
        var existingCategories = new List<Category> { categoryToConflict }.AsQueryable()
                                                                                            .BuildMockDbSet();

        _dbContext.Setup(c => c.Categories)
                  .Returns(existingCategories.Object);

        var command = new CreateCategoryHandler.Request(categoryToConflict.Name, _faker.Random.Float(0, 1), null);
        
        //Act and Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(command))
                                                   .Should()
                                                   .ThrowAsync<AttempToDuplicateCategoryNameException>();
    }
    
    [Fact]
    public async Task Should_Fail_CreatingCategory_WhenTwoFinancialConstraintsAreAdded()
    {
        //Arrange
        var existingCategories = new List<Category>().AsQueryable()
                                                                       .BuildMockDbSet();
        _dbContext.Setup(c => c.Categories)
                  .Returns(existingCategories.Object);

        var command = new CreateCategoryHandler.Request(_faker.Name.Random.String(), _faker.Random.Float(0, 1), _faker.Random.Decimal(0, decimal.MaxValue));

        //Act and Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(command))
                                                   .Should()
                                                   .ThrowAsync<AttempToAddMoreThanOneFinancialConstraintException>();
    }
}