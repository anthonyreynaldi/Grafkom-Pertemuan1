using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Pertemuan1
{
    internal class Window : GameWindow
    {
        /*float[] _vertices =
        {
            //x     y     z
            -0.5f, -0.5f, 0.0f,         //vertex 0
            0.5f,  -0.5f, 0.0f,         //vertex 1
            0.0f,   0.5f, 0.0f
        };*/

        float[] _vertices =
        {
            //x     y     z      r      g     b
            -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f,        //vertex 0 merah
            0.5f,  -0.5f, 0.0f, 0.0f, 1.0f, 0.0f,        //vertex 1 hijau
            0.0f,   0.5f, 0.0f, 0.0f, 0.0f, 1.0f        //vertex 2 biru
        };

        /*float[] _vertices =
        {
            //x     y     z
             0.5f,  0.5f, 0.0f,         //top R
             0.5f, -0.5f, 0.0f,         //bottom R
            -0.5f, -0.5f, 0.0f,        //bottom L
            -0.5f,  0.5f, 0.0f           //top L
        };
        uint[] _indices =
        {
            0,1,3, //segitiga pertama ambil dari vertices baris 0 1 3
            1,2,3  //segitia kedua ambil dari vertices baris 1 2 3
        };*/

        int _vertexBufferObject;
        //int _elementBufferObject;
        int _vertexArrayObject;
        Shader _shader;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //ganti background warna
            GL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);

            //inisialisasi generate buffer
            _vertexBufferObject = GL.GenBuffer();
            //nentuin targen dan yang handle diapa
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float),
                _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            //start index di shader 0, ada 3 vertex, tipedatanya float, perlu dinormalisasi false,  dalam satu vertex ada berapa float 3 * float (supaya dalam byte), mulai dari vertex ke 0 yang diolah
            /*GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //0 -> dari referensi param pertama yang atas
            GL.EnableVertexAttribArray(0);*/

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //0 -> dari referensi param pertama yang atas
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            //_elementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), 
            //    _indices, BufferUsageHint.StaticDraw);

            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAtrributeCount);
            Console.WriteLine($"Maximum number of " + $"vertex attributes supported: {maxAtrributeCount}");

            _shader = new Shader("../../../Shaders/shader.vert",
                                 "../../../Shaders/shader.frag");
            _shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, 
            //    DrawElementsType.UnsignedInt, 0);
            SwapBuffers();

        }

        // dijalani setiap ada reziew window
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        // dijalani setiap 60 fps 
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;      //inpu keyboard
            var mouse_input = MouseState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}
