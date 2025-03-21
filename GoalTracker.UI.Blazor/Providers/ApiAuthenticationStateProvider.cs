using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GoalTracker.UI.Blazor.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly JwtSecurityTokenHandler jweSecurityTokenHandler;
        public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
            jweSecurityTokenHandler = new JwtSecurityTokenHandler();
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var isTokenPresent = await localStorage.ContainKeyAsync("token");

            if (isTokenPresent)
            {
                return new AuthenticationState(user);
            }

            var savedToken = await localStorage.GetItemAsync<string>("token");
            try
            {
                var tokenContent = jweSecurityTokenHandler.ReadJwtToken(savedToken);

                if (tokenContent.ValidTo < DateTime.Now)
                {
                    // Token expired, remove it and return unauthenticated user
                    await localStorage.RemoveItemAsync("token");
                    return new AuthenticationState(user);
                }

                var claims = await GetClaims();
                user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            }
            catch (Exception)
            {
                // Invalid token format, remove it
                await localStorage.RemoveItemAsync("token");
            }

            return new AuthenticationState(user);
        }

        public async Task LoggedIn()
        {
            var claims = await GetClaims();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoggedOut()
        {
            await localStorage.RemoveItemAsync("token");
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }


        private async Task<List<Claim>> GetClaims_()
        {
            var savedToken = await localStorage.GetItemAsync<string>("token");
            var tokenContent = jweSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
        private async Task<List<Claim>> GetClaims()
        {
            var savedToken = await localStorage.GetItemAsync<string>("token");
            var tokenContent = jweSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims.ToList();

            // Check if we already have a Name claim
            if (!claims.Any(c => c.Type == ClaimTypes.Name))
            {
                // Try to find a suitable claim to use as Name
                var nameClaim = claims.FirstOrDefault(c =>
                    c.Type == "name" ||
                    c.Type == "email" ||
                    c.Type == "preferred_username" ||
                    c.Type == "sub");

                if (nameClaim != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
                }
            }

            return claims;
        }


    }
}
