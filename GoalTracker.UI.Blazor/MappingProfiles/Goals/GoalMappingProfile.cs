using AutoMapper;
using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles.Goals;

/// <summary>
/// AutoMapper profile for Goal and WorkItem mappings between ViewModels and DTOs/Commands
/// </summary>
public class GoalMappingProfile : Profile
{
    public GoalMappingProfile()
    {
        ConfigureWorkItemMappings();
        ConfigureGoalMappings();
        ConfigureEnumMappings();
        ConfigureUserMappings();
    }

    #region WorkItem Mappings

    private void ConfigureWorkItemMappings()
    {
        CreateMap<CreateWorkItemViewModel, CreateWorkItemDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapWorkItemStatus(src.Status)))
             
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => ConvertToDateTimeOffset(src.DueDate)));

        CreateMap<UpdateWorkItemViewModel, UpdateWorkItemDto>()
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => ConvertToDateTimeOffset(src.DueDate)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapWorkItemStatusForUpdate(src.Status)));

        CreateMap<WorkItemDto, WorkItemViewModel>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => ConvertToDateTime(src.CreatedDate)))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => ConvertToNullableDateTime(src.DueDate)))
            .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => ConvertToNullableDateTime(src.LastUpdatedDate)));
    }

    #endregion

    #region Goal Mappings

    private void ConfigureGoalMappings()
    {
        CreateMap<CreateGoalViewModel, CreateGoalCommand>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow)) // Use UTC for consistency
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => ConvertToDateTimeOffset(src.TargetDate)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapGoalStatus(src.Status)))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(_ => GetDefaultGoalPriority()))
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));

        CreateMap<UpdateGoalViewModel, UpdateGoalDto>()
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => ConvertToDateTimeOffset(src.TargetDate)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapGoalStatusForUpdate(src.Status)))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => MapGoalPriorityForUpdate(src.Priority)))
            .ForMember(dest => dest.UpdatedWorkItems, opt => opt.Ignore()) // Handled in service layer
            .ForMember(dest => dest.NewWorkItems, opt => opt.Ignore())     // Handled in service layer
            .ForMember(dest => dest.DeletedWorkItemIds, opt => opt.Ignore()); // Handled in service layer

        CreateMap<GoalDto, GoalViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Set in service layer from user context
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => ConvertToNullableDateTime(src.TargetDate)))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => ConvertToDateTime(src.CreatedDate)))
            .ForMember(dest => dest.WorkItems, opt => opt.MapFrom(src => src.WorkItems));
    }

    #endregion

    #region Enum Mappings

    private void ConfigureEnumMappings()
    {
        // Explicit enum mappings for better maintainability
        CreateMap<Priority, UpdateGoalDtoPriority>()
            .ConvertUsing(src => MapPriorityToUpdateDto(src));

        CreateMap<GoalStatus, UpdateGoalDtoStatus>()
            .ConvertUsing(src => MapGoalStatusToUpdateDto(src));

        CreateMap<WorkItemStatus, CreateWorkItemDtoStatus>()
            .ConvertUsing(src => MapWorkItemStatus(src));

        CreateMap<WorkItemStatus, UpdateWorkItemDtoStatus>()
            .ConvertUsing(src => MapWorkItemStatusForUpdate(src));
    }

    #endregion

    #region User/Collaborator Mappings

    private void ConfigureUserMappings()
    {
        //CreateMap<CollaboratorDto, CollaboratorViewModel>()
        //    .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => ConvertToNullableDateTime(src.JoinedDate)));

        CreateMap<CollaboratorViewModel, AssignUserRoleCommand>();
    }

    #endregion

    #region Date Conversion Helpers

    private static DateTimeOffset? ConvertToDateTimeOffset(DateTime? dateTime)
        => dateTime?.ToUniversalTime();

    private static DateTime ConvertToDateTime(DateTimeOffset dateTimeOffset)
        => dateTimeOffset.DateTime;

    private static DateTime? ConvertToNullableDateTime(DateTimeOffset? dateTimeOffset)
        => dateTimeOffset?.DateTime;

    #endregion

    #region Goal Status Mapping

    private static CreateGoalCommandStatus MapGoalStatus(GoalStatus? status)
    {
        return status switch
        {
            GoalStatus.NotStarted => CreateGoalCommandStatus.NotStarted,
            GoalStatus.InProgress => CreateGoalCommandStatus.InProgress,
            GoalStatus.Completed => CreateGoalCommandStatus.Completed,
            GoalStatus.OnHold => CreateGoalCommandStatus.OnHold,
            GoalStatus.Cancelled => CreateGoalCommandStatus.Cancelled,
            null => CreateGoalCommandStatus.NotStarted,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown goal status")
        };
    }

    private static UpdateGoalDtoStatus MapGoalStatusToUpdateDto(GoalStatus status)
    {
        return status switch
        {
            GoalStatus.NotStarted => UpdateGoalDtoStatus.NotStarted,
            GoalStatus.InProgress => UpdateGoalDtoStatus.InProgress,
            GoalStatus.Completed => UpdateGoalDtoStatus.Completed,
            GoalStatus.OnHold => UpdateGoalDtoStatus.OnHold,
            GoalStatus.Cancelled => UpdateGoalDtoStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown goal status")
        };
    }

    private static UpdateGoalDtoStatus MapGoalStatusForUpdate(GoalStatus? status)
    {
        return status switch
        {
            GoalStatus.NotStarted => UpdateGoalDtoStatus.NotStarted,
            GoalStatus.InProgress => UpdateGoalDtoStatus.InProgress,
            GoalStatus.Completed => UpdateGoalDtoStatus.Completed,
            GoalStatus.OnHold => UpdateGoalDtoStatus.OnHold,
            GoalStatus.Cancelled => UpdateGoalDtoStatus.Cancelled,
            null => UpdateGoalDtoStatus.NotStarted,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown goal status")
        };
    }

    #endregion

    #region WorkItem Status Mapping

    private static CreateWorkItemDtoStatus MapWorkItemStatus(WorkItemStatus? status)
    {
        return (status ?? WorkItemStatus.NotStarted) switch
        {
            WorkItemStatus.NotStarted => CreateWorkItemDtoStatus.NotStarted,
            WorkItemStatus.InProgress => CreateWorkItemDtoStatus.InProgress,
            WorkItemStatus.Blocked => CreateWorkItemDtoStatus.Pending,
            WorkItemStatus.Completed => CreateWorkItemDtoStatus.Completed,
            WorkItemStatus.Cancelled => CreateWorkItemDtoStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown work item status")
        };
    }

    private static UpdateWorkItemDtoStatus MapWorkItemStatusForUpdate(WorkItemStatus? status)
    {
        return (status ?? WorkItemStatus.NotStarted) switch
        {
            WorkItemStatus.NotStarted => UpdateWorkItemDtoStatus.NotStarted,
            WorkItemStatus.InProgress => UpdateWorkItemDtoStatus.InProgress,
            WorkItemStatus.Blocked => UpdateWorkItemDtoStatus.Pending,
            WorkItemStatus.Completed => UpdateWorkItemDtoStatus.Completed,
            WorkItemStatus.Cancelled => UpdateWorkItemDtoStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown work item status")
        };
    }

    #endregion

    #region Priority Mapping

    private static CreateGoalCommandPriority GetDefaultGoalPriority()
        => CreateGoalCommandPriority.Medium;

    private static UpdateGoalDtoPriority MapGoalPriorityForUpdate(Priority? priority)
    {
        return priority switch
        {
            Priority.Low => UpdateGoalDtoPriority.Low,
            Priority.Medium => UpdateGoalDtoPriority.Medium,
            Priority.High => UpdateGoalDtoPriority.High,
            null => UpdateGoalDtoPriority.Medium,
            _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unknown priority")
        };
    }

    private static UpdateGoalDtoPriority MapPriorityToUpdateDto(Priority priority)
    {
        return priority switch
        {
            Priority.Low => UpdateGoalDtoPriority.Low,
            Priority.Medium => UpdateGoalDtoPriority.Medium,
            Priority.High => UpdateGoalDtoPriority.High,
            _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unknown priority")
        };
    }

    #endregion
}