using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser {
    public static class Extensions {
        public static bool IsDash(this char ch) {
            if (ch == '-' || ch == '\u2011' || ch == '\u2010')
            {
                return true;
            }
            return false;
        }
    }
}
