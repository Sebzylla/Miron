using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Lab4_2
{
    class Axes
    {

        private bool visibility;
        private int xyzSize;

        public Axes()
        {
            visibility = true;
            xyzSize = 75;
        }

        public Axes(int _ax)
        {
            visibility = true;
            xyzSize = _ax;
        }

        public bool GetVisibility()
        {
            return visibility;
        }

        public void ShowMe()
        {
            visibility = true;
        }

        public void HideMe()
        {
            visibility = false;
        }

        public void ToggleVisibility()
        {
            if (visibility)
            {
                visibility = false;
            }
            else
            {
                visibility = true;
            }
        }

        public void DrawMe()
        {

            if (!visibility)
            {
                return;
            }

            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(xyzSize, 0, 0);
            GL.End();

            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, xyzSize, 0);
            GL.End();

            GL.Color3(Color.Blue);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, xyzSize);
            GL.End();
        }

    }

}
