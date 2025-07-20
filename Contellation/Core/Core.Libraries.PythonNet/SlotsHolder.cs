using Core.Libraries.PythonNet.PythonTypes;
using Core.Libraries.PythonNet.Runtimes;
using Core.Libraries.PythonNet.TypeOffsets;
using Core.Libraries.PythonNet.Types;
using Core.Libraries.PythonNet.Utils;
using Core.Libraries.Structs.PythonNet.References;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet
{
    class SlotsHolder
    {
        public delegate void Resetor(PyType type, int offset);

        private readonly Dictionary<int, ThunkInfo> _slots = new();
        private readonly List<ThunkInfo> _keepalive = new();
        private readonly Dictionary<int, Resetor> _customResetors = new();
        private readonly List<Action> _deallocators = new();
        private bool _alreadyReset = false;

        private readonly PyType Type;

        public string?[] Holds => _slots.Keys.Select(TypeOffset.GetSlotName).ToArray();

        /// <summary>
        /// Create slots holder for holding the delegate of slots and be able  to reset them.
        /// </summary>
        /// <param name="type">Steals a reference to target type</param>
        public SlotsHolder(PyType type)
        {
            this.Type = type;
        }

        public bool IsHolding(int offset) => _slots.ContainsKey(offset);

        public ICollection<int> Slots => _slots.Keys;

        public void Set(int offset, ThunkInfo thunk)
        {
            _slots[offset] = thunk;
        }

        public void Set(int offset, Resetor resetor)
        {
            _customResetors[offset] = resetor;
        }

        public void AddDealloctor(Action deallocate)
        {
            _deallocators.Add(deallocate);
        }

        public void KeeapAlive(ThunkInfo thunk)
        {
            _keepalive.Add(thunk);
        }

        public static void ResetSlots(BorrowedReference type, IEnumerable<int> slots)
        {
            foreach (int offset in slots)
            {
                IntPtr ptr = GetDefaultSlot(offset);
#if DEBUG
                //DebugUtil.Print($"Set slot<{TypeOffsetHelper.GetSlotNameByOffset(offset)}> to 0x{ptr.ToString("X")} at {typeName}<0x{_type}>");
#endif
                Util.WriteIntPtr(type, offset, ptr);
            }
        }

        public void ResetSlots()
        {
            if (_alreadyReset)
            {
                return;
            }
            _alreadyReset = true;
#if DEBUG
            IntPtr tp_name = Util.ReadIntPtr(Type, TypeOffset.tp_name);
            string typeName = Marshal.PtrToStringAnsi(tp_name);
#endif
            ResetSlots(Type, _slots.Keys);

            foreach (var action in _deallocators)
            {
                action();
            }

            foreach (var pair in _customResetors)
            {
                int offset = pair.Key;
                var resetor = pair.Value;
                resetor?.Invoke(Type, offset);
            }

            _customResetors.Clear();
            _slots.Clear();
            _keepalive.Clear();
            _deallocators.Clear();

            // Custom reset
            if (Type != Runtime.CLRMetaType)
            {
                var metatype = Runtime.PyObject_TYPE(Type);
                ManagedType.TryFreeGCHandle(Type, metatype);
            }
            Runtime.PyType_Modified(Type);
        }

        public static IntPtr GetDefaultSlot(int offset)
        {
            if (offset == TypeOffset.tp_clear)
            {
                return TypeManager.subtype_clear;
            }
            else if (offset == TypeOffset.tp_traverse)
            {
                return TypeManager.subtype_traverse;
            }
            else if (offset == TypeOffset.tp_dealloc)
            {
                // tp_free of PyTypeType is point to PyObejct_GC_Del.
                return Util.ReadIntPtr(Runtime.PyTypeType, TypeOffset.tp_free);
            }
            else if (offset == TypeOffset.tp_free)
            {
                // PyObject_GC_Del
                return Util.ReadIntPtr(Runtime.PyTypeType, TypeOffset.tp_free);
            }
            else if (offset == TypeOffset.tp_call)
            {
                return IntPtr.Zero;
            }
            else if (offset == TypeOffset.tp_new)
            {
                // PyType_GenericNew
                return Util.ReadIntPtr(Runtime.PySuper_Type, TypeOffset.tp_new);
            }
            else if (offset == TypeOffset.tp_getattro)
            {
                // PyObject_GenericGetAttr
                return Util.ReadIntPtr(Runtime.PyBaseObjectType, TypeOffset.tp_getattro);
            }
            else if (offset == TypeOffset.tp_setattro)
            {
                // PyObject_GenericSetAttr
                return Util.ReadIntPtr(Runtime.PyBaseObjectType, TypeOffset.tp_setattro);
            }

            return Util.ReadIntPtr(Runtime.PyTypeType, offset);
        }
    }
}
