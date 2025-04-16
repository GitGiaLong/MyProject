using Core.Entities.Enums.MethodApi;
using Core.Libraries.Models;

namespace Core.Libraries.Services.HttpServices
{
    public interface IHttpService
    {
        /// <summary>
        /// Async Action Api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlParameters"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public Task<ApiRestFul<T>> AsyncActionApi<T>(string urlParameters, EMethodApi TypeActionApi = EMethodApi.Get, object? Data = null);
    }
}
