using AutoMapper;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles.Goals;

public class GoalMappingProfile : Profile
{
    public GoalMappingProfile()
    {

        CreateMap<UpdateGoalViewModel, UpdateGoalDto>()
.ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src =>
    src.TargetDate.HasValue ? new DateTimeOffset(src.TargetDate.Value) : (DateTimeOffset?)null))
.ForMember(dest => dest.UpdatedWorkItems, opt => opt.Ignore()) // manually added in the form
.ForMember(dest => dest.NewWorkItems, opt => opt.Ignore())     // manually added in the form
.ForMember(dest => dest.DeletedWorkItemIds, opt => opt.Ignore()); // manually added in the form

        CreateMap<UpdateWorkItemViewModel, UpdateWorkItemDto>()
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src =>
                src.DueDate.HasValue ? new DateTimeOffset(src.DueDate.Value) : (DateTimeOffset?)null));


        CreateMap<GoalDto, GoalViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));

        CreateMap<WorkItemDto, WorkItemViewModel>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.DateTime))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.HasValue ? src.DueDate.Value.DateTime : (DateTime?)null))
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.LastUpdatedDate.HasValue ? src.LastUpdatedDate.Value.DateTime : (DateTime?)null));

        CreateMap<CreateGoalViewModel, CreateGoalCommand>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.Now)) // Set current time
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src =>
                src.TargetDate.HasValue ? new DateTimeOffset(src.TargetDate.Value) : (DateTimeOffset?)null));
        CreateMap<CreateWorkItemViewModel, CreateWorkItemDto>();

    
    }
}
