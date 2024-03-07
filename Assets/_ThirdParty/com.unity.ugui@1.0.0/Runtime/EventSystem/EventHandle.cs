using System;

namespace WildBoar.EventSystems
{
    [Flags]
    /// <summary>
    /// Enum that tracks event State.
    /// </summary>
    public enum EventHandle
    {
        Unused = 0,
        Used = 1
    }
}
