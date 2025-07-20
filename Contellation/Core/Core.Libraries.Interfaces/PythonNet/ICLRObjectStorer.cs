using Core.Libraries.PythonNet.StateSerialization;

namespace Core.Libraries.Interfaces.PythonNet
{
    public interface ICLRObjectStorer
    {
        ICollection<CLRMappedItem> Store(CLRWrapperCollection wrappers, Dictionary<string, object?> storage);
        CLRWrapperCollection Restore(Dictionary<string, object?> storage);
    }
}
