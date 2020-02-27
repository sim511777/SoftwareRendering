using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareRendering {
    public partial class FormMain : Form {
        private SceneManager scene;
        private InputManager input;

        public FormMain() {
            InitializeComponent();
            scene = new SceneManager();
            input = new InputManager(pbxDraw);
        }

        public static double GetTime() {
            return (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
        }

        private void FormMain_Shown(object sender, EventArgs e) {
            InitGame();
            GameLoop();
            FreeGame();
        }

        private void InitGame() {
        }

        private void FreeGame() {
        }

        private void GameLoop() {
            double timeOld = GetTime();
            while (true) {
                Application.DoEvents();
                if (this.IsDisposed)
                    break;

                ProcessInput();
                double timeNow = GetTime();
                double timeDelta = timeNow - timeOld;
                UpdateScene(timeDelta);
                timeOld = timeNow;
                RenderScene();
            }
        }

        private void ProcessInput() {
        }

        private void UpdateScene(double timeDelta) {
            scene.timeDelta = timeDelta;
            scene.mousePos = input.pos;
        }

        private void RenderScene() {
            pbxDraw.Invalidate();
        }

        private void pbxDraw_Paint(object sender, PaintEventArgs e) {
            DrawGraphics(e.Graphics);
        }

        private void DrawGraphics(Graphics gfx) {
            var st = GetTime();
            int step = 10;
            if (chkFillCircles.Checked) {
                for (int y = 0; y < 1000; y += step) {
                    for (int x = 0; x < 1000; x += step) {
                        gfx.FillEllipse(Brushes.Lime, x, y, step, step);
                    }
                }
            }
            var dt = GetTime() - st;
            string info = $"fps:{1.0 / scene.timeDelta:0} fps2:{1.0 / dt:0} time:{scene.timeDelta:0.000} pos:{scene.mousePos}";
            gfx.DrawString(info, Font, Brushes.Black, 0, 0);
        }
    }
}
