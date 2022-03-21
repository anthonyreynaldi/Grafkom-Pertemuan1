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
using OpenTK.Mathematics;

namespace Pertemuan1
{
    static class Constants
    {
        public const string path = "../../../Shaders/";
    }

    internal class Window : GameWindow
    {
        Asset2d[] _object = new Asset2d[8];
        List<Asset3d> _objectList = new List<Asset3d>();

        double _time = 0;
        float degr = 0;

        

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //ganti background warna
            //GL.Enable(EnableCap.DepthTest);             //kasih tau kau ada object dibelakang gak keliatan
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);


            //var ellipsoid1 = new Asset3d(new Vector3(0, 0.5f, 1));
            //ellipsoid1.createEllipsoid(0, 0, 0, 0.4f, 0.4f, 0.4f);
            //_objectList.Add(ellipsoid1);

            var box = new Asset3d(new Vector3(0, 0.5f, 1));
            box.createBoxVertices(0, 0, 0);
            _objectList.Add(box);


            foreach(Asset3d i in _objectList)
            {
                i.load(Size.X, Size.Y);
            }

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //rotation by time
            _time += 7.0 * args.Time;

            var temp = Matrix4.Identity;

            //rotation by degree
            //degr += MathHelper.DegreesToRadians(0.1f);       //muter 20 derajat
            //temp = temp * Matrix4.CreateRotationY(degr);

            //custom rotate


            foreach (Asset3d i in _objectList)
            {
                //         pusat rotasi                  sumbu putar derajat
                i.rotate(new Vector3(0.0f, 0.5f, 0.0f), i._euler[0], 0.5f);
                i.render(_time, temp);
            }

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
        /*
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
        */
    }
}
