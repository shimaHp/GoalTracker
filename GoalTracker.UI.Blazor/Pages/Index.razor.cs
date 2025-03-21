using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;


namespace GoalTracker.UI.Blazor.Pages
{
    public partial class Index
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }
        [Inject]

        public  IAuthenticationService AuthenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
           await ((ApiAuthenticationStateProvider)
                AuthenticationStateProvider).GetAuthenticationStateAsync();

        }

        protected void GoToLogin()
        {
            NavigationManager.NavigateTo("login/");
        }
        protected void GoToDashboard()
        {
            NavigationManager.NavigateTo("/goals");
        }
        protected void GoToLogout()
        {
            NavigationManager.NavigateTo("login/");
        }
        protected void GoToRegister()
        {
            NavigationManager.NavigateTo("register/");
        }



    }
}
