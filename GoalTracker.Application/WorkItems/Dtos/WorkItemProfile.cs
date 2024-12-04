
using GoalTracker.Domain.Entities;
using AutoMapper;

namespace GoalTracker.Application.WorkItems.Dtos;

public class WorkItemProfile : Profile
{
    public WorkItemProfile()
    {
        CreateMap<WorkItem, WorkItemDto>();
    }

}
