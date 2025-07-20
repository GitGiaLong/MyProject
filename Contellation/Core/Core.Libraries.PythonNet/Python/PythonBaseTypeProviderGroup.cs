using Core.Libraries.Interfaces.PythonNet;
using Core.Libraries.PythonNet.PythonTypes;

namespace Core.Libraries.PythonNet.Python
{
    class PythonBaseTypeProviderGroup : List<IPythonBaseTypeProvider>, IPythonBaseTypeProvider
    {
        public IEnumerable<PyType> GetBaseTypes(Type type, IList<PyType> existingBases)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            if (existingBases is null) { throw new ArgumentNullException(nameof(existingBases)); }

            foreach (var provider in this)
            {
                existingBases = provider.GetBaseTypes(type, existingBases).ToList();
            }

            return existingBases;
        }
    }
}
