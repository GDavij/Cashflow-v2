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

public class DeleteCategoryHandlerTests
{
    private readonly Faker _faker = new Faker();
    
    private readonly Mock<ILogger<DeleteCategoryHandler>> _logger;
    private readonly Mock<ICashflowDbContext> _dbContext;
    private readonly Mock<IAuthenticatedUser> _authenticatedUser;
    private readonly DeleteCategoryHandler _handler;

    public DeleteCategoryHandlerTests()
    {
        _logger = new Mock<ILogger<DeleteCategoryHandler>>();
        _dbContext = new Mock<ICashflowDbContext>();
        _authenticatedUser = new Mock<IAuthenticatedUser>();
        
        _authenticatedUser.Setup(a => a.Id).Returns(1);
        
        _handler = new DeleteCategoryHandler(_logger.Object, _dbContext.Object, _authenticatedUser.Object);
    }
    
    [Fact]
    public async Task Should_Success_DeletingCategory()
    {
        // Arrange
        var category = new Category("Test Category") { Id = 1, OwnerId = _authenticatedUser.Object.Id };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();
        
        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act
        var result = await _handler.HandleAsync(category.Id);

        // Assert
        result.Should().NotBeNull();
        result.CategoryId.Should().Be(category.Id);
        
        category.Deleted.Should().BeTrue();
        _dbContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_DeletingNonExistentCategory()
    {
        // Arrange
        var categories = new List<Category>().AsQueryable().BuildMockDbSet();
        
        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(1))
                           .Should()
                           .ThrowAsync<EntityNotFoundException<Category>>();
    }

    [Fact]
    public async Task Should_Fail_DeletingCategoryNotOwnedByUser()
    {
        // Arrange
        var category = new Category("Test Category") { Id = 1, OwnerId = 2 };
        var categories = new List<Category> { category }.AsQueryable().BuildMockDbSet();
        
        _dbContext.Setup(c => c.Categories).Returns(categories.Object);

        // Act & Assert
        await FluentActions.Invoking(() => _handler.HandleAsync(category.Id))
                           .Should()
                           .ThrowAsync<EntityNotFoundException<Category>>();
    }
}