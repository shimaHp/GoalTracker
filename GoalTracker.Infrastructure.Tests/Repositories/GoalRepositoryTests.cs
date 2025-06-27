using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GoalTracker.Domain.Entities;
using GoalTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using GoalTracker.Infrastructure.Repositories;
using GoalTracker.Domain.Repository;
using Shouldly;
using FluentAssertions;

namespace GoalTracker.Infrastructure.Tests.Repositories
{
    public class GoalRepositoryTests
    {
        private readonly GoalTrackerDbContext _dbContext;
        private readonly GoalRepository _repository;

        public GoalRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GoalTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: "GoalTrackerDbTest")
                .Options;

            _dbContext = new GoalTrackerDbContext(options);
            _repository = new GoalRepository(_dbContext);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddGoalToDatabase()
        {
            // Arrange
            var goal = new Goal
            {
                Title = "Unit Testing",
                Description = "Learn how to write unit tests",
                TargetDate = DateTime.UtcNow.AddDays(7),
                CreatedDate = DateTime.UtcNow,
                UserId = Guid.NewGuid().ToString()
            };

            // Act
            var createdGoalId = await _repository.CreateAsync(goal);
            var savedGoal = await _dbContext.Goals.FindAsync(createdGoalId);

            // Assert
            createdGoalId.ShouldBe(goal.Id);
            savedGoal.Should().NotBeNull();
            savedGoal!.Title.Should().Be("Unit Testing");
        }


    }
}
