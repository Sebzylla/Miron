using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Lab4
{
    class Program
    {
        internal class Window3D : GameWindow
        {
            bool showCube = true;
            KeyboardState lastKeyPress;
            private const int XYZ_SIZE = 75;
            private bool axesControl = true;
            private int transStep = 0;
            private int radStep = 0;
            private int attStep = 0;

            private bool newStatus = false;
            private bool triVer = false;
            private bool pressedAgain = false;

            Random r = new Random();
            Color randomColor;
            Color c1, c2, c3;

            private int[,] objVertices = {
            {5, 10, 5,
                10, 5, 10,
                5, 10, 5,
                10, 5, 10,
                5, 5, 5,
                5, 5, 5,
                5, 10, 5,
                10, 10, 5,
                10, 10, 10,
                10, 10, 10,
                5, 10, 5,
                10, 10, 5},
            {5, 5, 12,
                5, 12, 12,
                5, 5, 5,
                5, 5, 5,
                5, 12, 5,
                12, 5, 12,
                12, 12, 12,
                12, 12, 12,
                5, 12, 5,
                12, 5, 12,
                5, 5, 12,
                5, 12, 12},
            {6, 6, 6,
                6, 6, 6,
                6, 6, 12,
                6, 12, 12,
                6, 6, 12,
                6, 12, 12,
                6, 6, 12,
                6, 12, 12,
                6, 6, 12,
                6, 12, 12,
                12, 12, 12,
                12, 12, 12}};
            private Color[] colorVertices = { Color.White, Color.LawnGreen, Color.WhiteSmoke, Color.Tomato, Color.Turquoise, Color.OldLace, Color.Olive, Color.MidnightBlue, Color.PowderBlue, Color.PeachPuff, Color.LavenderBlush, Color.MediumAquamarine };

            private Window3D() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                GL.ClearColor(Color.MidnightBlue);
                GL.Enable(EnableCap.DepthTest);
                GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
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

                showCube = true;
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
                    if (showCube)
                    {
                        showCube = false;
                    }
                    else
                    {
                        showCube = true;
                    }
                }
                if (keyboard[Key.R] && !keyboard.Equals(lastKeyPress))
                {
                    if (newStatus)
                    {
                        newStatus = false;
                    }
                    else
                    {
                        newStatus = true;
                    }
                }
                //Schimba culuarea fetei de sus a cubului
                if (keyboard[Key.C] && !keyboard.Equals(lastKeyPress))
                {
                    randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                    colorVertices[6] = randomColor;
                    randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                    colorVertices[7] = randomColor;
                }
                //Schimba culuarea pentru fiecare vertex al triunghiului initial verde din partea stanga
                if (keyboard[Key.V] && !keyboard.Equals(lastKeyPress))
                {
                    if (triVer == true)
                        triVer = false;
                    else
                        triVer = pressedAgain = true;
                }

                if (keyboard[Key.A])
                {
                    transStep--;
                }
                if (keyboard[Key.D])
                {
                    transStep++;
                }

                if (keyboard[Key.W])
                {
                    radStep--;
                }
                if (keyboard[Key.S])
                {
                    radStep++;
                }

                if (keyboard[Key.Up])
                {
                    attStep++;
                }
                if (keyboard[Key.Down])
                {
                    attStep--;
                }

                lastKeyPress = keyboard;
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                base.OnRenderFrame(e);

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                if (newStatus)
                {
                    DrawCube();
                }

                if (axesControl)
                {
                    DrawAxes();
                }

                if (showCube == true)
                {
                    GL.PushMatrix();
                    GL.Translate(transStep, attStep, radStep);
                    DrawCube();
                    GL.PopMatrix();
                }

                //GL.Flush();


                SwapBuffers();
            }

            private void DrawAxes()
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Red);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(XYZ_SIZE, 0, 0);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Yellow);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, XYZ_SIZE, 0); ;
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Green);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, XYZ_SIZE);
                GL.End();
            }

            private void DrawCube()
            {
                if (triVer == false)
                {
                    GL.Begin(PrimitiveType.Triangles);
                    for (int i = 0; i < 35; i = i + 3)
                    {
                        GL.Color3(colorVertices[i / 3]);
                        GL.Vertex3(objVertices[0, i], objVertices[1, i], objVertices[2, i]);
                        GL.Vertex3(objVertices[0, i + 1], objVertices[1, i + 1], objVertices[2, i + 1]);
                        GL.Vertex3(objVertices[0, i + 2], objVertices[1, i + 2], objVertices[2, i + 2]);
                    }
                    GL.End();
                }
                else
                {
                    if (pressedAgain == true)
                    {
                        GL.Begin(PrimitiveType.Triangles);

                        for (int i = 0; i < 32; i = i + 3)
                        {
                            GL.Color3(colorVertices[i / 3]);
                            GL.Vertex3(objVertices[0, i], objVertices[1, i], objVertices[2, i]);
                            GL.Vertex3(objVertices[0, i + 1], objVertices[1, i + 1], objVertices[2, i + 1]);
                            GL.Vertex3(objVertices[0, i + 2], objVertices[1, i + 2], objVertices[2, i + 2]);
                        }

                        Console.Clear();
                        randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                        Console.WriteLine("Vertex1 \nR:" + randomColor.R + " G:" + randomColor.G + " B:" + randomColor.B);
                        GL.Color3(randomColor);
                        GL.Vertex3(objVertices[0, 32], objVertices[1, 32], objVertices[2, 32]);
                        c1 = randomColor;

                        randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                        Console.WriteLine("Vertex2 \nR:" + randomColor.R + " G:" + randomColor.G + " B:" + randomColor.B);
                        GL.Color3(randomColor);
                        GL.Vertex3(objVertices[0, 33], objVertices[1, 33], objVertices[2, 33]);
                        c2 = randomColor;

                        randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                        Console.WriteLine("Vertex3 \nR:" + randomColor.R + " G:" + randomColor.G + " B:" + randomColor.B);
                        GL.Color3(randomColor);
                        GL.Vertex3(objVertices[0, 34], objVertices[1, 34], objVertices[2, 34]);
                        c3 = randomColor;

                        GL.End();
                        pressedAgain = false;
                    }
                    else
                    {
                        GL.Begin(PrimitiveType.Triangles);

                        for (int i = 0; i < 32; i = i + 3)
                        {
                            GL.Color3(colorVertices[i / 3]);
                            GL.Vertex3(objVertices[0, i], objVertices[1, i], objVertices[2, i]);
                            GL.Vertex3(objVertices[0, i + 1], objVertices[1, i + 1], objVertices[2, i + 1]);
                            GL.Vertex3(objVertices[0, i + 2], objVertices[1, i + 2], objVertices[2, i + 2]);
                        }

                        GL.Color3(c1);
                        GL.Vertex3(objVertices[0, 32], objVertices[1, 32], objVertices[2, 32]);

                        GL.Color3(c2);
                        GL.Vertex3(objVertices[0, 33], objVertices[1, 33], objVertices[2, 33]);

                        GL.Color3(c3);
                        GL.Vertex3(objVertices[0, 34], objVertices[1, 34], objVertices[2, 34]);

                        GL.End();
                    }
                }
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
