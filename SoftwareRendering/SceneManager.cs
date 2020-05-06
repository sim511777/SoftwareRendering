using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    }
}
