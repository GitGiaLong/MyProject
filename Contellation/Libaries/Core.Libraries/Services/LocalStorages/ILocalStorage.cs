namespace Core.Libraries.Services.LocalStorages
{
    public interface ILocalStorage
    {
        /// <summary>
        /// Get Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetItem<T>(string key);

        /// <summary>
        /// Set Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetItem<T>(string key, T value);

        /// <summary>
        /// Remove Item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveItem(string key);
    }
}
