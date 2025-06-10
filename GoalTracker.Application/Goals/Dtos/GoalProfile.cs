using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Application.Goals.Dtos;

public class GoalProfile : Profile
{
    public GoalProfile()
    {
        // Map from CreateGoalCommand to Goal entity
        CreateMap<CreateGoalCommand, Goal>()
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));

      
    
        CreateMap<UpdateGoalCommand, Goal>();
        CreateMap<UpdateGoalDto, Goal>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())

    .ForMember(dest => dest.WorkItems, opt => opt.Ignore());
    //.ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
    //.ForMember(dest => dest.LastUpdatedById, opt => opt.Ignore());



        CreateMap<Goal, GoalDto>()
    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => (int)src.Status))
    .ForMember(dest => dest.Priority,
        opt => opt.MapFrom(src => (int)src.Priority))
    .ForMember(dest => dest.WorkItems,
        opt => opt.MapFrom(src => src.WorkItems));
    }
}



    