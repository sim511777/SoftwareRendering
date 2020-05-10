using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareRendering {
    public class SceneManager {
        public double timeDelta;
        public Point mousePos;

        private int cnt = 0;
        private double sum = 0;
        public double fpsAvg = 0;
        public double dtimeAvg = 0;

        private Bsp bsp = null;
        private int ambientLight = Color.White.ToArgb();
        private List<Model> modelGroup;

        double keyAngleSpeed = 5;
        double mouseAngleSPeed = 0.1;
        double moveSpeed = 500;

        Vector3 camPos = new Vector3();
        double camYaw = 0;
        double camPitch = 0;

        public SceneManager() {
            Bsp.LoadPalette();
            modelGroup = new List<Model>();
        }

        public void Update(double _timeDelta, Point _pos) {
            cnt++;
            sum += timeDelta;
            if (sum > 0.5) {
                dtimeAvg = sum / cnt;
                fpsAvg = 1 / dtimeAvg;
                cnt = 0;
                sum = 0;
            }

            timeDelta = _timeDelta;
            mousePos = _pos;
        }

        private void LoadCamera() {
            try {
                var infostart = this.bsp.entities.FirstOrDefault((entity) => entity.classname == "info_player_start");
                var words = infostart.items["origin"].Split(' ');
                camPos = new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2]));
                camYaw = double.Parse(infostart.items["angle"]) * Math.PI * 2 / 360;
                camPitch = 0;
            } catch {
                camPos = new Vector3(0, 0, 0);
                camYaw = 0;
                camPitch = 0;
            }
        }

        public void LoadBsp(byte[] buf) {
            bsp = Bsp.Read(buf);
            LoadCamera();
        }
    }
}
