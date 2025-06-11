
using GoalTracker.Domain.Entities;
using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;

namespace GoalTracker.Application.WorkItems.Dtos;

public class WorkItemProfile : Profile
{
    public WorkItemProfile()
    {

        //create
        CreateMap<CreateWorkItemDto, WorkItem>()
.ForMember(dest => dest.Id, opt => opt.Ignore())
.ForMember(dest => dest.GoalId, opt => opt.Ignore())
.ForMember(dest => dest.Goal, opt => opt.Ignore())
.ForMember(dest => dest.CreatorId, opt => opt.Ignore())
.ForMember(dest => dest.Creator, opt => opt.Ignore())
.ForMember(dest => dest.AssigneeId, opt => opt.MapFrom(src => src.AssigneeId.ToString()))
.ForMember(dest => dest.Assignee, opt => opt.Ignore())
.ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
.ForMember(dest => dest.LastUpdatedById, opt => opt.Ignore())
.ForMember(dest => dest.LastUpdatedDate, opt => opt.Ignore());


        CreateMap<CreateWorkItemDto, WorkItem>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.GoalId, opt => opt.Ignore())
    .ForMember(dest => dest.Goal, opt => opt.Ignore())
    .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        //============






        CreateMap<WorkItem, WorkItemDto>()
         .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.UserName)) // or UserName/Email
         .ForMember(dest => dest.CreatorEmail, opt => opt.MapFrom(src => src.Creator.Email))
         .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.UserName : null))
         .ForMember(dest => dest.AssigneeEmail, opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Email : null))
         .ForMember(dest => dest.LastUpdatedByName, opt => opt.MapFrom(src => src.LastUpdatedBy != null ? src.LastUpdatedBy.UserName : null))
         .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => src.LastUpdatedDate)); // If you track this field
        // Map from CreateWorkItemCommand to WorkItem entity
        
        // Map from UpdateWorkItemDto to WorkItem
        CreateMap<UpdateWorkItemDto, WorkItem>().ReverseMap();
        CreateMap<CreateWorkItemCommand, WorkItem>().ReverseMap();







    }

}
