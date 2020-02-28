using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareRendering {
    public class Util {
        public static double GetTime() {
            return (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
        }

        public static void SetStyle(Control control, ControlStyles styles, bool flag) {
            MethodInfo method = control.GetType().GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(control, new object[] { styles, flag });
        }
    }
}
