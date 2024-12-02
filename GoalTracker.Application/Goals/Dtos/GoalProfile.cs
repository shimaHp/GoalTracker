

using AutoMapper;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Application.Goals.Dtos;

public class GoalProfile:Profile
{
    public GoalProfile()
    {
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));
                
           
        
    }
}
