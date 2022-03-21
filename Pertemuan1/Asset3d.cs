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

        Matrix4 _view;              //pengganti camera
        Matrix4 _projection;

        Vector3 _centerPosition = new Vector3(0, 0, 0);
        public List<Vector3> _euler = new List<Vector3>();

        public Asset3d(Vector3 color)
        {
            this._color = color;

            //sumbu x
            _euler.Add(new Vector3(1, 0, 0));
            //sumbu y
            _euler.Add(new Vector3(0, 1, 0));
            //sumbyx
            _euler.Add(new Vector3(0, 0, 1));
        }

        
        public void load(
            float Size_x, float Size_y,
            string shadervert = "../../../Shaders/shader.vert", 
            string shaderfrag = "../../../Shaders/shader.frag")
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

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f), Size_x / (float) Size_y, 
                0.1f, 100.0f);
            //kalo jaraknya < 0.1f gak bakal keliatan
            //kalo jaraknya > 100.0f gak bapak keliatan
        }



        public void render(double time, Matrix4 temp, string pilihan = "")
        {
            _shader.Use();

            //uniform untuk color
            _shader.SetVector3("ourColor", _color);
            //int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, 0.2f, 0.0f, 1.0f);
            //GL.Uniform4(vertexColorLocation, _color);

            var model = Matrix4.Identity;       //normal
            model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));
            model = temp;
            //model = model * Matrix4.CreateTranslation(0.0f, 1.0f, 0.0f);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

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
                                                                                                        //bujur              lintang
        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            var objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    _vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }
                }
            }
        }


        // minus artinya menjauh dari layar

        public void createBoxVertices(float x, float y, float z)
        {
            //biar lebih fleksibel jangan inisialiasi posisi dan 
            //panjang kotak didalam tapi ditaruh ke parameter
            float _positionX = x;
            float _positionY = y;
            float _positionZ = z;

            float _boxLength = 0.5f;

            //Buat temporary vector
            Vector3 temp_vector;
            //1. Inisialisasi vertex
            // Titik 1
            temp_vector.X = _positionX - _boxLength / 2.0f; // x 
            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);

            // Titik 2
            temp_vector.X = _positionX + _boxLength / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);
            // Titik 3
            temp_vector.X = _positionX - _boxLength / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z
            _vertices.Add(temp_vector);

            // Titik 4
            temp_vector.X = _positionX + _boxLength / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ - _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);

            // Titik 5
            temp_vector.X = _positionX - _boxLength / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);

            // Titik 6
            temp_vector.X = _positionX + _boxLength / 2.0f; // x
            temp_vector.Y = _positionY + _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);

            // Titik 7
            temp_vector.X = _positionX - _boxLength / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);

            // Titik 8
            temp_vector.X = _positionX + _boxLength / 2.0f; // x
            temp_vector.Y = _positionY - _boxLength / 2.0f; // y
            temp_vector.Z = _positionZ + _boxLength / 2.0f; // z

            _vertices.Add(temp_vector);
            //2. Inisialisasi index vertex
            _indices = new List<uint> {
                // Segitiga Depan 1
                0, 1, 2,
                // Segitiga Depan 2
                1, 2, 3,
                // Segitiga Atas 1
                0, 4, 5,
                // Segitiga Atas 2
                0, 1, 5,
                // Segitiga Kanan 1
                1, 3, 5,
                // Segitiga Kanan 2
                3, 5, 7,
                // Segitiga Kiri 1
                0, 2, 4,
                // Segitiga Kiri 2
                2, 4, 6,
                // Segitiga Belakang 1
                4, 5, 6,
                // Segitiga Belakang 2
                5, 6, 7,
                // Segitiga Bawah 1
                2, 3, 6,
                // Segitiga Bawah 2
                3, 6, 7
            };

        }

        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            model *= Matrix4.CreateTranslation(-pivot);
            model *= arbRotationMatrix;
            model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            rotationCenter = getRotationResult(pivot, vector, radAngle, rotationCenter);
            objectCenter = getRotationResult(pivot, vector, radAngle, objectCenter);

            foreach (var i in child)
            {
                i.rotate(pivot, vector, angle);
            }
        }


        public void rotate2(Vector3 pivot, Vector3 vector, float angle)
        {
            //pivot -> mau rotate di titik mana
            //vector -> mau rotate di sumbu apa? (x,y,z)
            //angle -> rotatenya berapa derajat?

            angle = MathHelper.DegreesToRadians(angle);

            //mulai ngerotasi
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = getRotationResult(pivot, vector, angle, _vertices[i]);
            }
            //rotate the euler direction
            for (int i = 0; i < 3; i++)
            {
                _euler[i] = getRotationResult(pivot, vector, angle, _euler[i], true);

                //NORMALIZE
                //LANGKAH - LANGKAH
                //length = akar(x^2+y^2+z^2)
                float length = (float)Math.Pow(Math.Pow(_euler[i].X, 2.0f) + Math.Pow(_euler[i].Y, 2.0f) + Math.Pow(_euler[i].Z, 2.0f), 0.5f);
                Vector3 temporary = new Vector3(0, 0, 0);
                temporary.X = _euler[i].X / length;
                temporary.Y = _euler[i].Y / length;
                temporary.Z = _euler[i].Z / length;
                _euler[i] = temporary;
            }
            _centerPosition = getRotationResult(pivot, vector, angle, _centerPosition);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * Vector3.SizeInBytes,
                _vertices.ToArray(), BufferUsageHint.StaticDraw);
        }

        Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;
            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                (float)temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                (float)temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));
            newPosition.Y =
                (float)temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                (float)temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                (float)temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));
            newPosition.Z =
                (float)temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                (float)temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                (float)temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }
    }
}
