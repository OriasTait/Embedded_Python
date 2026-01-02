using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedded_Python
{
    internal static partial class Program
    {
        static string QuoteForCmd(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "\"\"";

            return "\"" + s.Replace("\"", "\\\"") + "\"";
        } // static string QuoteForCmd(string s)
    } // internal class Program
} // Embedded_Python
