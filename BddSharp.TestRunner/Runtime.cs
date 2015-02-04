using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BddSharp.TestRunner
{
    public class Runtime
    {
        public static bool IsAdmin { get; set; }

        static Runtime()
        {
            IsAdmin = false;
        }
    }
}
