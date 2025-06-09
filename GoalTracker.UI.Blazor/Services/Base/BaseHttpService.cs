﻿namespace GoalTracker.UI.Blazor.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        public BaseHttpService(IClient client)
        {
            _client = client;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
        {

            if (exception.StatusCode == 400)
            {
                return new Response<Guid> 
                { Message = "Invalid data was submitted",
                    ValidationErrors = exception.Response, Success = false };
            }
            else if (exception.StatusCode == 404)
            {
                return new Response<Guid>
                {
                    Message = "The record was not found",
                    ValidationErrors = exception.Response,
                    Success = false
                };
            }
            else { return new Response<Guid>() { Message="Somthing went wrong, please try again later",Success = false}; }
        }
    }
}
