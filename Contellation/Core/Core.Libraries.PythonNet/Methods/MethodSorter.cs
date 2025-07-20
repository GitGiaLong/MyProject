using Core.Libraries.Structs.PythonNet.Method;
using System.Reflection;

namespace Core.Libraries.PythonNet.Methods
{
    using MaybeMethodBase = MaybeMethodBase<MethodBase>;

    /// <summary>
    /// Utility class to sort method info by parameter type precedence.
    /// </summary>
    internal class MethodSorter : IComparer<MaybeMethodBase>
    {
        int IComparer<MaybeMethodBase>.Compare(MaybeMethodBase m1, MaybeMethodBase m2)
        {
            MethodBase me1 = m1.UnsafeValue;
            MethodBase me2 = m2.UnsafeValue;
            if (me1 == null && me2 == null) { return 0; }
            else if (me1 == null) { return -1; }
            else if (me2 == null) { return 1; }

            if (me1.DeclaringType != me2.DeclaringType)
            {
                // m2's type derives from m1's type, favor m2
                if (me1.DeclaringType.IsAssignableFrom(me2.DeclaringType)) { return 1; }

                // m1's type derives from m2's type, favor m1
                if (me2.DeclaringType.IsAssignableFrom(me1.DeclaringType)) { return -1; }
            }

            int p1 = MethodBinder.GetPrecedence(me1);
            int p2 = MethodBinder.GetPrecedence(me2);
            
            if (p1 < p2) { return -1; }
            if (p1 > p2) { return 1; }
            
            return 0;
        }
    }
}
