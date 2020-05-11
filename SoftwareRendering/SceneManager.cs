using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareRendering {
    public class SceneManager {
        private int cnt = 0;
        private double sum = 0;
        public double fpsAvg = 0;
        public double dtimeAvg = 0;

        private Bsp bsp = null;
        private int ambientLight = Color.White.ToArgb();
        private List<Model> modelGroup;

        float keyAngleSpeed = 5;
        float mouseAngleSPeed = 0.1f;
        float moveSpeed = 500;

        Vector3 camPos = new Vector3();
        float camYaw = 0;
        float camPitch = 0;

        PerspectiveCamera cam = new PerspectiveCamera();

        public SceneManager() {
            Bsp.LoadPalette();
            modelGroup = new List<Model>();
        }

        public void Update(double dTime, InputManager input) {
            cnt++;
            sum += dTime;
            if (sum > 0.5) {
                dtimeAvg = sum / cnt;
                fpsAvg = 1 / dtimeAvg;
                cnt = 0;
                sum = 0;
            }

            if (input.viewFocus) {
                ProcessInput((float)dTime, input);
                UpdateCamera();
            }
        }

        private void ProcessInput(float dTime, InputManager input) {
            if (input[Keys.Right]) this.camYaw -= keyAngleSpeed * dTime;
            if (input[Keys.Left]) this.camYaw += keyAngleSpeed * dTime;
            if (input[Keys.Up]) this.camPitch += keyAngleSpeed * dTime;
            if (input[Keys.Down]) this.camPitch -= keyAngleSpeed * dTime;

            var diff = input.Pos - (Size)input.viewCenterPt;
            input.Pos = input.viewCenterPt;

            this.camPitch -= diff.Y * mouseAngleSPeed * dTime;
            if (this.camPitch > Math.PI / 2 - 0.01)
                this.camPitch = (float)(Math.PI / 2 - 0.01);
            if (this.camPitch < -Math.PI / 2 + 0.01)
                this.camPitch = (float)(-Math.PI / 2 + 0.01);
            this.camYaw -= diff.X * mouseAngleSPeed * dTime;

            Vector3 vMove = new Vector3(0, 0, 0);
            if (input[Keys.D]) vMove.X += 1;
            if (input[Keys.A]) vMove.X -= 1;
            if (input[Keys.W]) vMove.Y += 1;
            if (input[Keys.S]) vMove.Y -= 1;
            if (vMove.Length() != 0) {
                vMove = Vector3.Normalize(vMove);
                vMove = vMove * moveSpeed * dTime;
                var vLook = cam.LookDirection;
                var vUp = cam.UpDirection;
                var vRight = Vector3.Cross(vLook, vUp);
                vRight = Vector3.Normalize(vRight);
                var vMove2 = vLook * vMove.Y + vRight * vMove.X;
                this.camPos += vMove2;
            }
        }

        private void LoadCamera() {
            try {
                var infostart = this.bsp.entities.FirstOrDefault((entity) => entity.classname == "info_player_start");
                var words = infostart.items["origin"].Split(' ');
                camPos = new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2]));
                camYaw = (float)(double.Parse(infostart.items["angle"]) * Math.PI * 2 / 360);
                camPitch = 0;
            } catch {
                camPos = new Vector3(0, 0, 0);
                camYaw = 0;
                camPitch = 0;
            }
        }

        private void UpdateCamera() {
            cam.Position = camPos;
            cam.LookDirection = new Vector3((float)(Math.Cos(camPitch) * Math.Cos(camYaw)), (float)(Math.Cos(camPitch) * Math.Sin(camYaw)), (float)(Math.Sin(camPitch)));
            cam.UpDirection = new Vector3(0, 0, 1);
            cam.FieldOfView = 90;
        }

        public void LoadBsp(byte[] buf) {
            bsp = Bsp.Read(buf);
            LoadCamera();
        }
    }
}
