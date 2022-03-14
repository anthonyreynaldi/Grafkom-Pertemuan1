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
    static class Constants
    {
        public const string path = "../../../Shaders/";
    }

    internal class Window : GameWindow
    {
        Asset2d[] _object = new Asset2d[8];

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //ganti background warna
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            //ketupat tengah
            _object[0] = new Asset2d(
                new float[]
                {
                    0.0f, 0.75f, 0.0f,
                    0.75f, 0.0f, 0.0f,
                    0.0f, -0.75f, 0.0f,
                    -0.75f, 0.0f, 0.0f
                },
                new uint[]
                {
                    0, 1, 2,
                    0, 3, 2
                },
                new float[]
                {
                    1.0f, 1.0f, 1.0f, 1.0f
                }
                
            );

            //segitiga kanan atas
            _object[1] = new Asset2d(
                new float[]
                {
                    0.75f, 0.75f, 0.0f,     //siku
                    0.75f, 0.25f, 0.0f,
                    0.25f, 0.75f, 0.0f
                },
                new uint[]
                {

                },
                new float[]
                {
                    0.0f, 0.0f, 1.0f, 1.0f
                }

            );

            //segitiga kanan bawah --> semua y jadi -
            _object[2] = new Asset2d(
                new float[]
                {
                    0.75f, -0.75f, 0.0f,     //siku
                    0.75f, -0.25f, 0.0f,
                    0.25f, -0.75f, 0.0f
                },
                new uint[]
                {

                },
                new float[]
                {
                    0.0f, 1.0f, 0.0f, 1.0f
                }

            );

            //segitiga kiri atas semua x jadi -
            _object[3] = new Asset2d(
                new float[]
                {
                    -0.75f, 0.75f, 0.0f,     //siku
                    -0.75f, 0.25f, 0.0f,
                    -0.25f, 0.75f, 0.0f
                },
                new uint[]
                {

                },
                new float[]
                {
                    1.0f, 1.0f, 0.0f, 1.0f
                }

            );

            //segitiga kiri bawah semua x dan y jadi -
            _object[4] = new Asset2d(
                new float[]
                {
                    -0.75f, -0.75f, 0.0f,     //siku
                    -0.75f, -0.25f, 0.0f,
                    -0.25f, -0.75f, 0.0f
                },
                new uint[]
                {

                },
                new float[]
                {
                    1.0f, 0.0f, 0.0f, 1.0f
                }

            );

            _object[5] = new Asset2d(
                new float[] {},
                new uint[] {}
                );
            _object[5].createEllips(0.0f, -0.5f, 0.25f, 0.5f);


            _object[6] = new Asset2d(
                new float[1080],
                new uint[] { }
                );

            _object[7] = new Asset2d(
               new float[] { },
               new uint[] { }
               );

            //load all
            for (int i = 0; i < _object.Length; i++)
            {
                _object[i].load(Constants.path + "shader.vert", Constants.path + "shader.frag");
            }

            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAtrributeCount);
            Console.WriteLine($"Maximum number of " + $"vertex attributes supported: {maxAtrributeCount}");

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //all render
            for (int i = 0; i < _object.Length-2; i++)
            {
                _object[i].render();
            }

            _object[5].render("circle");
            
            if (_object[6].getVerticesLength())
            {
                List<float> _verticesTemp = _object[6].createCureveBezier();
                _object[7].setvertices(_verticesTemp.ToArray());
                _object[7].load(Constants.path + "shader.vert", Constants.path + "shader.frag");
                _object[7].render("lineBezier");
            }
            _object[6].render("line");

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if(e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X/2) / (Size.X/2);
                float _y = -(MousePosition.Y - Size.Y/2) / (Size.Y/2);

                Console.WriteLine(_x + " " + _y);

                _object[6].updateMousePosition(_x, _y);
            }
        }
    }
}
