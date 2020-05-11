using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareRendering {
    public class Mesh {
        public List<Vector3> pos = new List<Vector3>();
        public List<Vector3> norm = new List<Vector3>();
        public List<Vector2> tcoord = new List<Vector2>();
        public List<int> indices = new List<int>();
    }

    public class PerspectiveCamera {
        public float NearPlaneDistance { get; set; }
        public float FarPlaneDistance { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LookDirection { get; set; }
        public Vector3 UpDirection { get; set; }
        public float FieldOfView { get; set; }
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
