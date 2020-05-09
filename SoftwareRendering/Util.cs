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

        public static unsafe void Memset(IntPtr dbuf, int val, int count) {
            byte* dp = (byte*)dbuf;
            byte bval = (byte)val;
            for (int i = 0; i < count; i++, dp++) {
                *dp = bval;
            }
        }

        public static unsafe void Memset4(IntPtr dbuf, int val, int count) {
            int* dp = (int*)dbuf;
            for (int i = 0; i < count; i++, dp++) {
                *dp = val;
            }
        }

        public static unsafe void Memcpy(IntPtr dbuf, IntPtr sbuf, int count) {
            byte* sp = (byte*)sbuf;
            byte* dp = (byte*)dbuf;
            for (int i = 0; i < count; i++, dp++) {
                *dp = *sp;
            }
        }

        public static unsafe void Memcpy4(IntPtr dbuf, IntPtr sbuf, int count) {
            int* sp = (int*)sbuf;
            int* dp = (int*)dbuf;
            for (int i = 0; i < count; i++, dp++) {
                *dp = *sp;
            }
        }
    }
}
