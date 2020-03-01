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

        public unsafe static void Memset(IntPtr dbuf, int val, int count) {
            byte* dp = (byte*)dbuf;
            byte bval = (byte)val;
            for (int i = 0; i < count; i++, dp++) {
                *dp = bval;
            }
        }

        public unsafe static void Memset4(IntPtr dbuf, int val, int count) {
            int* dp = (int*)dbuf;
            for (int i = 0; i < count; i++, dp++) {
                *dp = val;
            }
        }
        public unsafe static void Memcpy(IntPtr dbuf, IntPtr sbuf, int count) {
            byte* sp = (byte*)sbuf;
            byte* dp = (byte*)dbuf;
            for (int i = 0; i < count; i++, dp++) {
                *dp = *sp;
            }
        }

        public unsafe static void Memcpy4(IntPtr dbuf, IntPtr sbuf, int count) {
            int* sp = (int*)sbuf;
            int* dp = (int*)dbuf;
            for (int i = 0; i < count; i++, dp++) {
                *dp = *sp;
            }
        }
    }

    public static class ControlExtension {
        public static void SetStyle(this Control ctrl, ControlStyles style, bool val) {
            var method = typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] prms = { ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true };
            method.Invoke(ctrl, prms);
        }
    }
}
