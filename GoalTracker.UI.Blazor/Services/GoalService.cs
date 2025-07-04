﻿using AutoMapper;
using Blazored.Toast.Services;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.Goals;
using GoalTracker.UI.Blazor.Services.Base;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Services
{
    public class GoalService : BaseHttpService,IGoalService
    {
        private readonly IMapper _mapper;
        private readonly IToastService _toastService;

        public GoalService(IMapper mapper,IToastService toastService, IClient client, Blazored.LocalStorage.ILocalStorageService localStorage) 
            : base(client, localStorage)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _toastService = toastService;
        }
      
  public async Task<List<GoalViewModel>> GetGoals()
        {
            try
            {
                await AddBearerToken();

              
                var httpClient = _client.HttpClient;
                var response = await httpClient.GetAsync($"api/Goals?searchPharse=&pageNumber=1&pageSize=10&sortBy=Title&sortDirection=0");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Log the full response for debugging
                    Console.WriteLine($"API Response: {content}");

                    using var doc = JsonDocument.Parse(content);

                    // Extract just the items array if it exists
                    if (doc.RootElement.TryGetProperty("items", out var itemsElement))
                    {
                        var itemsJson = itemsElement.GetRawText();
                        Console.WriteLine($"Items JSON: {itemsJson}");

                        var goals = System.Text.Json.JsonSerializer.Deserialize<List<GoalDto>>(itemsJson);

                        // Check if deserialization was successful
                        if (goals == null)
                        {
                            Console.WriteLine("Deserialization resulted in null goals list");
                            return new List<GoalViewModel>();
                        }

                        Console.WriteLine($"Deserialized {goals.Count} goal(s)");

                        // Log first goal properties for debugging
                        if (goals.Count > 0)
                        {
                            var firstGoal = goals[0];
                            Console.WriteLine($"First goal: Id={firstGoal.Id}, Title={firstGoal.Title}, CreatedDate={firstGoal.CreatedDate}");
                        }

                        try
                        {
                            Console.WriteLine($"_mapper is null: {_mapper == null}");
                            Console.WriteLine($"goals is null: {goals == null}");
                            Console.WriteLine($"goals count: {goals?.Count}");

                            if (_mapper == null)
                            {
                                Console.WriteLine("ERROR: _mapper is NULL - this is the problem!");
                                return new List<GoalViewModel>();
                            }
                            var viewModels = _mapper.Map<List<GoalViewModel>>(goals);
                            return viewModels;
                        }
                        catch (Exception mapEx)
                        {
                            Console.WriteLine($"Mapping exception details: {mapEx}");
                            return new List<GoalViewModel>();
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"API returned non-success status code: {response.StatusCode}");
                }

                return new List<GoalViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching goals: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<GoalViewModel>();
            }
        }

  public async Task<Response<int>> DeleteGoal(int goalId)
        {
            try
            {
                await _client.GoalsDELETEAsync(goalId);
                return new Response<int> { Success = true, Data = goalId };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
 public async Task<Response<GoalDto>> CreateGoal(CreateGoalViewModel goal)
        {
            try
            {
                await AddBearerToken();
                var createGoalCommand = _mapper.Map<CreateGoalCommand>(goal);

                var createdGoal = await _client.GoalsPOSTAsync(createGoalCommand);

                // Show success toast
                _toastService.ShowSuccess($"Goal '{createdGoal.Title}' created successfully!");

                return new Response<GoalDto>
                {
                    Success = true,
                    Data = createdGoal,
                    Message = "Goal created successfully"
                };
            }
            catch (ApiException ex)
            {
                var response = ConvertApiExceptions<GoalDto>(ex);

                if (!response.Success)
                {
                    _toastService.ShowError($"Failed to create goal: {response.Message}");
                }

                return response;
            }
        }

 public async Task<Response<GoalViewModel>> UpdateGoal(int id, UpdateGoalViewModel goalViewModel)
        {
            try
            {
                await AddBearerToken();

                //  AutoMapper to map everything (including enums)
                var updateGoalDto = _mapper.Map<UpdateGoalDto>(goalViewModel);
                updateGoalDto.Id = id;

                // Manually assign work items only if needed
                updateGoalDto.NewWorkItems = goalViewModel.GetNewWorkItems()
                    .Select(wi => _mapper.Map<CreateWorkItemDto>(wi)).ToList();

                updateGoalDto.UpdatedWorkItems = goalViewModel.GetExistingWorkItems()
                    .Select(wi => _mapper.Map<UpdateWorkItemDto>(wi)).ToList();

                updateGoalDto.DeletedWorkItemIds = goalViewModel.DeletedWorkItemIds;

                var goalDto = await _client.GoalsPUTAsync(id, updateGoalDto);

                _toastService.ShowSuccess("Goal updated successfully!");

                var viewModel = _mapper.Map<GoalViewModel>(goalDto);

                return new Response<GoalViewModel>
                {
                    Data = viewModel,
                    Success = true,
                    Message = "Goal updated successfully"
                };
            }
            catch (ApiException ex)
            {
                var errorMessage = ex.StatusCode switch
                {
                    404 => "Goal not found",
                    403 => "You don't have permission to update this goal",
                    400 => "Invalid goal data provided",
                    _ => "An error occurred while updating the goal"
                };

                _toastService.ShowError(errorMessage);

                return new Response<GoalViewModel>
                {
                    Success = false,
                    Message = errorMessage,
                };
            }
            catch (Exception ex)
            {
                const string errorMessage = "An unexpected error occurred while updating the goal";
                _toastService.ShowError(errorMessage);
                Console.WriteLine(ex);

                return new Response<GoalViewModel>
                {
                    Success = false,
                    Message = errorMessage
                };
            }
        }

 public async Task<PagedResult<GoalViewModel>> GetGoals(string searchPhrase = "", int pageNumber = 1, int pageSize = 10, string sortBy = "Title", int sortDirection = 0)
        {
            try
            {
                await AddBearerToken();

                var httpClient = _client.HttpClient;
                var response = await httpClient.GetAsync($"api/Goals?searchPhrase={Uri.EscapeDataString(searchPhrase)}&pageNumber={pageNumber}&pageSize={pageSize}&sortBy={sortBy}&sortDirection={sortDirection}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response: {content}");

                    var pagedResult = System.Text.Json.JsonSerializer.Deserialize<PagedResult<GoalDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (pagedResult?.Items == null)
                    {
                        Console.WriteLine("Deserialization resulted in null paged result");
                        return new PagedResult<GoalViewModel>
                        {
                            Items = new List<GoalViewModel>(),
                            TotalCount = 0,
                            PageSize = pageSize,
                            PageNumber = pageNumber
                        };
                    }

                    Console.WriteLine($"Deserialized {pagedResult.Items.Count()} goal(s) out of {pagedResult.TotalCount} total");

                    try
                    {
                        if (_mapper == null)
                        {
                            Console.WriteLine("ERROR: _mapper is NULL - this is the problem!");
                            return new PagedResult<GoalViewModel>
                            {
                                Items = new List<GoalViewModel>(),
                                TotalCount = 0,
                                PageSize = pageSize,
                                PageNumber = pageNumber
                            };
                        }

                        var viewModels = _mapper.Map<List<GoalViewModel>>(pagedResult.Items);

                        return new PagedResult<GoalViewModel>
                        {
                            Items = viewModels,
                            TotalCount = pagedResult.TotalCount,
                            PageSize = pagedResult.PageSize,
                            PageNumber = pagedResult.PageNumber
                        };
                    }
                    catch (Exception mapEx)
                    {
                        Console.WriteLine($"Mapping exception details: {mapEx}");
                        return new PagedResult<GoalViewModel>
                        {
                            Items = new List<GoalViewModel>(),
                            TotalCount = 0,
                            PageSize = pageSize,
                            PageNumber = pageNumber
                        };
                    }
                }
                else
                {
                    Console.WriteLine($"API returned non-success status code: {response.StatusCode}");
                }

                return new PagedResult<GoalViewModel>
                {
                    Items = new List<GoalViewModel>(),
                    TotalCount = 0,
                    PageSize = pageSize,
                    PageNumber = pageNumber
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching goals: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new PagedResult<GoalViewModel>
                {
                    Items = new List<GoalViewModel>(),
                    TotalCount = 0,
                    PageSize = pageSize,
                    PageNumber = pageNumber
                };
            }
        }

 private List<string> ParseValidationErrors(string response)
        {
            // Simple validation error parsing - you might want to enhance this
            // based on your API's error response format
            try
            {
                // This is a basic implementation - adjust based on your API's error format
                return new List<string> { response };
            }
            catch
            {
                return new List<string> { "Validation errors occurred" };
            }
        }

        public GoalViewModel CachedGoal { get; set; }
        public DetailGoalViewModel CachedDetailGoal { get; set; }
        public async Task<DetailGoalViewModel> GetGoalDetail(int id)

        {
            // Check if cached goal matches the requested ID
            if (CachedDetailGoal != null && CachedDetailGoal.Id == id)
            {
                var cached = CachedDetailGoal;
                CachedGoal = null; // Clear after using once
                return cached;
            }

            try
            {
                await AddBearerToken();
                var httpClient = _client.HttpClient;

                Console.WriteLine($"Making request to: api/Goals/{id}");

                var response = await httpClient.GetAsync($"api/Goals/{id}");

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response for Goal Detail: {content}");

                    var goal = System.Text.Json.JsonSerializer.Deserialize<GoalDto>(content);

                    if (goal == null)
                    {
                        Console.WriteLine("Deserialization resulted in null goal");
                        return null;
                    }

                    Console.WriteLine($"Deserialized Goal: Id={goal.Id}, Title={goal.Title}");

                    try
                    {
                      //  var viewModel = _mapper.Map<DetailGoalViewModel>(goal);
                        var viewModel = _mapper.Map<DetailGoalViewModel>(goal);
                       
                        return viewModel;
                    }
                    catch (Exception mapEx)
                    {
                        Console.WriteLine($"Mapping exception details: {mapEx}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"API returned non-success status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching goal detail: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }
    }
}
