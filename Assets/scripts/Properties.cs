using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{

    class Properties
    {
        public enum PlayStyle{GOOD,BAD,NUETRAL};
        public static PlayStyle lastWin = PlayStyle.NUETRAL;

    }
}
