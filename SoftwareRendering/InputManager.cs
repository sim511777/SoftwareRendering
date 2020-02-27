using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;

namespace SoftwareRendering {
    public class InputManager {
        private Control control;
        public InputManager(Control control) {
            this.control = control;
        }

        public bool this[Key key] => Keyboard.IsKeyDown(key);

        public bool this[MouseButtons mbtn] => Control.MouseButtons.HasFlag(mbtn);

        public Point pos => control.PointToClient(Control.MousePosition);
    }
}
