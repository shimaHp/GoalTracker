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
        // Map from CreateGoalCommand to Goal entity and nested workitem as well
        CreateMap<CreateGoalDto, Goal>()
     .ForMember(dest => dest.Id, opt => opt.Ignore()) // DB will set it
     .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) // Set in code
     .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));

        CreateMap<CreateGoalCommand, Goal>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
    .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));

        //=========




        CreateMap<UpdateGoalCommand, Goal>();
        CreateMap<UpdateGoalDto, Goal>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())

    .ForMember(dest => dest.WorkItems, opt => opt.Ignore());
        //.ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore())
        //.ForMember(dest => dest.LastUpdatedById, opt => opt.Ignore());

//        CreateMap<WorkItem, WorkItemDto>()
//.ForMember(dest => dest.CreatorEmail, opt => opt.MapFrom(src => src.Creator.Email))
//.ForMember(dest => dest.AssigneeEmail, opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Email : null))
//.ForMember(dest => dest.LastUpdatedByName, opt => opt.MapFrom(src => src.LastUpdatedBy != null ? src.LastUpdatedBy.NormalizedEmail : null));

        CreateMap<WorkItem, WorkItemDto>()
    .ForMember(dest => dest.CreatorEmail, opt => opt.MapFrom(src => src.Creator.Email))
    .ForMember(dest => dest.AssigneeEmail, opt => opt.MapFrom(src => src.Assignee.Email))
    .ForMember(dest => dest.LastUpdatedByName, opt => opt.MapFrom(src => src.LastUpdatedBy.Email))
    .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id)) 
    .ForMember(dest => dest.AssigneeId, opt => opt.MapFrom(src => src.Assignee.Id)) 
    .ForMember(dest => dest.LastUpdatedById, opt => opt.MapFrom(src => src.LastUpdatedBy.Id));  






        CreateMap<Goal, GoalDto>()
    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => (int)src.Status))
    .ForMember(dest => dest.Priority,
        opt => opt.MapFrom(src => (int)src.Priority))
    .ForMember(dest => dest.WorkItems,
        opt => opt.MapFrom(src => src.WorkItems));
    }
}



    