using AutoMapper;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles.Goals;

public class GoalMappingProfile : Profile
{
    public GoalMappingProfile()
    {
         // Now it's simple - no date conversions needed!
        CreateMap<GoalDto, GoalViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

    }
}
