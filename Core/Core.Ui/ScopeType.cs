using System;

namespace Core.Ui
{
    [Flags]
    public enum ScopeType
    {
        None = 0,
        Main = 1 << 0,
        PopUps = 1 << 1,
        All = Main | PopUps
    }
}