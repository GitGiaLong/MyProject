﻿using Microsoft.Win32.SafeHandles;
using System.Security;

namespace Core.WPF.Interop
{
    internal abstract class WpfSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private readonly int _collectorId;

        [SecurityCritical]
        protected WpfSafeHandle(bool ownsHandle, int collectorId) : base(ownsHandle)
        {
            HandleCollector.Add(collectorId);
            _collectorId = collectorId;
        }

        [SecurityCritical, SecuritySafeCritical]
        protected override void Dispose(bool disposing)
        {
            HandleCollector.Remove(_collectorId);
            base.Dispose(disposing);
        }
    }
}