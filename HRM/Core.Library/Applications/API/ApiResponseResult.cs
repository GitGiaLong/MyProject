using Core.Entities.Applications.Base.Responses;
using Core.Entities.Applications.Connects.API;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Core.Library.Applications.API
{
    /// <summary>
    /// Phản hồi kết quả
    /// </summary>
    public class ApiResponseResult
    {
        HttpClient client;
        ConnectAPI api = new ConnectAPI();

        public ApiResponseResult()
        {
            client = new HttpClient { BaseAddress = new Uri(api.Url) };
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + api.Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_client.Timeout = TimeSpan.FromSeconds(EDU1_4Base.TimeOut);

            // Use SecurityProtocolType SSL
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
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
            T? Data = default(T?);
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
