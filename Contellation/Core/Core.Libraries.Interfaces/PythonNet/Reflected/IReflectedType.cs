namespace Core.Libraries.Interfaces.PythonNet.Reflected
{
    internal interface IReflectedType
    {
        string PythonTypeName();
        Type GetReflectedType();
    }
}
