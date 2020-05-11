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

        public bool WheelUp { get; private set; }
        public bool WheelDown { get; private set; }

        private Control control;
        public bool viewFocus = false;
        public Point viewCenterPt =  Point.Empty;
        public InputManager(Control _control) {
            control = _control;
            control.MouseWheel += this.Control_MouseWheel;
            control.MouseDown += Control_MouseDown;
            control.KeyDown += Control_KeyDown;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e) {
            viewFocus = true;
            control.Cursor = Cursors.No;
            viewCenterPt = Pos;
        }

        private void Control_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                viewFocus = false;
                control.Cursor = Cursors.Arrow;
            }
        }

        private void Control_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta > 0)
                WheelUp = true;
            if (e.Delta < 0)
                WheelDown = true;
        }

        public void MouseWheelReset() {
            WheelUp = false;
            WheelDown = false;
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
                return control.PointToClient(Cursor.Position);
            } set {
                Cursor.Position = control.PointToScreen(value);
            }
        }
    }
}
