using Core.Libraries.PythonNet.References;
using Core.Libraries.PythonNet.Types;
using Core.Libraries.Structs.PythonNet.References;

namespace Core.Libraries.PythonNet.StateSerialization
{
    [Serializable]
    internal class UnloadedClass : ClassBase
    {
        readonly string name;

        internal UnloadedClass(string name) : base(typeof(object)) { this.name = name; }

        public static NewReference tp_new(BorrowedReference tp, BorrowedReference args, BorrowedReference kw)
        {
            var self = (UnloadedClass)GetManagedObject(tp)!;
            return self.RaiseTypeError();
        }

        public override NewReference type_subscript(BorrowedReference idx) => RaiseTypeError();

        private NewReference RaiseTypeError() => Exceptions.RaiseTypeError("The .NET type no longer exists: " + name);

        internal override bool CanSubclass() => false;
    }
}
