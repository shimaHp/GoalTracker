
using GoalTracker.Domain.Entities;
using AutoMapper;

namespace GoalTracker.Application.Goals.WorkItems.Dtos;

public class WorkItemProfile:Profile
{
    public WorkItemProfile()
    {
        CreateMap<WorkItem, WorkItemDto>();  
    }

}
