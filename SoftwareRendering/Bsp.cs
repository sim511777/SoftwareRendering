using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SoftwareRendering {
    class Bsp {
        public static int[] colorPalette;
        public static int[] grayPalette;

        public static void LoadPalette() {
            var bytes = Properties.Resources.palette;
            int palCnt = bytes.Length / 3;
            Bsp.colorPalette = Enumerable.Range(0, palCnt).Select(palIdx => (bytes[palIdx * 3] | bytes[palIdx * 3 + 1] << 8 | bytes[palIdx * 3 + 2] | 0xff << 24)).ToArray();
            Bsp.grayPalette = Enumerable.Range(0, 256).Select(palIdx => (palIdx | palIdx << 8 | palIdx | 0xff << 24)).ToArray();
        }

        public Entity[] entities;    // List of Entities.
        public Plane[] planes;      // Map Planes.
        public Mipheader mipheader;   // Wall Textures.
        public Miptex[] miptexs;     // Wall TexturesData
        public Vector3[] vertices;    // Map Vertices.
        public byte[] vislist;    // Leaves Visibility lists.
        public Node[] nodes;       // BSP Nodes.
        public TexInfo[] texinfo;     // Texture Info for faces.
        public Face[] faces;       // Faces of each surface.
        public byte[] lightmaps;   // Wall Light Maps.
        public Clipnode[] clipnodes;   // clip nodes, for Models.
        public Leaf[] leafs;      // BSP Leaves.
        public ushort[] lface;       // List of Faces.
        public Edge[] edges;       // Edges of faces.
        public int[] ledges;      // List of Edges.
        public BspModel[] models;      // List of Models.

        public ImageBuffer[] mipMaterials;   // texture
        public Model[] mipModels; // geo models
        public Model[] lightModels; // geo models

        public static ImageBuffer GetMipMaterial(Miptex miptex) {
            if (miptex == null)
                return null;

            return BuildMap(miptex.texture1, miptex.width, miptex.height, colorPalette);
        }

        public Model GetMipModel(Face face) {
            var ledgelist = ledges.Skip(face.ledge_id).Take(face.ledge_num);
            var vidxs = ledgelist
               .Select((ledge) => (ledge >= 0) ? new int[] { edges[ledge].vertex0, edges[ledge].vertex1 } : new int[] { edges[-ledge].vertex1, edges[-ledge].vertex0 })
               .SelectMany((pair) => pair)
               .Where((vidx, i) => (i % 2 == 0));

            TexInfo tinfo = texinfo[face.texinfo_id];
            var material = mipMaterials[tinfo.texture_id];
            var tw = material.bw;
            var th = material.bh;

            Mesh mesh = new Mesh();
            foreach (var vidx in vidxs) {
                var v = vertices[vidx];
                mesh.pos.Add(v);
                mesh.norm.Add(planes[face.plane_id].normal);

                Vector2 st = new Vector2();
                st.X = (Vector3.Dot(v, tinfo.vectorS) + tinfo.distS) / tw;
                st.Y = (Vector3.Dot(v, tinfo.vectorT) + tinfo.distT) / th;
                mesh.tcoord.Add(st);
            }

            for (int i = 2; i < vidxs.Count(); i++) {
                mesh.indices.Add(i);
                mesh.indices.Add(i - 1);
                mesh.indices.Add(0);
            }

            return new Model(mesh, material);
        }

        private void CalcSurfaceExtents(Face s) {
            float[] numArray1 = new float[2];
            float[] numArray2 = new float[2];
            int[] numArray3 = new int[2];
            int[] numArray4 = new int[2];
            numArray1[0] = numArray1[1] = 999999f;
            numArray2[0] = numArray2[1] = -99999f;
            TexInfo texinfoT = texinfo[(int)s.texinfo_id];
            for (int index1 = 0; index1 < (int)s.ledge_num; ++index1) {
                int surfedge = ledges[s.ledge_id + index1];
                var vertexT = surfedge < 0 ? vertices[(int)edges[-surfedge].vertex1] : vertices[(int)edges[surfedge].vertex0];
                double numS = Vector3.Dot(vertexT, texinfoT.vectorS) + texinfoT.distS;
                if (numS < numArray1[0])
                    numArray1[0] = (float)numS;
                if (numS > numArray2[0])
                    numArray2[0] = (float)numS;
                double numT = Vector3.Dot(vertexT, texinfoT.vectorT) + texinfoT.distT;
                if (numT < numArray1[1])
                    numArray1[1] = (float)numT;
                if (numT > numArray2[1])
                    numArray2[1] = (float)numT;
            }
            s.texturemins = new int[2];
            s.extents = new int[2];
            for (int index = 0; index < 2; ++index) {
                numArray3[index] = (int)Math.Floor((double)numArray1[index] / 16.0);
                numArray4[index] = (int)Math.Ceiling((double)numArray2[index] / 16.0);
                s.texturemins[index] = numArray3[index] * 16;
                s.extents[index] = (numArray4[index] - numArray3[index]) * 16;
            }
        }

        private static ImageBuffer BuildMap(byte[] buffer, int width, int height, int[] palette) {
            if (buffer == null)
                return null;

            int[] bufArr = buffer.Select(colIdx => palette[colIdx]).ToArray();
            IntPtr bufPtr = Marshal.AllocHGlobal(bufArr.Length * 4);
            Marshal.Copy(bufArr, 0, bufPtr, bufArr.Length);
            ImageBuffer imgBuf = new ImageBuffer(bufPtr, width, height, 4, true);
            return imgBuf;
        }

        private ImageBuffer R_BuildLightMap(Face surf) {
            int lightofs1 = surf.lightofs;
            if (lightofs1 == -1) {
                byte[] buffer2 = new byte[4];
                for (int index1 = 0; index1 < 4; ++index1)
                    buffer2[index1] = (byte)0;
                return BuildMap(buffer2, 2, 2, grayPalette);
            }

            uint[] blocklights = new uint[4096];
            int width = (surf.extents[0] >> 4) + 1;
            int height = (surf.extents[1] >> 4) + 1;
            int length = width * height;
            int lightofs2 = surf.lightofs;
            for (int index = 0; index < 364; ++index)
                blocklights[index] = 0U;
            for (int index1 = 0; index1 < 4 && surf.styles[index1] != byte.MaxValue; ++index1) {
                uint num = 264;
                for (int index2 = 0; index2 < length; ++index2)
                    blocklights[index2] += lightmaps[lightofs2 + index2] * num;
                lightofs2 += length;
            }
            int index3 = 0;
            byte[] buffer1 = new byte[length];
            int index4 = 0;
            while (index4 < length) {
                uint num = blocklights[index3] >> 7;
                if (num > (uint)byte.MaxValue)
                    num = (uint)byte.MaxValue;
                buffer1[index4] = (byte)num;
                ++index4;
                ++index3;
            }
            return BuildMap(buffer1, width, height, grayPalette);
        }

        public Model GetLightModel(Face face) {
            CalcSurfaceExtents(face);
            var ledgelist = ledges.Skip(face.ledge_id).Take(face.ledge_num);
            var vidxs = ledgelist
               .Select((ledge) => (ledge >= 0) ? new int[] { edges[ledge].vertex0, edges[ledge].vertex1 } : new int[] { edges[-ledge].vertex1, edges[-ledge].vertex0 })
               .SelectMany((pair) => pair)
               .Where((vidx, i) => (i % 2 == 0));

            TexInfo texinfoT = texinfo[face.texinfo_id];

            Mesh mesh = new Mesh();
            foreach (var vidx in vidxs) {
                var v = vertices[vidx];
                mesh.pos.Add(v);
                mesh.norm.Add(planes[face.plane_id].normal);

                Vector2 st = new Vector2();
                st.X = (Vector3.Dot(texinfoT.vectorS, v) + texinfoT.distS + 8f - face.texturemins[0]) / face.extents[0];
                st.Y = (Vector3.Dot(texinfoT.vectorT, v) + texinfoT.distT + 8f - face.texturemins[1]) / face.extents[1];
                mesh.tcoord.Add(st);
            }

            for (int i = 2; i < vidxs.Count(); i++) {
                mesh.indices.Add(i);
                mesh.indices.Add(i - 1);
                mesh.indices.Add(0);
            }

            ImageBuffer material = R_BuildLightMap(face);
            return new Model(mesh, material);
        }

        public static Bsp Read(byte[] buf) {
            using (var ms = new MemoryStream(buf))
            using (var br = new BinaryReader(ms)) {
                Header header = Header.Read(br);

                Bsp bsp = new Bsp();

                bsp.entities = ReadItems(br, header.entities, (b) => Entity.Read(b));
                bsp.planes = ReadItems(br, header.planes, (b) => Plane.Read(b));

                br.BaseStream.Seek(header.miptex.offset, SeekOrigin.Begin);
                bsp.mipheader = Mipheader.Read(br);
                bsp.miptexs = new Miptex[bsp.mipheader.numtex];
                for (int i = 0; i < bsp.miptexs.Length; i++) {
                    if (bsp.mipheader.offset[i] == -1)
                        continue;
                    br.BaseStream.Seek(header.miptex.offset + bsp.mipheader.offset[i], SeekOrigin.Begin);
                    bsp.miptexs[i] = Miptex.Read(br);
                }

                bsp.vertices = ReadItems(br, header.vertices, (b) => new Vector3(b.ReadSingle(), b.ReadSingle(), b.ReadSingle()));
                bsp.vislist = ReadItems(br, header.visilist, (b) => b.ReadByte());
                bsp.nodes = ReadItems(br, header.nodes, (b) => Node.Read(b));
                bsp.texinfo = ReadItems(br, header.texinfo, (b) => TexInfo.Read(b));
                bsp.faces = ReadItems(br, header.faces, (b) => Face.Read(b));
                bsp.lightmaps = ReadItems(br, header.lightmaps, (b) => b.ReadByte());
                bsp.clipnodes = ReadItems(br, header.clipnodes, (b) => Clipnode.Read(b));
                bsp.leafs = ReadItems(br, header.leafs, (b) => Leaf.Read(b));
                bsp.lface = ReadItems(br, header.lface, (b) => b.ReadUInt16());
                bsp.edges = ReadItems(br, header.edges, (b) => Edge.Read(b));
                bsp.ledges = ReadItems(br, header.ledges, (b) => b.ReadInt32());
                bsp.models = ReadItems(br, header.models, (b) => BspModel.Read(b));

                bsp.mipMaterials = bsp.miptexs.Select((mip) => GetMipMaterial(mip)).ToArray();
                bsp.mipModels = bsp.faces.Select((face) => bsp.GetMipModel(face)).ToArray();
                bsp.lightModels = bsp.faces.Select((face) => bsp.GetLightModel(face)).ToArray();

                return bsp;
            }
        }

        public static T[] ReadItems<T>(BinaryReader br, Entry entry, Func<BinaryReader, T> itemReader) {
            var stream = br.BaseStream;
            var items = new List<T>();
            for (stream.Seek(entry.offset, SeekOrigin.Begin); stream.Position < entry.offset + entry.size;) {
                T item = itemReader(br);
                if (item != null)
                    items.Add(item);
            }
            return items.ToArray();
        }
    }

    class Entry {
        public int offset;
        public int size;
        public static Entry Read(BinaryReader br) {
            Entry entry = new Entry();
            entry.offset = br.ReadInt32();
            entry.size = br.ReadInt32();
            return entry;
        }
    }

    class Header {
        public int version;
        public Entry entities;
        public Entry planes;
        public Entry miptex;
        public Entry vertices;
        public Entry visilist;
        public Entry nodes;
        public Entry texinfo;
        public Entry faces;
        public Entry lightmaps;
        public Entry clipnodes;
        public Entry leafs;
        public Entry lface;
        public Entry edges;
        public Entry ledges;
        public Entry models;
        public static Header Read(BinaryReader br) {
            Header header = new Header();
            header.version = br.ReadInt32();
            header.entities = Entry.Read(br);
            header.planes = Entry.Read(br);
            header.miptex = Entry.Read(br);
            header.vertices = Entry.Read(br);
            header.visilist = Entry.Read(br);
            header.nodes = Entry.Read(br);
            header.texinfo = Entry.Read(br);
            header.faces = Entry.Read(br);
            header.lightmaps = Entry.Read(br);
            header.clipnodes = Entry.Read(br);
            header.leafs = Entry.Read(br);
            header.lface = Entry.Read(br);
            header.edges = Entry.Read(br);
            header.ledges = Entry.Read(br);
            header.models = Entry.Read(br);
            return header;
        }
    }

    class Entity {
        public string classname;
        public Dictionary<string, string> items = new Dictionary<string, string>();
        public static Entity Read(BinaryReader br) {
            StringBuilder buf = new StringBuilder();
            while (true) {
                try {
                    char ch = Convert.ToChar(br.ReadByte());
                    if (ch == '\0')
                        return null;
                    if (ch == '{')
                        continue;
                    if (ch == '}')
                        break;
                    buf.Append(ch);
                } catch {
                    break;
                }
            }
            string text = buf.ToString();

            Entity entity = new Entity();
            var lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines) {
                int idx = line.IndexOf(' ');
                string itemName = line.Substring(0, idx).Trim('"');
                string itemValue = line.Substring(idx + 1).Trim('"');
                if (itemName == "classname") {
                    entity.classname = itemValue;
                } else {
                    entity.items[itemName] = itemValue;
                }
            }
            return entity;
        }
    }

    class Boundbox {
        public Vector3 min;
        public Vector3 max;
        public static Boundbox Read(BinaryReader br) {
            Boundbox box = new Boundbox();
            box.min = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            box.max = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            return box;
        }
    }

    class BBoxshort {
        public short minX;
        public short minY;
        public short minZ;
        public short maxX;
        public short maxY;
        public short maxZ;
        public static BBoxshort Read(BinaryReader br) {
            BBoxshort box = new BBoxshort();
            box.minX = br.ReadInt16();
            box.minY = br.ReadInt16();
            box.minZ = br.ReadInt16();
            box.maxX = br.ReadInt16();
            box.maxY = br.ReadInt16();
            box.maxZ = br.ReadInt16();
            return box;
        }
    }

    class Node {
        public int plane_id;
        public short front;
        public short back;
        public BBoxshort box;
        public ushort face_id;
        public ushort face_num;
        public static Node Read(BinaryReader br) {
            Node node = new Node();
            node.plane_id = br.ReadInt32();
            node.front = br.ReadInt16();
            node.back = br.ReadInt16();
            node.box = BBoxshort.Read(br);
            node.face_id = br.ReadUInt16();
            node.face_num = br.ReadUInt16();
            return node;
        }
    }

    class TexInfo {
        public Vector3 vectorS;
        public float distS;
        public Vector3 vectorT;
        public float distT;
        public uint texture_id;
        public uint animated;
        public static TexInfo Read(BinaryReader br) {
            TexInfo surface = new TexInfo();
            surface.vectorS = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            surface.distS = br.ReadSingle();
            surface.vectorT = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            surface.distT = br.ReadSingle();
            surface.texture_id = br.ReadUInt32();
            surface.animated = br.ReadUInt32();
            return surface;
        }
    }

    class Clipnode {
        public uint planenum;
        public short front;
        public short back;
        public static Clipnode Read(BinaryReader br) {
            Clipnode node = new Clipnode();
            node.planenum = br.ReadUInt32();
            node.front = br.ReadInt16();
            node.back = br.ReadInt16();
            return node;
        }
    }

    class Leaf {
        public int type;
        public int visOffset;
        public BBoxshort bound;
        public ushort lface_id;
        public ushort lface_num;
        public byte sndwater;
        public byte sndsky;
        public byte sndslime;
        public byte sndlava;
        public static Leaf Read(BinaryReader br) {
            Leaf leaf = new Leaf();
            leaf.type = br.ReadInt32();
            leaf.visOffset = br.ReadInt32();
            leaf.bound = BBoxshort.Read(br);
            leaf.lface_id = br.ReadUInt16();
            leaf.lface_num = br.ReadUInt16();
            leaf.sndwater = br.ReadByte();
            leaf.sndsky = br.ReadByte();
            leaf.sndslime = br.ReadByte();
            leaf.sndlava = br.ReadByte();
            return leaf;
        }
    }

    class BspModel {
        public Boundbox bound;
        public Vector3 origin;
        public int node_id0;
        public int node_id1;
        public int node_id2;
        public int node_id3;
        public int numleafs;
        public int face_id;
        public int face_num;
        public static BspModel Read(BinaryReader br) {
            BspModel model = new BspModel();
            model.bound = Boundbox.Read(br);
            model.origin = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            model.node_id0 = br.ReadInt32();
            model.node_id1 = br.ReadInt32();
            model.node_id2 = br.ReadInt32();
            model.node_id3 = br.ReadInt32();
            model.numleafs = br.ReadInt32();
            model.face_id = br.ReadInt32();
            model.face_num = br.ReadInt32();
            return model;
        }
    }

    enum PlaneType {
        AxialPlaneInX,
        AxialPlaneInY,
        AxialPlaneInZ,
        NonAxialPlaneTowardX,
        NonAxialPlaneTowardY,
        NonAxialPlaneTowardZ,
    }

    class Plane {
        public Vector3 normal;
        public double dist;
        public PlaneType type;
        public static Plane Read(BinaryReader br) {
            Plane plane = new Plane();
            plane.normal = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            plane.dist = br.ReadSingle();
            plane.type = (PlaneType)br.ReadInt32();
            return plane;
        }
    }

    class Mipheader {
        public int numtex;
        public int[] offset;
        public static Mipheader Read(BinaryReader br) {
            Mipheader mipheader = new Mipheader();
            mipheader.numtex = br.ReadInt32();
            mipheader.offset = new Int32[mipheader.numtex];
            for (int i = 0; i < mipheader.numtex; i++) {
                mipheader.offset[i] = br.ReadInt32();
            }
            return mipheader;
        }
    }

    class Miptex {
        public string name;  // 16
        public int width;
        public int height;
        public int offset1;
        public int offset2;
        public int offset4;
        public int offset8;
        public byte[] texture1; // full size
        public byte[] texture2; // 1/2  size
        public byte[] texture4; // 1/4  size
        public byte[] texture8; // 1/16 size
        public static Miptex Read(BinaryReader br) {
            Miptex miptex = new Miptex();
            var bytes = br.ReadBytes(16);
            miptex.name = Encoding.ASCII.GetString(bytes, 0, bytes.ToList().IndexOf(0));
            miptex.width = (int)br.ReadUInt32();
            miptex.height = (int)br.ReadUInt32();
            miptex.offset1 = (int)br.ReadUInt32();
            miptex.offset2 = (int)br.ReadUInt32();
            miptex.offset4 = (int)br.ReadUInt32();
            miptex.offset8 = (int)br.ReadUInt32();
            miptex.texture1 = br.ReadBytes((miptex.width * miptex.height));
            miptex.texture2 = br.ReadBytes((miptex.width * miptex.height) / 4);
            miptex.texture4 = br.ReadBytes((miptex.width * miptex.height) / 16);
            miptex.texture8 = br.ReadBytes((miptex.width * miptex.height) / 64);
            return miptex;
        }
    }

    class Face {
        public ushort plane_id;
        public ushort side;
        public int ledge_id;
        public ushort ledge_num;
        public ushort texinfo_id;
        public byte[] styles;
        public int lightofs;
        public int[] texturemins;
        public int[] extents;

        public static Face Read(BinaryReader br) {
            Face face = new Face();
            face.plane_id = br.ReadUInt16();
            face.side = br.ReadUInt16();
            face.ledge_id = br.ReadInt32();
            face.ledge_num = br.ReadUInt16();
            face.texinfo_id = br.ReadUInt16();
            face.styles = br.ReadBytes(4);
            face.lightofs = br.ReadInt32();
            return face;
        }
    }

    class Edge {
        public ushort vertex0;
        public ushort vertex1;
        public static Edge Read(BinaryReader br) {
            Edge edge = new Edge();
            edge.vertex0 = br.ReadUInt16();
            edge.vertex1 = br.ReadUInt16();
            return edge;
        }
    }
}
