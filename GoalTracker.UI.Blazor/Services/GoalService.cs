using AutoMapper;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;
using System.Net.Http;
using System.Text.Json;

namespace GoalTracker.UI.Blazor.Services
{
    public class GoalService(IClient client, IMapper mapper) : BaseHttpService(client), IGoalService
    {
        public async Task<List<GoalViewModel>> GetGoals()
        {
            try
            {
                var httpClient = client.HttpClient;
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
                            var viewModels = mapper.Map<List<GoalViewModel>>(goals);
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

        public async Task<GoalViewModel> GetGoalDetail(int id)
        {
            try
            {
                var httpClient = client.HttpClient;
                var response = await httpClient.GetAsync($"api/Goals/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response for Goal Detail: {content}");

                    // Deserialize the goal directly
                    var goal = System.Text.Json.JsonSerializer.Deserialize<GoalDto>(content);

                    if (goal == null)
                    {
                        Console.WriteLine("Deserialization resulted in null goal");
                        return null;
                    }

                    Console.WriteLine($"Deserialized Goal: Id={goal.Id}, Title={goal.Title}");

                    try
                    {
                        var viewModel = mapper.Map<GoalViewModel>(goal);
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

        public async Task<Response<Guid>> CreateGoal(GoalViewModel goal)
        {
            try
            {
                var createGoalCommand = mapper.Map<CreateGoalCommand>(goal);
                await client.GoalsPOSTAsync(createGoalCommand);
                return new Response<Guid> { Success = true };
            }
            catch (ApiException ex) { 
            return ConvertApiExceptions<Guid>(ex);
            }

           
        }

        public async Task<Response<Guid>> DeleteGoal(GoalViewModel goal)
        {
            try
            {
                
                await client.GoalsDELETEAsync(goal.Id);
                return new Response<Guid> { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public async Task<Response<Guid>> UpdateGoal(int id, GoalViewModel goal)
        {
            try
            {
                var updateGoalTypeCommand = mapper.Map<UpdateGoalCommand>(goal);
                await client.GoalsPATCHAsync(id, updateGoalTypeCommand);
                return new Response<Guid> { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }
    }
}
