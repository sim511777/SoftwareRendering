using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareRendering {
    public partial class FormMain : Form {
        private RenderManager renderer = new RenderManager();
        private InputManager input;
        private SceneManager scene;

        public FormMain() {
            InitializeComponent();
        }

        private void FormMain_Shown(object sender, EventArgs e) {
            InitGame();
            GameLoop();
            FreeGame();
        }

        private void InitGame() {
            input = new InputManager(pbxDraw);
            scene = new SceneManager();
        }

        private void FreeGame() {
            renderer.FreeBuffer();
        }

        private void pbxDraw_Layout(object sender, LayoutEventArgs e) {
            renderer.ReallocBuffer(pbxDraw);
        }

        private void GameLoop() {
            double timeOld = Util.GetTime();
            while (true) {
                Application.DoEvents();

                if (this.IsDisposed)
                    break;

                double timeNow = Util.GetTime();
                double timeDelta = timeNow - timeOld;
                timeOld = timeNow;
                
                ProcessInput();
                UpdateScene(timeDelta);
                Render();
            }
        }

        private void ProcessInput() {
        }

        private void UpdateScene(double timeDelta) {
            scene.Update(timeDelta, input.Pos);
        }

        private void Render() {
            renderer.Draw(scene);
        }
    }
}
