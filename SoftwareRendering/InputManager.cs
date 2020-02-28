using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SoftwareRendering {
    public class InputManager {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        private Control control;
        public InputManager(Control control) {
            this.control = control;
        }

        public bool this[Keys key] {
            get {
                return (GetAsyncKeyState(key) | 0x8000) != 0;
            }
        }

        public bool this[MouseButtons mbtn] {
            get {
                return Control.MouseButtons.HasFlag(mbtn);
            }
        }

        public Point Pos {
            get {
                return control.PointToClient(Control.MousePosition);
            }
        }
    }
}
