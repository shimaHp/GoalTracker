using System.Net.Http;

namespace GoalTracker.UI.Blazor.Services.Base
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }
    }

}
