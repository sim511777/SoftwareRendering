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
        private SceneManager scene;
        private InputManager input;

        private Bitmap backBuffer;
        private IntPtr imgBuf;
        private int bw;
        private int bh;
        private int step;

        public FormMain() {
            InitializeComponent();
            input = new InputManager(pbxDraw);
            scene = new SceneManager();
            ReallocBuffer();
        }

        private void ReallocBuffer() {
            if (backBuffer != null) {
                backBuffer.Dispose();
                Marshal.FreeHGlobal(imgBuf);
            }

            bw = pbxDraw.Width;
            bh = pbxDraw.Height;
            step = bw * 4;
            imgBuf = Marshal.AllocHGlobal(step * bh);
            backBuffer = new Bitmap(bw, bh, step, PixelFormat.Format32bppPArgb, imgBuf);
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
            double timeOld = Util.GetTime();
            while (true) {
                Application.DoEvents();

                if (this.IsDisposed)
                    break;

                ProcessInput();
                double timeNow = Util.GetTime();
                UpdateScene(timeNow - timeOld);
                timeOld = timeNow;
                RenderScene();
            }
        }

        private void ProcessInput() {
        }

        private void UpdateScene(double timeDelta) {
            scene.timeDelta = timeDelta;
            scene.mousePos = input.Pos;
        }

        private void RenderScene() {
            pbxDraw.Invalidate();
        }

        private void pbxDraw_Paint(object sender, PaintEventArgs e) {
            using (Graphics g = Graphics.FromImage(backBuffer)) {
                DrawImgBuf();
                DrawGraphics(g);
            }
            e.Graphics.DrawImageUnscaledAndClipped(backBuffer, pbxDraw.ClientRectangle);
        }

        private unsafe void DrawImgBuf() {
            int red = Color.Red.ToArgb();
            int* ptr = (int *)imgBuf;
            for (int i = 100; i < 200; i++) {
                *(ptr + bw * i + i) = red;
            }
        }

        private void DrawGraphics(Graphics gfx) {
            gfx.Clear(Color.White);
            var st = Util.GetTime();
            int step = 10;
            if (chkFillCircles.Checked) {
                for (int y = 0; y < 1000; y += step) {
                    for (int x = 0; x < 1000; x += step) {
                        gfx.FillEllipse(Brushes.Lime, x, y, step, step);
                    }
                }
            }
            var dt = Util.GetTime() - st;
            string info = string.Format("fps:{0:0} fps2:{1:0} time:{2:0.000} pos:{3}", 1.0 / scene.timeDelta, 1.0 / dt, scene.timeDelta, scene.mousePos);
            gfx.DrawString(info, Font, Brushes.Black, 0, 0);
        }

        private void pbxDraw_Layout(object sender, LayoutEventArgs e) {
            ReallocBuffer();
        }
    }
}
