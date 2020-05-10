using SoftwareRendering.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        private void btnStart_Click(object sender, EventArgs e) {
            var buf = Resources.start;
            scene.LoadBsp(buf);
        }

        private void btnE1M1_Click(object sender, EventArgs e) {
            var buf = Resources.e1m1;
            scene.LoadBsp(buf);
        }

        private void btnLoad_Click(object sender, EventArgs e) {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return;

            var buf = File.ReadAllBytes(dlgOpen.FileName);
            scene.LoadBsp(buf);
        }
    }
}
