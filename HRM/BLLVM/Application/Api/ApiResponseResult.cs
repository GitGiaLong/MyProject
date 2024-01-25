using Entities.Application.Connect.Api;
using Entities.Application.Convert;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BLLVM.Application.Api
{
    public class ApiResponseResult
    {
        HttpClient client;
        ConnectApi api = new ConnectApi();

        public ApiResponseResult()
        {
            client = new HttpClient { BaseAddress = new Uri(api.Url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + api.Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_client.Timeout = TimeSpan.FromSeconds(EDU1_4Base.TimeOut);

            // Use SecurityProtocolType SSL
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        }


        public async Task<T> AsyncActionApi<T>(string urlParameters, EnumMethodApi TypeActionApi = EnumMethodApi.A, object Data = null)
        {
            T? value = default(T);
            try
            {
                HttpResponseMessage? response;

                switch (TypeActionApi)
                {
                    case EnumMethodApi.A:
                        value = await Request<T>(await client.GetAsync(urlParameters).ConfigureAwait(false));
                        break;
                    case EnumMethodApi.B:
                        value = await Request<T>(await client.PutAsJsonAsync(urlParameters, Data).ConfigureAwait(false));
                        break;
                    case EnumMethodApi.C:
                        value = await Request<T>(await client.PutAsJsonAsync(urlParameters, Data).ConfigureAwait(false));
                        break;
                    case EnumMethodApi.D:
                        value = await Request<T>(await client.DeleteAsync(urlParameters).ConfigureAwait(false));
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
                    var _request = await response.Content.ReadFromJsonAsync<ApiRestFul<T>>();
                    return Data = _request.Data;
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
