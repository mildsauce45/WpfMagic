using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfMagic
{
    public class AmbiguousViewTypeException : Exception
    {
        public AmbiguousViewTypeException()
        {
        }

        public AmbiguousViewTypeException(string message)
            : base(message)
        {
        }
    }
}
