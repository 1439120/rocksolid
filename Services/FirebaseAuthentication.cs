using Microsoft.JSInterop;

namespace BlazorApp.Services
{
    public class FirebaseAuthentication
    {
        private readonly IJSRuntime _js;
        public string Message { get; private set; } = "";
        public bool IsLoggedIn{get; private set;} = false;
        public string CurrentUserEmail{get; private set;} = "";

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public FirebaseAuthentication(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string> Login(string email, string password)
        {
            try
            {
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./firebaseAuth.js");
                var UserId = await module.InvokeAsync<string>("login", email, password);
                Message = "Logged in successfully!";
                IsLoggedIn=true;
                CurrentUserEmail=email;
                NotifyStateChanged();
                return UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error: " + ex.Message);
                Message = $"Error: {ex.Message}";
                IsLoggedIn=false;
                return "";
            }
        }

        public async Task<string> Register(string email, string password)
        {
            try
            {
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./firebaseAuth.js");
                var UserId = await module.InvokeAsync<string>("register", email, password);
                Message = "Registered successfully!";
                return UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error: " + ex.Message);
                Message = $"Error: {ex.Message}";
                return "";
            }
        }

        public async void Logout()
        {
            IsLoggedIn = false;
            CurrentUserEmail = "";
            NotifyStateChanged();
            await _js.InvokeVoidAsync("logout", "./firebaseAuth.js");
        }
    }
}
