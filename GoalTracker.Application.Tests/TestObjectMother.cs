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
        public static Goal CreateGoal()
        {
            return new Goal()
            {
                Title = "Test Goal",
                CreatedDate = DateTime.Now,
                Description = "Test Description",
                Priority = Domain.Enums.Priority.Low,
                TargetDate = DateTime.Now.AddDays(10),
                Status = Domain.Enums.GoalStatus.NotStarted,
            };
        }

        public static CurrentUser CreateUser(string? id = null, string? userName = null)
        {
            return new CurrentUser(
                Id: id ?? "test-user-id",
                UserName: userName ?? "TestUser",
                Email: "test@example.com",
                Roles: new[] { "User" },
                DateOfBirth: null
            );
        }


        public static CreateGoalCommand CreateGoalCommand(bool withWorkItems = false)
        {
            var command = new CreateGoalCommand()
            {
                Title = "Test Goal Title",
                Description = "Test Description",
                WorkItems = new List<CreateWorkItemDto>()
            };

            if (withWorkItems)
            {
                command.WorkItems.Add(CreateWorkItemDto("Work Item 1"));
                command.WorkItems.Add(CreateWorkItemDto("Work Item 2"));
            }

            return command;
        }
        public static CreateWorkItemDto CreateWorkItemDto(string title = "Test Work Item")
        {
            return new CreateWorkItemDto()
            {
                Title = title,
                Description = "Test Description",
                AssigneeId = "test-assignee",
                DueDate = DateTime.Now.AddDays(10),
                Priority = Domain.Enums.Priority.Low,
                Status = Domain.Enums.WorkItemStatus.NotStarted,
            };
        }

        public static List<WorkItem> CreateWorkItems(int count = 2)
        {
            var workItems = new List<WorkItem>();
            for (int i = 1; i <= count; i++)
            {
                workItems.Add(new WorkItem()
                {
                    Title = $"Work Item {i}",
                    Description = "Test Description",
                    AssigneeId = "test-assignee",
                    DueDate = DateTime.Now.AddDays(10),
                                       Status = Domain.Enums.WorkItemStatus.NotStarted,
                    Goal = null // Add this line
                });
            }
            return workItems;
        }
    }
}