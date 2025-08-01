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

           


        }
        [Fact()]
        public void CreateMap_ForCreateGoalCommandToGoal_MapsCorrectly()
        {
            //arrange

            


        }

      
    }
}