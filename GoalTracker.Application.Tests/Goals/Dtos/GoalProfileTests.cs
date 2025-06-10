using Xunit;
using GoalTracker.Application.Goals.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Enums;
using FluentAssertions;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;

namespace GoalTracker.Application.Goals.Dtos.Tests
{

    public class GoalProfileTests
    {
        private IMapper _mapper;
        public GoalProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<GoalProfile>());
            _mapper = configuration.CreateMapper();

        }

        [Fact()]
        public void CreateMap_ForGoalToGoalDto_MapsCorrectly()
        {
            //arrange

            var goal = new Goal()
            {
                Id = 1,
                Title = "Test",
                Description = "Test",
                CreatedDate = DateTime.Now,
                Priority = Priority.Critical,
                Status = GoalStatus.NotStarted,
                TargetDate = DateTime.Now,

            };

            //act
            var goalDto = _mapper.Map<GoalDto>(goal);

            //assert

            goalDto.Should().NotBeNull();
            goalDto.Id.Should().Be(goal.Id);
            goalDto.Title.Should().Be(goal.Title);
            goalDto.Description.Should().Be(goal.Description);
            goal.CreatedDate.Should().Be(goal.CreatedDate);
            goal.Priority.Should().Be(goal.Priority);
            goal.Status.Should().Be(goal.Status);
            goal.TargetDate.Should().Be(goal.TargetDate);


        }
        [Fact()]
        public void CreateMap_ForCreateGoalCommandToGoal_MapsCorrectly()
        {
            //arrange

            var command = new CreateGoalCommand()
            {
                Title = "Test",
                Description = "Test",
                CreatedDate = DateTime.Now,
                Priority = Priority.Critical,
                Status = GoalStatus.NotStarted,
                TargetDate = DateTime.Now,

            };

            //act 
            var goal = _mapper.Map<Goal>(command);

            //assert

            goal.Should().NotBeNull();
            goal.Title.Should().Be(command.Title);
            goal.Description.Should().Be(command.Description);
            goal.CreatedDate.Should().Be(command.CreatedDate);
            goal.Priority.Should().Be(command.Priority);
            goal.Status.Should().Be(command.Status);
            goal.TargetDate.Should().Be(command.TargetDate);


        }

        // [Fact()]
        //    public void CreateMap_ForUpdateGoalCommandToGoal_MapsCorrectly()
        //    {
        //        //arrange

        //        var command = new UpdateGoalCommand()
        //        {

        //            Id=1,
        //            Title = "Test",
        //            Description = "Test",
        //            Priority = Priority.Critical,
        //            Status = GoalStatus.NotStarted,
        //            TargetDate = DateTime.Now,

        //        };

        //        //act 
        //        var goal = _mapper.Map<Goal>(command);

        //        //assert

        //        goal.Should().NotBeNull();
        //        goal.Id.Should().Be(command.Id);
        //        goal.Title.Should().Be(command.Title);
        //        goal.Description.Should().Be(command.Description);
        //        goal.Priority.Should().Be(command.Priority);
        //        goal.Status.Should().Be(command.Status);
        //        goal.TargetDate.Should().Be(command.TargetDate);


        //    }
        //}
    }
}