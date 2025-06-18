


using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Enums;
using MediatR;

namespace GoalTracker.Application.Goals.Commands.editGoal;



    /// <summary>
    /// Command to update an existing goal with its work items
    /// </summary>
    public class UpdateGoalCommand : IRequest<GoalDto>
    {
        public UpdateGoalCommand(UpdateGoalDto updateGoalDto)
        {
            UpdateGoalDto = updateGoalDto ?? throw new ArgumentNullException(nameof(updateGoalDto));
        }

        // Private setter ensures immutability
        public UpdateGoalDto UpdateGoalDto { get; private set; }

        // Convenience properties for easier access in handler
        public int GoalId => UpdateGoalDto.Id;
        public string Title => UpdateGoalDto.Title;
        public string? Description => UpdateGoalDto.Description;
    }
