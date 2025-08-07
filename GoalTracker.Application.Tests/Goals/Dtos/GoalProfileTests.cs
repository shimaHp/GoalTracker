using AutoMapper;
using GoalTracker.Application.Tests;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;


namespace GoalTracker.Application.Goals.Dtos.Tests
{

    public class GoalProfileTests
    {
        [Fact()]
        public void GoalProfile_MapCreateGoalCommandToGoal_ShouldMapCorrectly()
        {
            //arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();

            });
            var mapper = config.CreateMapper();
            var commandTest = TestObjectMother.CreateGoalCommand(withWorkItems: true);
            //Act
            var result = mapper.Map<Goal>(commandTest);

            //Assert
            // Test Goal mapping
            Assert.Equal(commandTest.Title, result.Title);
            Assert.Equal(commandTest.Description, result.Description);
            Assert.Equal(0, result.Id); // Should be ignored
            Assert.Equal(default(DateTime), result.CreatedDate); // Should be ignored

            // Test WorkItems mapping
            Assert.Equal(commandTest.WorkItems.Count, result.WorkItems.Count);
            Assert.Equal(commandTest.WorkItems[0].Title, result.WorkItems.First().Title);
            Assert.Equal(commandTest.WorkItems[0].Description, result.WorkItems.First().Description);


        }

        [Fact()]
        public void GoalProfile_MapCreateGoalDtoToGoalWithNoWorkItems_ShouldMapCorrectly()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });
            var mapper = config.CreateMapper();

            var goalDtoTest = TestObjectMother.CreateGoalDto(false);
            //Act
            var result = mapper.Map<Goal>(goalDtoTest);
            // Test Goal mapping
            Assert.Equal(goalDtoTest.Title, result.Title);
            Assert.Equal(goalDtoTest.Description, result.Description);
            Assert.Equal(goalDtoTest.Status, result.Status);
            Assert.Equal(goalDtoTest.Priority, result.Priority);
            Assert.Empty(result.WorkItems);

        }
        [Fact()]
        public void GoalProfile_MapGoalToGoalDtoWithNoWorkItems_ShouldMapCorrectly()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });

            var mapper = config.CreateMapper();
            var goalTest = TestObjectMother.CreateGoal();
            //Act
            var result = mapper.Map<GoalDto>(goalTest);
            // Assert
            Assert.Equal(goalTest.Title, result.Title);
            Assert.Equal(goalTest.Description, result.Description);
            Assert.Equal(goalTest.Status, result.Status);
            Assert.Equal(goalTest.Priority, result.Priority);
            Assert.Empty(result.WorkItems);
        }

        [Fact()]
        public void GoalProfile_MapGoalToGoalDtoWithWorkItems_ShouldMapCorrectly()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });

            var mapper = config.CreateMapper();
            var goalTest = TestObjectMother.CreateGoal();
            goalTest.WorkItems = TestObjectMother.CreateWorkItems();
            //Act
            var result = mapper.Map<GoalDto>(goalTest);
            //Assert

            //Goal
            // Assert
            Assert.Equal(goalTest.Title, result.Title);
            Assert.Equal(goalTest.Description, result.Description);
            Assert.Equal(goalTest.Status, result.Status);
            Assert.Equal(goalTest.Priority, result.Priority);
            //WorkItems
            Assert.NotEmpty(result.WorkItems);
            Assert.Equal(goalTest.WorkItems.Count, result.WorkItems.Count);
            //Test eachworkitems seperatly
            var expectedWorkitems = goalTest.WorkItems.ToArray();
            var actualWorkItems = result.WorkItems.ToArray();
            for (int i = 0; i < expectedWorkitems.Length; i++)
            {
                Assert.Equal(expectedWorkitems[i].Title, actualWorkItems[i].Title);
                Assert.Equal(expectedWorkitems[i].Description, actualWorkItems[i].Description);
                Assert.Equal(expectedWorkitems[i].Status, actualWorkItems[i].Status);
                Assert.Equal(expectedWorkitems[i].DueDate, actualWorkItems[i].DueDate);
                Assert.Equal(expectedWorkitems[i].AssigneeId, actualWorkItems[i].AssigneeId);
                Assert.Equal(expectedWorkitems[i].CreatorId, actualWorkItems[i].CreatorId);
                Assert.Equal(expectedWorkitems[i].CreatedDate, actualWorkItems[i].CreatedDate);
            }
        }

        [Fact()]
        public void GoalProfile_MapCreateGoalDtoToGoalWithWorkItems_ShouldMapCorrectly()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });
            var mapper = config.CreateMapper();

            var goalDtoTest = TestObjectMother.CreateGoalDto(true);
            //Act
            var result = mapper.Map<Goal>(goalDtoTest);
            //Assert
            // Test Goal mapping
            Assert.Equal(goalDtoTest.Title, result.Title);
            Assert.Equal(goalDtoTest.Description, result.Description);
            Assert.Equal(goalDtoTest.Status, result.Status);
            Assert.Equal(goalDtoTest.Priority, result.Priority);
            // Test WorkItems
            Assert.NotEmpty(result.WorkItems);
            Assert.Equal(goalDtoTest.WorkItems.Count, result.WorkItems.Count);
            //
            var expectedWorkitems = goalDtoTest.WorkItems.ToArray();
            var actualWorkItems = result.WorkItems.ToArray();
            for (int i = 0; i < expectedWorkitems.Length; i++)
            {
                Assert.Equal(expectedWorkitems[i].Title, actualWorkItems[i].Title);
                Assert.Equal(expectedWorkitems[i].Description, actualWorkItems[i].Description);
                Assert.Equal(expectedWorkitems[i].Status, actualWorkItems[i].Status);
                Assert.Equal(expectedWorkitems[i].DueDate, actualWorkItems[i].DueDate);
                Assert.Equal(expectedWorkitems[i].AssigneeId, actualWorkItems[i].AssigneeId);

            }

        }

        [Fact]
        public void WorkItemProfile_MapWorkItemToWorkItemDto_ShouldMapAllPropertiesCorrectly()
        {
            var result = new WorkItemProfile();
        }


    }
}





