using Core.Libraries.Services.LocalStorages;
using Microsoft.AspNetCore.Components;

namespace Core.Libraries.Services.HttpServices
{

    public class HttpService : IHttpService
    {

        HttpClient client { get; set; }
        NavigationManager _navigationManager { get; set; }
        ILocalStorage _localStorageService { get; set; }
        //ConnectAPI api { get; set; } = new ConnectAPI();

    }
}
