using Core.Libraries.Enums.PythonNet;
using Core.Libraries.PythonNet.Python;
using Core.Libraries.PythonNet.References;
using Core.Libraries.PythonNet.Runtimes;
using Core.Libraries.Structs.PythonNet.References;

namespace Core.Libraries.Helpers.PythonNet
{
    static class SlotHelper
    {
        public static NewReference CreateObjectType()
        {
            using var globals = Runtime.PyDict_New();
            if (Runtime.PyDict_SetItemString(globals.Borrow(), "__builtins__", Runtime.PyEval_GetBuiltins()) != 0)
            {
                globals.Dispose();
                throw PythonException.ThrowLastAsClrException();
            }
            const string code = "class A(object): pass";
            using var resRef = Runtime.PyRun_String(code, RunFlagType.File, globals.Borrow(), globals.Borrow());
            if (resRef.IsNull())
            {
                globals.Dispose();
                throw PythonException.ThrowLastAsClrException();
            }
            resRef.Dispose();
            BorrowedReference A = Runtime.PyDict_GetItemString(globals.Borrow(), "A");
            return new NewReference(A);
        }
    }
}
