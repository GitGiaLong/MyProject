using Core.Libraries.PythonNet.PythonTypes;

using static Core.Libraries.PythonNet.Runtimes.Runtime;

namespace Core.Libraries.PythonNet
{
    public static class ConverterExtension
    {
        public static PyObject ToPython(this object? o)
        {
            if (o is null) { return None; }
            return Converter.ToPython(o, o.GetType()).MoveToPyObject();
        }

        public static PyObject ToPythonAs<T>(this T? o)
        {
            if (o is null) { return None; }
            return Converter.ToPython(o, typeof(T)).MoveToPyObject();
        }
    }
}
