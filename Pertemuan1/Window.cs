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
        Asset2d[] _object = new Asset2d[2];

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //ganti background warna
            GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);

            _object[0] = new Asset2d(
                new float[]
                {
                    -0.75f, 0.0f, 0.0f,
                    -0.5f, 0.5f, 0.0f,
                    -0.25f, 0.0f, 0.0f
                },
                new uint[]
                {

                }
                
            );

            _object[1] = new Asset2d(
                new float[]
                {
                    0.75f, 0.0f, 0.0f,
                    0.5f, 0.5f, 0.0f,
                    0.25f, 0.0f, 0.0f
                },
                new uint[]
                {

                }

            );

            _object[0].load();
            _object[1].load();

            GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAtrributeCount);
            Console.WriteLine($"Maximum number of " + $"vertex attributes supported: {maxAtrributeCount}");

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _object[0].render();
            _object[1].render();

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
