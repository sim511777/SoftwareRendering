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

            bw = Math.Max(pbxDraw.Width, 16);
            bh = Math.Max(pbxDraw.Height, 16);
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
            int clearColor = Color.Black.ToArgb();
            Util.Memset4(imgBuf, clearColor, bw * bh);
        }

        private void DrawGraphics(Graphics gfx) {
            string info = $"fps:{1.0 / scene.timeDelta:0} time:{scene.timeDelta:0.000}sec";
            var size = gfx.MeasureString(info, Font);
            gfx.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
            gfx.DrawString(info, Font, Brushes.Black, 0, 0);
        }

        private void pbxDraw_Layout(object sender, LayoutEventArgs e) {
            ReallocBuffer();
        }
    }
}
