using Core.Entities.Applications.Connects.API;
using Core.Entities.Managements.User;
using Core.Library.Services.HttpServices;
using Core.Library.Services.LocalStorages;
using Microsoft.AspNetCore.Components;

namespace Core.Library.Authentications
{
    public class Authentication : IAuthentication
    {
        private IHttpService _httpClient;
        private NavigationManager _navigationManager;
        private ILocalStorage _localStorage;
        private string _userKey = "user";
        public User User { get; set; }
        private bool _IsLogin = false;
        public bool IsLogin 
        { 
            get 
            {
                if (User != null)
                    _IsLogin = true;
                return _IsLogin; 
            } 
            set { _IsLogin = value; } }
        public Authentication(ILocalStorage localStorage, IHttpService httpClient)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task Initialize()
        {
            User = await _localStorage.GetItem<User>("user");
        }

        //public async Task Login(User model)
        public async Task Login()
        {
            var _User = await _httpClient.AsyncActionApi<ConnectAPI>("Authentication", EMethodApi.Post, User);
            await _localStorage.SetItem(_userKey, User);
        }

        public async Task Logout()
        {
            User = new User();
            await _localStorage.RemoveItem(_userKey);
            _navigationManager.NavigateTo("/login");
        }
    }
}
