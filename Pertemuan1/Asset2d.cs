using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
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

        public Asset2d(float[] verticies, uint[] indices, float[] color)
        {
            _vertices = verticies;
            _indices = indices;
            _color = color;
        }

        public void load()
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

            _shader = new Shader("../../../Shaders/shader.vert",
                                 "../../../Shaders/shader.frag");
            _shader.Use();          //ngasih tau GPU ini mau diapain
        }

        public void render()
        {
            _shader.Use();

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
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }
        }
    }
}
