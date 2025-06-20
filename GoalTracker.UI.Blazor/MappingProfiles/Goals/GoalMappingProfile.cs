using AutoMapper;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles.Goals;

public class GoalMappingProfile : Profile
{
    public GoalMappingProfile()
    {
        // --------------------------
        // WorkItem Mappings
        // --------------------------
        CreateMap<CreateWorkItemViewModel, CreateWorkItemDto>();

        CreateMap<UpdateWorkItemViewModel, UpdateWorkItemDto>()
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src =>
                src.DueDate.HasValue ? new DateTimeOffset(src.DueDate.Value) : (DateTimeOffset?)null));

        CreateMap<WorkItemDto, WorkItemViewModel>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.DateTime))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.HasValue ? src.DueDate.Value.DateTime : (DateTime?)null))
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.LastUpdatedDate.HasValue ? src.LastUpdatedDate.Value.DateTime : (DateTime?)null));

        // --------------------------
        // Goal Mappings
        // --------------------------
        CreateMap<CreateGoalViewModel, CreateGoalCommand>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTimeOffset.Now))
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src =>
                src.TargetDate.HasValue ? new DateTimeOffset(src.TargetDate.Value) : (DateTimeOffset?)null))
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems)); // auto-converted via item mapping

        CreateMap<UpdateGoalViewModel, UpdateGoalDto>()
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src =>
                src.TargetDate.HasValue ? new DateTimeOffset(src.TargetDate.Value) : (DateTimeOffset?)null))
            .ForMember(dest => dest.UpdatedWorkItems, opt => opt.Ignore()) // handled manually
            .ForMember(dest => dest.NewWorkItems, opt => opt.Ignore())     // handled manually
            .ForMember(dest => dest.DeletedWorkItemIds, opt => opt.Ignore()); // handled manually

        CreateMap<GoalDto, GoalViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));

        // --------------------------
        // Enum Mappings
        // --------------------------
        CreateMap<Models.Enums.Priority, UpdateGoalDtoPriority>()
            .ConvertUsing(src => (UpdateGoalDtoPriority)(int)src);

        CreateMap<Models.Enums.GoalStatus, UpdateGoalDtoStatus>()
            .ConvertUsing(src => (UpdateGoalDtoStatus)(int)src);

        CreateMap<Models.Enums.WorkItemStatus, CreateWorkItemDtoStatus>()
            .ConvertUsing(src => (CreateWorkItemDtoStatus)(int)src);

        CreateMap<Models.Enums.WorkItemStatus, UpdateWorkItemDtoStatus>()
            .ConvertUsing(src => (UpdateWorkItemDtoStatus)(int)src);

        // --------------------------
        // Collaborator / User Mappings
        // --------------------------
        CreateMap<CollaboratorDto, CollaboratorViewModel>();
        CreateMap<CollaboratorViewModel, AssignUserRoleCommand>();
    }
}
