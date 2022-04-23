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

        Camera _camera;

        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _objectPos = new Vector3(0,0,0);
        float _rotationSpeed = 1f;

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

            var ellipsoid1 = new Asset3d(new Vector3(0, 0.5f, 1));
            ellipsoid1.createElipticParaboloid(0, 0, -0.5f, 0.1f, 0.4f, 0.4f);
            _objectList.Add(ellipsoid1);

            /* var box = new Asset3d(new Vector3(0, 0.5f, 1));
             box.createBoxVertices(0, 0, 0);
             _objectList.Add(box);*/


            foreach (Asset3d i in _objectList)
            {
                i.load(Size.X, Size.Y);
            }

            _camera = new Camera(new Vector3(0, 0, 1), Size.X / Size.Y);

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //rotation by time
            _time += 7.0 * args.Time;

            var temp = Matrix4.Identity;

            //rotation by degree
            degr = MathHelper.DegreesToRadians(-0.1f);       //muter 20 derajat
            //temp = temp * Matrix4.CreateRotationZ(degr);
            temp = temp * Matrix4.CreateRotationX(degr);
            //temp = temp * Matrix4.CreateRotationY(degr);

            //custom rotate


            foreach (Asset3d i in _objectList)
            {
                //         pusat rotasi                  sumbu putar derajat
                //i.rotate2(new Vector3(0.0f, 0.5f, 0.0f), i._euler[0], 0.5f);
                i.render(_time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            }

            SwapBuffers();

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }



        // dijalani setiap ada reziew window
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
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

            float cameraSpeed = 1f;
            if (KeyboardState.IsKeyDown(Keys.W))    //maju
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.S))    //mundur
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.A))    //maju
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.D))    //maju
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))    //maju
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift))    //maju
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }

            var mouse = MouseState;
            var sesitivity = 0.2f;
            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;

                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sesitivity;
                _camera.Pitch += deltaY * sesitivity;
            }

            if (KeyboardState.IsKeyDown(Keys.N)){
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPos;
                _camera.Position = Vector3.Transform(
                    _camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());

                _camera.Position += _objectPos;
                _camera._front =  -Vector3.Normalize(_camera.Position - _objectPos);
            }

            if (KeyboardState.IsKeyDown(Keys.Comma))
            {
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPos;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(
                    _camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());

                _camera.Position += _objectPos;
                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
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

        public void rotateCamera(Vector3 axis, Vector3 camRotationCenter, Vector3 lookAt, float rotationSpeed)
        {
            _camera.Position -= camRotationCenter;
            if (axis == Vector3.UnitY)
            {
                _camera.Yaw += rotationSpeed;
            }
            _camera.Position = Vector3.Transform(_camera.Position, generateArbRotationMatrix(axis, rotationSpeed).ExtractRotation());
            _camera.Position += camRotationCenter;
            _camera._front = Vector3.Normalize(lookAt - _camera.Position);
        }

        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatix;
        }

        public Matrix4 generateArbRotationMatrix(Vector3 axis, float angle)
        {
            angle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4(
                (float)Math.Cos(angle) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(angle)), axis.X * axis.Y * (1 - (float)Math.Cos(angle)) - axis.Z * (float)Math.Sin(angle), axis.X * axis.Z * (1 - (float)Math.Cos(angle)) + axis.Y * (float)Math.Sin(angle), 0,
                axis.Y * axis.X * (1 - (float)Math.Cos(angle)) + axis.Z * (float)Math.Sin(angle), (float)Math.Cos(angle) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(angle)), axis.Y * axis.Z * (1 - (float)Math.Cos(angle)) - axis.X * (float)Math.Sin(angle), 0,
                axis.Z * axis.X * (1 - (float)Math.Cos(angle)) - axis.Y * (float)Math.Sin(angle), axis.Z * axis.Y * (1 - (float)Math.Cos(angle)) + axis.X * (float)Math.Sin(angle), (float)Math.Cos(angle) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(angle)), 0,
                0, 0, 0, 1
                );

            return arbRotationMatrix;
        }
    }
}
