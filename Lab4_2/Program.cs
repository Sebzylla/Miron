using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Lab4_2
{
    class Program
    {
        internal class Window3D : GameWindow
        {
            Axes xyz;
            ManualTriangle trg;

            KeyboardState lastKeyPress;
            private const int XYZ_SIZE = 75;

            private Window3D() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                GL.ClearColor(Color.MidnightBlue);
                GL.Enable(EnableCap.DepthTest);
                GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

                xyz = new Axes();
                trg = new ManualTriangle();
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);

                GL.Viewport(0, 0, Width, Height);

                double aspect_ratio = Width / (double)Height;

                Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref perspective);

                Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref lookat);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                base.OnUpdateFrame(e);

                KeyboardState keyboard = Keyboard.GetState();

                if (keyboard[Key.Escape])
                {
                    Exit();
                    return;
                }

                if (keyboard[Key.P] && !keyboard.Equals(lastKeyPress))
                {
                    xyz.ToggleVisibility();
                }

                if (keyboard[Key.L] && !keyboard.Equals(lastKeyPress))
                {
                    trg.ToggleVisibility();
                }

                lastKeyPress = keyboard;
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                base.OnRenderFrame(e);

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                xyz.DrawMe();

                trg.DrawMe();

                SwapBuffers();
            }


            [STAThread]
            static void Main(string[] args)
            {

                using (Window3D example = new Window3D())
                {
                    example.Run(30.0, 0.0);
                }

            }
        }
    }
}
