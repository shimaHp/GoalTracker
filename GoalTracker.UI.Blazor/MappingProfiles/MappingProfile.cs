using AutoMapper;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<GoalDto,GoalViewModel>().ReverseMap();
            CreateMap<CreateGoalCommand, GoalViewModel>().ReverseMap();
           // CreateGoalCommand
        }
    }
}
