using Core.Entities.Applications.Base.Responses;
using Core.Entities.Applications.Connects.API;

namespace Core.Library.Services.HttpServices
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
