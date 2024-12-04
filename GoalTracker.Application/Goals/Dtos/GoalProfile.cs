

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Application.Goals.Dtos;

public class GoalProfile:Profile
{
    public GoalProfile()
    {
        CreateMap<CreateGoalCommand, Goal>();
        CreateMap<UpdateGoalCommand, Goal>();
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));
                
           
        
    }
}
