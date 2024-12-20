using Bogus;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Exceptions;
using Cashflow.Domain.Exceptions.FinancialBoundaries;
using Cashflow.Domain.Features.FinancialBoundaries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Cashflow.Domain.Tests.Features.FinancialBoundaries;

public class UpdateCategoryHandlerTests
{
    private readonly Faker _faker = new Faker();

    private readonly Mock<ILogger<UpdateCategoryHandler>> _logger;
    private readonly Mock<ICashflowDbContext> _dbContext;
    private readonly Mock<IAuthenticatedUser> _authenticatedUser;
    private readonly UpdateCategoryHandler _handler;

    public UpdateCategoryHandlerTests()
    {
        _logger = new Mock<ILogger<UpdateCategoryHandler>>();
        _dbContext = new Mock<ICashflowDbContext>();
        _authenticatedUser = new Mock<IAuthenticatedUser>();

        _authenticatedUser.Setup(a => a.Id).Returns(1);

        _handler = new UpdateCategoryHandler(_logger.Object, _dbContext.Object, _authenticatedUser.Object);
    }

    [Fact]
    public async Task Should_Success_UpdateCategory()
    {
        // Arrange
        var category = new Category("Old Category Name") { Id = 1, OwnerId = _authenticatedUser.Object.Id };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var request = new UpdateCategoryHandler.Request("New Category Name", null, null, true);

        // Act
        var result = await _handler.HandleAsync(category.Id, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        category.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Should_Success_UpdateCategory_WithMaximumBudgetInvestment()
    {
        // Arrange
        var category = new Category("Category") { Id = 1, OwnerId = _authenticatedUser.Object.Id };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var request = new UpdateCategoryHandler.Request("Category", 0.5f, null, true);

        // Act
        var result = await _handler.HandleAsync(category.Id, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        category.MaximumBudgetInvestment.Should().Be(0.5f);
        category.MaximumMoneyInvestment.Should().BeNull();
    }

    [Fact]
    public async Task Should_Success_UpdateCategory_WithMaximumMoneyInvestment()
    {
        // Arrange
        var category = new Category("Category") { Id = 1, OwnerId = _authenticatedUser.Object.Id };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var request = new UpdateCategoryHandler.Request("Category", null, 100m, true);

        // Act
        var result = await _handler.HandleAsync(category.Id, request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        category.MaximumMoneyInvestment.Should().Be(100m);
        category.MaximumBudgetInvestment.Should().BeNull();
    }

    [Fact]
    public async Task Should_Fail_UpdateCategory_WithDuplicateName()
    {
        // Arrange
        var category1 = new Category("Category 1") { Id = 1, OwnerId = _authenticatedUser.Object.Id };
        var category2 = new Category("Category 2") { Id = 2, OwnerId = _authenticatedUser.Object.Id };
        var categories = new List<Category> { category1, category2 }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var request = new UpdateCategoryHandler.Request("Category 2", null, null, true);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(category1.Id, request))
            .Should()
            .ThrowAsync<AttempToDuplicateCategoryNameException>();
    }

    [Fact]
    public async Task Should_Fail_UpdateNonExistentCategory()
    {
        // Arrange
        var categories = new List<Category>().AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var request = new UpdateCategoryHandler.Request("New Category Name", null, null, true);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(1, request))
            .Should()
            .ThrowAsync<EntityNotFoundException<Category>>();
    }

    [Fact]
    public async Task Should_Fail_UpdateCategory_WithTwoFinancialConstraints()
    {
        // Arrange
        var category = new Category("Category") { Id = 1, OwnerId = _authenticatedUser.Object.Id };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        var request = new UpdateCategoryHandler.Request("Category", 0.5f, 100m, true);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(category.Id, request))
            .Should()
            .ThrowAsync<AttempToAddMoreThanOneFinancialConstraintException>();
    }
}