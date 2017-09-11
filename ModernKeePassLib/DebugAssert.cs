using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernKeePassLib
{
    public class Debug
    {
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string why)
        {
            if (!condition)
                System.Diagnostics.Debugger.Break();

        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if (!condition)
                System.Diagnostics.Debugger.Break();

        }

    }

}
