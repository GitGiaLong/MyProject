using Core.Library.Services;
using Core.Library.Services.LocalStorages;

namespace Core.Library.Helpers
{
    /// <summary>
    /// Use if not api
    /// </summary>
    public class FakeBackendHandler : HttpClientHandler
    {
        private ILocalStorage _localStorage;

        public FakeBackendHandler(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }
    }
}
