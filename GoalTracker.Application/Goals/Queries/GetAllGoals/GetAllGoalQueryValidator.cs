
using FluentValidation;

using GoalTracker.Application.Goals.Dtos;

namespace GoalTracker.Application.Goals.Queries.GetAllGoals;

public class GetAllGoalQueryValidator:AbstractValidator<GetAllGoalsQuery>
{
    private int[] allowPageSize = [5, 10, 15, 30];
    private string[] allowSortByColumnNames = [nameof(GoalDto.Title)];
    public GetAllGoalQueryValidator()
    {
        
        RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSize.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowPageSize)}]");

        RuleFor(r => r.SortBy)
            .Must(value => allowSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optinal,or must be in [{string.Join(",", allowSortByColumnNames)}]");

    }
}

