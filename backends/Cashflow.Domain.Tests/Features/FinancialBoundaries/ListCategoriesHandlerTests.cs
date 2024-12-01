using Bogus;
using Cashflow.Domain.Abstractions.DataAccess;
using Cashflow.Domain.Abstractions.RequestPipeline;
using Cashflow.Domain.Entities;
using Cashflow.Domain.Features.FinancialBoundaries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Cashflow.Domain.Tests.Features.FinancialBoundaries;

public class ListCategoriesHandlerTests
{
    private readonly Faker _faker = new Faker();

    private readonly Mock<ILogger<ListCategoriesHandler>> _logger;
    private readonly Mock<ICashflowDbContext> _dbContext;
    private readonly Mock<IAuthenticatedUser> _authenticatedUser;
    private readonly ListCategoriesHandler _handler;

    public ListCategoriesHandlerTests()
    {
        _logger = new Mock<ILogger<ListCategoriesHandler>>();
        _dbContext = new Mock<ICashflowDbContext>();
        _authenticatedUser = new Mock<IAuthenticatedUser>();

        _authenticatedUser.Setup(a => a.Id).Returns(1);

        _handler = new ListCategoriesHandler(_logger.Object, _dbContext.Object, _authenticatedUser.Object);
    }

    [Fact]
    public async Task Should_Success_ListCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category("Category 1") { Id = 1, OwnerId = _authenticatedUser.Object.Id },
            new Category("Category 2") { Id = 2, OwnerId = _authenticatedUser.Object.Id }
        }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "Category 1");
        result.Should().Contain(c => c.Name == "Category 2");
    }

    [Fact]
    public async Task Should_ReturnEmptyList_WhenNoCategoriesExist()
    {
        // Arrange
        var categories = new List<Category>().AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnOnlyUserCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category("Category 1") { Id = 1, OwnerId = _authenticatedUser.Object.Id },
            new Category("Category 2") { Id = 2, OwnerId = 2 }
        }.AsQueryable().BuildMockDbSet();

        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act
        var result = await _handler.HandleAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().Contain(c => c.Name == "Category 1");
    }
}