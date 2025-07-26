using Core.Libraries.Interfaces.PythonNet;
using Core.Libraries.PythonNet.Mixins;
using Core.Libraries.PythonNet.PY;
using Core.Libraries.PythonNet.Python;
using Core.Libraries.PythonNet.PythonTypes;

namespace Core.Libraries.PythonNet
{

    public sealed class InteropConfiguration : IDisposable
    {
        internal readonly PythonBaseTypeProviderGroup pythonBaseTypeProviders = new();

        /// <summary>
        /// Enables replacing base types of CLR types as seen from Python
        /// </summary>
        public IList<IPythonBaseTypeProvider> PythonBaseTypeProviders => this.pythonBaseTypeProviders;

        public static InteropConfiguration MakeDefault()
        {
            return new InteropConfiguration
            {
                PythonBaseTypeProviders =
                {
                    DefaultBaseTypeProvider.Instance,
                    new CollectionMixinsProvider(new Lazy<PyObject>(() => Py.Import("clr._extras.collections"))),
                },
            };
        }

        public void Dispose()
        {
            foreach (var provider in PythonBaseTypeProviders.OfType<IDisposable>())
            {
                provider.Dispose();
            }
            PythonBaseTypeProviders.Clear();
        }
    }
}
