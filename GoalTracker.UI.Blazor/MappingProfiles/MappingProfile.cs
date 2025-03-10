using AutoMapper;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.MappingProfiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateGoalCommand, GoalViewModel>().ReverseMap();

            // Define a single, comprehensive mapping for GoalDto to GoalViewModel
            CreateMap<GoalDto, GoalViewModel>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate.DateTime))
                .ForMember(dest => dest.TargetDate,
                    opt => opt.MapFrom(src => src.TargetDate.HasValue
                        ? src.TargetDate.Value.DateTime
                        : (DateTime?)null))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => (GoalStatus)src.Status))
                .ForMember(dest => dest.Priority,
                    opt => opt.MapFrom(src => (Priority)src.Priority));

            // WorkItem Mapping
            CreateMap<WorkItemDto, WorkItemViewModel>()
                .ForMember(dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate.DateTime))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => (WorkItemStatus)src.Status));
            //.ForMember(dest => dest.Status,
            //    opt => opt.MapFrom(src => (Priority)src.Status));

            CreateMap<GoalViewModel, CreateGoalCommand>()
    .ForMember(dest => dest.CreatedDate,
        opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
