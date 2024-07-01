namespace Core.Library.Services.LocalStorages
{
    public interface ILocalStorage
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }
}
