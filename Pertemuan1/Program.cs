using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Windowing.Desktop;

namespace Pertemuan1
{
    internal class Program
    {
        static void Main(String[] args)
        {

            var nativeWindowSetting = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 800),       //vector 2 dimensi integer
                Title = "Pertemuan 2"
            };

            //pake using supaya bisa di dispse dengan benar dan gak buang memori
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSetting))
            {
                window.Run();
            }
        }
    }
}
