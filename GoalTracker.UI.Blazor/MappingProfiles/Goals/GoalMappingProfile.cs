using AutoMapper;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles.Goals;

public class GoalMappingProfile : Profile
{
    public GoalMappingProfile()
    {

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
        CreateMap<CreateWorkItemViewModel, CreateWorkItemCommand>();
    }
}
