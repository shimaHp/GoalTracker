using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.Users;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;

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
        public static CreateGoalDto CreateGoalDto(bool withWorkItems = false)
        {
            var dto = new CreateGoalDto()
            {
                Title = "Test Goal Title",
                Description = "Test Description",
                WorkItems = new List<CreateWorkItemDto>()
            };

            if (withWorkItems)
            {
                dto.WorkItems.Add(CreateWorkItemDto("Work Item 1"));
                dto.WorkItems.Add(CreateWorkItemDto("Work Item 2"));


            }

            return dto;
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
            return
             new CreateWorkItemDto()
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
                    AssigneeId = $"Test-assignee-{i}",
                    DueDate = DateTime.Now.AddDays(10),
                    Status = Domain.Enums.WorkItemStatus.NotStarted,
                    Goal = null,

                    //  user 
                    Creator = new User { Id = $"creator-{i}", Email = $"creator{i}@test.com", UserName = $"Creator{i}" },
                    Assignee = new User { Id = $"assignee-{i}", Email = $"assignee{i}@test.com", UserName = $"Assignee{i}" },
                    LastUpdatedBy = new User { Id = $"updater-{i}", Email = $"updater{i}@test.com", UserName = $"Updater{i}" }
                });
            }
            return workItems;
        }
    }
}