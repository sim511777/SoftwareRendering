using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareRendering {
    public class Vertex {
        public Vector4 pos;
        public Vector4 norm;
        public Vector2 tpos;
    }

    public class Mesh {
        public List<Vector3> pos = new List<Vector3>();
        public List<Vector3> norm = new List<Vector3>();
        public List<Vector2> tcoord = new List<Vector2>();
        public List<int> indices = new List<int>();
    }
    
    public class Model {
        public Mesh mesh;
        public ImageBuffer texture;
        public Model(Mesh _mesh, ImageBuffer _texture) {
            mesh = _mesh;
            texture = _texture;
        }
    }
}
