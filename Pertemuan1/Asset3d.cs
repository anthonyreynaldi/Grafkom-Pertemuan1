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
    internal class Asset3d
    {
        public List<Vector3> _vertices = new List<Vector3>();
        private List<uint> _indices = new List<uint>();

        private Vector3 _color;

        int _vertexBufferObject;
        int _elementBufferObject;
        int _vertexArrayObject;
        Shader _shader;

        public Asset3d(Vector3 color)
        {
            this._color = color;
        }

        
        public void load(string shadervert = "../../../Shaders/shader.vert", string shaderfrag = "../../../Shaders/shader.frag")
        {
            //inisialisasi generate buffer
            _vertexBufferObject = GL.GenBuffer();
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
           
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            if (_indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Count * sizeof(uint),
                    _indices.ToArray(), BufferUsageHint.StaticDraw);
            }


            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();          //ngasih tau GPU ini mau diapain
        }



        public void render(string pilihan = "")
        {
            _shader.Use();

            //uniform untuk color
            _shader.SetVector3("ourColor", _color);
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.2f, 0.0f, 1.0f);
            //GL.Uniform4(vertexColorLocation, _color);


            GL.BindVertexArray(_vertexArrayObject);

            /*if (_indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count,
                    DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                if (pilihan == "circle")
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, (_vertices.Count + 1) / 3);
                }
                else if (pilihan == "line")
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, index);
                }
                else if (pilihan == "lineBezier")
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, (_vertices.Count + 1) / 3);
                }
                else
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
            }*/

            if (_indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.LineStrip, 0, _vertices.Count);
            }
        }

        public void createEllipsoid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            var tempVertex = new Vector3();
            for (float u = - MathF.PI; u < MathF.PI; u += MathF.PI / 100.0f)
            {
                for (float v = - MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = radiusX * MathF.Cos(v) * MathF.Cos(u) + x;
                    tempVertex.Y = radiusY * MathF.Cos(v) * MathF.Sin(u) + y;
                    tempVertex.Z = radiusZ * MathF.Sin(v) + z;
                    _vertices.Add(tempVertex);
                }
            }
        }

    
    }
}
