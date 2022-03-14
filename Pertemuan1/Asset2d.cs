using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pertemuan1
{
    internal class Asset2d
    {
        float[] _vertices =
        {
            //x     y     z
            //-0.5f, -0.5f, 0.0f,         //vertex 0
            //0.5f,  -0.5f, 0.0f,         //vertex 1
            //0.0f,   0.5f, 0.0f
        };

        /*float[] _vertices =
        {
            //x     y     z      r      g     b
            -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f,        //vertex 0 merah
            0.5f,  -0.5f, 0.0f, 0.0f, 1.0f, 0.0f,        //vertex 1 hijau
            0.0f,   0.5f, 0.0f, 0.0f, 0.0f, 1.0f        //vertex 2 biru
        };*/

        /*float[] _vertices =
        {
            //x     y     z
             0.5f,  0.5f, 0.0f,         //top R
             0.5f, -0.5f, 0.0f,         //bottom R
            -0.5f, -0.5f, 0.0f,        //bottom L
            -0.5f,  0.5f, 0.0f           //top L
        };*/

        uint[] _indices =
        {
            //0,1,3, //segitiga pertama ambil dari vertices baris 0 1 3
            //1,2,3  //segitia kedua ambil dari vertices baris 1 2 3
        };

        float[] _color =
        {
            1.0f, 0.0f, 0.0f, 0.0f
        };

        int _vertexBufferObject;
        int _elementBufferObject;
        int _vertexArrayObject;
        Shader _shader;
        int index;
        int[] _pascal;

        public Asset2d(float[] verticies, uint[] indices)
        {
            _vertices = verticies;
            _indices = indices;
            index = 0;
        }

        public Asset2d(float[] verticies, uint[] indices, float[] color)
        {
            _vertices = verticies;
            _indices = indices;
            _color = color;
        }

        public void load(string shadervert = "../../../Shaders/shader.vert", string shaderfrag = "../../../Shaders/shader.frag")
        {
            //inisialisasi generate buffer
            _vertexBufferObject = GL.GenBuffer();               //menyimpan vertex bisa warna, texture dll untuk dikirim ke GPU (cuman dikirim)
            //nentuin targen dan yang handle diapa
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float),
                _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();           //kasih tau GPU dibaginya datanya kek gmn
            GL.BindVertexArray(_vertexArrayObject);
            //start index di shader 0, ada 3 data (x,y,z), tipedatanya float, perlu dinormalisasi false,  dalam satu vertex ada berapa float 3 * float (supaya dalam byte), mulai dari vertex ke 0 yang diolah
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //0 -> dari referensi param pertama yang atas
            GL.EnableVertexAttribArray(0);

            /*GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //0 -> dari referensi param pertama yang atas
            GL.EnableVertexAttribArray(0);*/

            /* GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
             GL.EnableVertexAttribArray(1);*/


            if (_indices.Length != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), 
                    _indices, BufferUsageHint.StaticDraw);
            }

            
            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();          //ngasih tau GPU ini mau diapain
        }

        public void render(string pilihan = "")
        {
            _shader.Use();

            //uniform untuk color
            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.2f, 0.0f, 1.0f);
            GL.Uniform4(vertexColorLocation, _color[0], _color[1], _color[2], _color[3]);


            GL.BindVertexArray(_vertexArrayObject);

            if (_indices.Length != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, 
                    DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (pilihan == "circle")
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, (_vertices.Length + 1) / 3);
                }
                else if (pilihan == "line")
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, index);
                }
                else if (pilihan == "lineBezier")
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, (_vertices.Length + 1) / 3);
                }
                else
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
            }

            
            
        }

        public void createCircle(float center_x, float center_y, float radius)
        {
            _vertices = new float[1080];
            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;
                //x
                _vertices[i * 3] = radius * (float)Math.Cos(degInRad) + center_x;
                //x
                _vertices[i * 3 + 1] = radius * (float)Math.Sin(degInRad) + center_y;
                //x
                _vertices[i * 3 + 2] = 0;
            }
        }

        public void createEllips(float center_x, float center_y, float radiusX, float radiusY)
        {
            _vertices = new float[1080];
            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;
                //x
                _vertices[i * 3] = radiusX * (float)Math.Cos(degInRad) + center_x;
                //x
                _vertices[i * 3 + 1] = radiusY * (float)Math.Sin(degInRad) + center_y;
                //x
                _vertices[i * 3 + 2] = 0;
            }
        }

        public void updateMousePosition(float _x, float _y)
        {
            //x
            _vertices[index * 3] = _x;
            //x
            _vertices[index * 3 + 1] = _y;
            //x
            _vertices[index * 3 + 2] = 0;

            index++;

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float),
                _vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public List<int> getRow(int rowIndex)
        {
            List<int> currow = new List<int>();
            //------
            currow.Add(1);
            if (rowIndex == 0)
            {
                return currow;
            }
            //-----
            List<int> prev = getRow(rowIndex - 1);
            for (int i = 1; i < prev.Count; i++)
            {
                int curr = prev[i - 1] + prev[i];
                currow.Add(curr);
            }
            currow.Add(1);
            return currow;
        }

        public List<float> createCureveBezier()
        {
            List<float> _vertices_bezier = new List<float>();

            List<int> pascal = getRow(index - 1);
            _pascal = pascal.ToArray();

            for (float t = 0; t <= 1.0f; t += 0.01f)
            {
                Vector2 p = getP(index, t);
                _vertices_bezier.Add(p.X);
                _vertices_bezier.Add(p.Y);
                _vertices_bezier.Add(0);
            }

            return _vertices_bezier;
        }

        public Vector2 getP(int n, float t)
        {
            Vector2 p = new Vector2(0,0);
            float k;
            for (int i = 0; i < n; i++)
            {
                k = (float) Math.Pow((1-t), n-1-i)
                    * (float) Math.Pow(t, i) * _pascal[i] ;
                p.X += k * _vertices[i * 3];
                p.Y += k * _vertices[i * 3 + 1];
            }

            return p;
        }

        public bool getVerticesLength()
        {
            if(_vertices[0] == 0)
            {
                return false;
            }

            if((_vertices.Length + 1) / 3 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setvertices(float[] _temp)
        {
            _vertices = _temp;
        }
    }
}   
