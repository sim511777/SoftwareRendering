using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareRendering {
    public struct Vertex {
        public Vector4 pos;
        public Vector4 norm;
        public Vector2 tpos;
        public Vertex(Vector4 _pos, Vector4 _norm, Vector2 _tpos) {
            pos = _pos;
            norm = _norm;
            tpos = _tpos;
        }
    }

    public class ImageBuffer {
        public IntPtr buf;
        public int bw;
        public int bh;
        public int bytepp;
    }

    public class Model {
        public Vector<Vertex> vertices;
        public Vector<int> indices;
        public ImageBuffer texture;
    }
}
