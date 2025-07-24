using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Users;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.Tests
{
    public static class TestObjectMother
    {
        public static Goal CreateTestGoal()
        {
            return new Goal()
            {
                Title = "Test",
                CreatedDate = DateTime.Now,
                Description = "Test",
                Priority = Domain.Enums.Priority.Low,
                TargetDate = DateTime.Now.AddDays(10),
                Status = Domain.Enums.GoalStatus.NotStarted,

            };
        }

        public static CurrentUser CreateTestUser(string? id= null, string? userName= null)
        {
            return new CurrentUser(
           Id: id ?? "test-user-id",
           UserName: userName ?? "TestUser",
           Email: "test@example.com",
           Roles: new[] { "User" },
           DateOfBirth: null
       );

        }


        public static CreateGoalCommand CreateGoalCommandWithoutWorkItems()
        {

            return new CreateGoalCommand()
            {
                Title =  "Test Goal Title",
                Description = "Test Description",
                WorkItems = new List<CreateWorkItemDto>() 
            };
        }
       
    }
}
