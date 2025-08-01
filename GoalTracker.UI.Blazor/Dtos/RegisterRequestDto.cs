﻿namespace GoalTracker.UI.Blazor.Dtos;

public record RegisterRequestDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
