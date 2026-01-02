using System.Diagnostics;

namespace NET_8_0
/*=============
using classic style namespace declaration because .NET 8 duplicates the namespace calls
without it.  Example to call main method:
- With:     Embedded_IPython.Program.Main()
- Without:  Embedded_IPython.Embedded_IPython.Program.Main()
=============*/
{
    internal static partial class Program
        {
        static string QuoteForCmd(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "\"\"";

            return "\"" + s.Replace("\"", "\\\"") + "\"";
        } // static string QuoteForCmd(string s)
    } // internal static class Program
} // namespace Embedded_Python