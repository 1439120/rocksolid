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

        public async Task Login(string email, string password)
        {
            try
            {
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./firebaseAuth.js");
                await module.InvokeVoidAsync("login", email, password);
                Message = "Logged in successfully!";
                IsLoggedIn=true;
                CurrentUserEmail=email;
                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error: " + ex.Message);
                Message = $"Error: {ex.Message}";
                IsLoggedIn=false;
            }
        }

        public async Task Register(string email, string password)
        {
            try
            {
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./firebaseAuth.js");
                await module.InvokeVoidAsync("register", email, password);
                Message = "Registered successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error: " + ex.Message);
                Message = $"Error: {ex.Message}";
            }
        }

        public void Logout()
        {
            IsLoggedIn = false;
            CurrentUserEmail = "";
            NotifyStateChanged();
        }
    }
}
