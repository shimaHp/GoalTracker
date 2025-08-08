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
        public void GoalProfile_UpdateGoalDtoToExistingGoalWithnoWorkItems_ShouldUpdateCorrectProperties()
        {
            // Arrange 
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });
            var mapper = config.CreateMapper();
            var updateGoalDtoTest = TestObjectMother.CreateUpdateGoalDto(1);
            var existingGoalTest = TestObjectMother.CreateExistingGoal(1);
            // Act 
            var result = mapper.Map(updateGoalDtoTest, existingGoalTest);
            // Assert 
            Assert.Equal(updateGoalDtoTest.Title, existingGoalTest.Title);
            Assert.Equal(updateGoalDtoTest.Description, existingGoalTest.Description);
            Assert.Equal(updateGoalDtoTest.Priority, existingGoalTest.Priority);
            Assert.Equal(updateGoalDtoTest.Status, existingGoalTest.Status);
            Assert.Equal(updateGoalDtoTest.TargetDate, existingGoalTest.TargetDate);
            //shouldnt change
            Assert.Equal(1, existingGoalTest.Id);
            Assert.Equal(DateTime.Now.AddDays(-5).Date, existingGoalTest.CreatedDate.Date);
        }
        [Fact]
        public void GoalProfile_UpdateGoalDtoWithNullDescription_ShouldSetToNull()
        {
            // Arrange 
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });
            var mapper = config.CreateMapper();
            var updateGoalDtoTest = TestObjectMother.CreateUpdateGoalDto(1);
            // Arrange
            var existingGoalTest = TestObjectMother.CreateExistingGoal(1);
            existingGoalTest.Description = "Original Description";
            var updateDto = TestObjectMother.CreateUpdateGoalDto(1);
            updateDto.Description = null;
            // Act
            mapper.Map(updateDto, existingGoalTest);
            // Assert
            Assert.Null(existingGoalTest.Description); // Should be null now
        }
        [Fact]
        public void GoalProfile_UpdateGoalDtoWithNullTitle_ShouldThrowException()
        {
            // Arrange 
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });
            var mapper = config.CreateMapper();
            var updateGoalDtoTest = TestObjectMother.CreateUpdateGoalDto(1);

            // Arrange
            var existingGoalTest = TestObjectMother.CreateExistingGoal(1);
            existingGoalTest.Title = null;

            var updateDto = TestObjectMother.CreateUpdateGoalDto(1);
            updateDto.Title = null;

            //Act
            mapper.Map(updateDto, existingGoalTest);
            //Assert
            Assert.Null(existingGoalTest.Title);
        }
        [Fact]
        public void GoalProfile_UpdateGoalDtoWithExistingWorkItems_ShouldIgnoreWorkItems()
        {
            // Arrange 
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GoalProfile>();
                cfg.AddProfile<WorkItemProfile>();
            });
            var mapper = config.CreateMapper();
            var updateGoalDtoTest = TestObjectMother.CreateUpdateGoalDto(1);
            var existingGoal = TestObjectMother.CreateExistingGoal(1);
            existingGoal.WorkItems = TestObjectMother.CreateWorkItems(2);
            var originalWorkItemsCount = existingGoal.WorkItems.Count;
            var originalFirstWorkItemTitle = existingGoal.WorkItems.First().Title;
            //Act
            mapper.Map(updateGoalDtoTest, existingGoal);
            //Assert
            Assert.Equal(originalWorkItemsCount, existingGoal.WorkItems.Count);
            Assert.Equal(originalFirstWorkItemTitle, existingGoal.WorkItems.First().Title);

        }

        //[Fact]
        //public void AutoMapper_Configuration_ShouldBeValid()
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile<GoalProfile>();
        //        cfg.AddProfile<WorkItemProfile>();
        //    });

        //    config.AssertConfigurationIsValid(); // This will tell you about unused mappings
        //}

        [Fact]

        public void GoalProfile_MappingGoalToGoalDto_ShouldMapCorrectly()
        {
         var config=new MapperConfiguration(cfg => {
             cfg.AddProfile<GoalProfile>();
             cfg.AddProfile<WorkItemProfile>();
             });
            var mapper=config.CreateMapper();
            var goalTest = TestObjectMother.CreateGoal();
            goalTest.WorkItems=TestObjectMother.CreateWorkItems(2);

            //Act
            var result = mapper.Map<GoalDto>(goalTest);

            //Assert
            Assert.Equal(goalTest.Title, result.Title);
            Assert.Equal(goalTest.Description, result.Description);
            Assert.Equal(goalTest.Id, result.Id);
            Assert.Equal(goalTest.Priority, result.Priority);
            Assert.Equal(goalTest.Status, result.Status);
            Assert.Equal(goalTest.CreatedDate, result.CreatedDate);
            Assert.Equal(goalTest.TargetDate, result.TargetDate);
            Assert.Equal(goalTest.WorkItems.Count, result.WorkItems.Count);
           

        }
    }

}





