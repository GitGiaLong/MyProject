using Core.Libraries.Enums.PythonNet;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.PythonTypes
{
    public class TypeSpec
    {
        public TypeSpec(string name, int basicSize, IEnumerable<Slot> slots, TypeFlags flags, int itemSize = 0)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.BasicSize = basicSize;
            this.Slots = slots.ToArray();
            this.Flags = flags;
            this.ItemSize = itemSize;
        }
        public string Name { get; }
        public int BasicSize { get; }
        public int ItemSize { get; }
        public TypeFlags Flags { get; }
        public IReadOnlyList<Slot> Slots { get; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Slot
        {
            public Slot(TypeSlotID id, IntPtr value)
            {
                ID = id;
                Value = value;
            }

            public TypeSlotID ID { get; }
            public IntPtr Value { get; }
        }
    }
}
