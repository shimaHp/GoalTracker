

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Application.Goals.Dtos;

public class GoalProfile : Profile
{
    public GoalProfile()
    {
        CreateMap<CreateGoalCommand, Goal>();
        CreateMap<UpdateGoalCommand, Goal>();



        // Map Goal
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.Priority,
                opt => opt.MapFrom(src => (int)src.Priority))
            .ForMember(dest => dest.WorkItems,
                opt => opt.MapFrom(src => src.WorkItems.Select(w =>
                    new WorkItemDto
                    {
                        Id = w.Id,
                        Title = w.Title,
                        Description = w.Description,
                        Status = w.Status,
                        CreatedDate = w.CreatedDate,
                        DueDate = w.DueDate
                    }).ToList()));
    }
}




    