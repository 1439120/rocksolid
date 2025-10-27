using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace BlazorApp.Authentication{
    public class CustomAuthenticationProvider: AuthenticationStateProvider{
        // private readonly ProtectedSessionStorage _sessionStorage;
        private readonly IMemoryCache _cache;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userSession = _cache.Get<UserSession>("UserSession");
            
            if (userSession == null)
                return Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Role, userSession.Role),
                new Claim("UserId", userSession.UserID),
            }, "CustomAuth"));

            return Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public Task UpdateAuthenticationState(UserSession? userSession)
        {
            if (userSession != null)
                _cache.Set("UserSession", userSession, TimeSpan.FromHours(8));
            else
                _cache.Remove("UserSession");

            var claimsPrincipal = userSession != null 
                ? new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role),
                    new Claim("UserId", userSession.UserID),
                }, "CustomAuth"))
                : _anonymous;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
            return Task.CompletedTask;
        }

    }
}