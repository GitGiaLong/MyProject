using Core.Entities.Enums.MethodApi;
using Core.Libraries.Services.LocalStorages;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using Core.Entities.Connects;
using Core.Libraries.Models;

namespace Core.Libraries.Services.HttpServices
{


    public class HttpService : IHttpService
    {
        HttpClient client { get; set; }
        NavigationManager _navigationManager { get; set; }
        ILocalStorage _localStorageService { get; set; }
        Connect api { get; set; } = new Connect();

        public HttpService(
            NavigationManager navigationManager,
            ILocalStorage localStorageService)
        {
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;

            client = new HttpClient { BaseAddress = new Uri(api.Url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + api.Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //_client.Timeout = TimeSpan.FromSeconds(api.TimeOut);

            //// Use SecurityProtocolType SSL
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        }

        public async Task<ApiRestFul<T>> AsyncActionApi<T>(string urlParameters, EMethodApi TypeActionApi = EMethodApi.Get, object? Data = null)
        {
            ApiRestFul<T?> value = default(ApiRestFul<T?>);
            try
            {
                HttpResponseMessage? response;

                switch (TypeActionApi)
                {
                    case EMethodApi.Get:
                        value = await Request<ApiRestFul<T?>>(await client.GetAsync(urlParameters).ConfigureAwait(false));
                        break;
                    case EMethodApi.Post:
                        value = await Request<ApiRestFul<T?>>(await client.PostAsJsonAsync(urlParameters, Data).ConfigureAwait(false));
                        //Select = await Request<T>(await client.PostAsync(urlParameters, Data).ConfigureAwait(false));
                        break;
                    case EMethodApi.Put:
                        value = await Request<ApiRestFul<T?>>(await client.PutAsJsonAsync(urlParameters, Data).ConfigureAwait(false));
                        //Select = await Request<T>(await client.PutAsync(urlParameters, Data).ConfigureAwait(false));
                        break;
                    case EMethodApi.Delete:
                        value = await Request<ApiRestFul<T?>>(await client.DeleteAsync(urlParameters).ConfigureAwait(false));
                        break;
                }

                return value;
            }
            catch (Exception ex)
            {
                return value;
            }
        }

        private async Task<T> Request<T>(HttpResponseMessage response, string Toats = null)
        {
            T? Data = default;
            try
            {
                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var _request = await response.Content.ReadFromJsonAsync<T>();
                    return Data = _request;
                    //string json = await response.Content.ReadAsStringAsync();
                }
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                {
                }
                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _navigationManager.NavigateTo("/logout");
                    return Data;
                }
                return Data;
            }
            catch (Exception ex)
            {
                return Data;
            }
        }
    }
}
