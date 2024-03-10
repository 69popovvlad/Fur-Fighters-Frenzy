using System.Collections.Generic;
using System.Reflection;

namespace Core.Ioc
{
    internal class ConstructorsComparer: IComparer<ConstructorInfo>
    {
        public int Compare(ConstructorInfo x, ConstructorInfo y)
        {
            if (x == null || y == null)
            {
                return -1;
            }

            var lx = x.GetParameters().Length;
            var ly = y.GetParameters().Length;

            if (lx == ly)
            {
                return 0;
            }

            return lx > ly ? -1 : 1;
        }
    }
}