using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareRendering {
    public class RenderManager {
        public int bw = 0;
        public int bh = 0;
        public IntPtr buf = IntPtr.Zero;
        public BufferedGraphics backBuffer = null;
        public Font font = null;

        public void ReallocBuffer(Control ctrl) {
            FreeBuffer();

            bw = Math.Max(ctrl.ClientSize.Width, 16);
            bh = Math.Max(ctrl.ClientSize.Height, 16);
            buf = Marshal.AllocHGlobal(bw * bh * 4);
            backBuffer = BufferedGraphicsManager.Current.Allocate(ctrl.CreateGraphics(), new Rectangle(0, 0, bw, bh));
            backBuffer.Graphics.CompositingMode = CompositingMode.SourceCopy;
            font = new Font("굴림체", 9);
        }

        public void FreeBuffer() {
            if (buf != IntPtr.Zero)
                Marshal.FreeHGlobal(buf);
            if (backBuffer != null)
                backBuffer.Dispose();
            if (font != null)
                font.Dispose();
        }

        public void Draw(SceneManager scene) {
            // 픽셀버퍼 렌더링
            RenderPixelBuffer();
            
            using (var bmp = new Bitmap(bw, bh, bw * 4, PixelFormat.Format32bppPArgb, buf)) {
                using (var g = Graphics.FromImage(bmp)) {
                    // 픽셀버퍼에 그림 그리기
                    DrawGraphics(g, scene);
                }

                // 백버퍼에 픽셀버퍼 복사
                backBuffer.Graphics.DrawImageUnscaledAndClipped(bmp, new Rectangle(0, 0, bw, bh));
            }

            // 백버퍼를 프론트버퍼로 복사
            backBuffer.Render();
        }

        private void RenderPixelBuffer() {
            var color = Color.Gray;
            int argb = color.ToArgb();
            Util.Memset4(buf, argb, bw * bh);
        }

        private void DrawGraphics(Graphics g, SceneManager scene) {
            string info = $"{scene.fpsAvg,4:f0} fps, {scene.dtimeAvg * 1000,5:f2} ms";
            var size = g.MeasureString(info, font);
            g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
            g.DrawString(info, font, Brushes.Black, 0, 0);
        }
    }
}
