
using GoalTracker.Domain.Entities;
using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;

namespace GoalTracker.Application.WorkItems.Dtos;

public class WorkItemProfile : Profile
{
    public WorkItemProfile()
    {
        CreateMap<WorkItem, WorkItemDto>();
        CreateMap<CreateWorkItemCommand, WorkItem > ();
       

    }

}
