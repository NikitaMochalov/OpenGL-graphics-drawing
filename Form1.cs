using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;

namespace WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool showSpiral, showSphere, showSurface, showCube, showFractal, showCone, showTorus, showDodechahedron = false;


        float sphereSpeed = 0;

        float cubeRotation = 0;
        float cubeRotationSpeed = 0;
        float cubeSize = 1;
        float wantedCubeSize = 1;

        float cubeX = 0;
        float cubeY = 0;
        float cubeXSpeed = 0;

        float surfaceSpeed = 0.01f;

        int fractalDepth = 4;

        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            // Создаем экземпляр
            OpenGL gl = this.openGLControl1.OpenGL;

            // Очистка экрана и буфера глубин
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // Сбрасываем модельно-видовую матрицу
            gl.LoadIdentity();

            
            // Двигаем перо вглубь экрана
            gl.Translate(0.0f, 0.0f, -15.0f);

            

            if (wantedCubeSize > cubeSize) cubeSize += 0.05f;
            if (wantedCubeSize < cubeSize) cubeSize -= 0.05f;
            // Спираль
            if(showSpiral)
            {
                gl.Rotate(90 + sphereSpeed, 0.8f, 0.5f, 0f);
                Circle.DrawSpiral(gl);
                gl.Rotate(-90 - sphereSpeed, 0.8f, 0.5f, 0f);
            }
            // Сфера
            if (showSphere)
            {
                gl.Rotate(90 + sphereSpeed, 0.8f, 0.5f, 0f);
                Sphere.Draw(gl, 100, 36, 2, 0, 0);
                gl.Rotate(-(90 + sphereSpeed), 0.8f, 0.5f, 0f);
            }
            sphereSpeed += 2;
            if (showCube)
            {
                // Куб
                gl.Translate(cubeX, cubeY, 0f);
                Cube.Draw(gl, cubeSize, cubeRotation, 0, 0);
                cubeRotation += cubeRotationSpeed;
                cubeX += cubeXSpeed;
                if (cubeXSpeed != 0)
                    cubeY = (float)Math.Sin((double)cubeX * 10);
                gl.Translate(-cubeX, -cubeY, 0f);
            }
            if (showSurface)
            {
                //Surface
                gl.Translate(-3, -3, 0f);
                Surface.Draw(gl, 0, 0, surfaceSpeed);
                surfaceSpeed += 1f;
                gl.Translate(3, 3, 0f);
            }
            if (showFractal) Fractal.Draw(gl, -6, -6, fractalDepth);
            if (showCone)
            {
                gl.Rotate(90 + sphereSpeed, 0.8f, 0.5f, 0f);
                Sphere.Draw(gl, 90, 3, 2, 0, 0);
                gl.Rotate(-(90 + sphereSpeed), 0.8f, 0.5f, 0f);
            }
            if (showTorus)
            {
                gl.Rotate(90 + sphereSpeed, 0.8f, 0.5f, 0f);
                Sphere.Draw(gl, 360, 36, 1, 3, 0);
                gl.Rotate(-(90 + sphereSpeed), 0.8f, 0.5f, 0f);
            }

            if(showDodechahedron) Chedron.Draw(gl, 3, sphereSpeed, 0, 0);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") wantedCubeSize = float.Parse(textBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            showSpiral = !showSpiral;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            showSphere = !showSphere;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            showSurface = !showSurface;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            showFractal = !showFractal;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            showCone = !showCone;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            showDodechahedron = !showDodechahedron;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            showTorus = !showTorus;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "") fractalDepth = Convert.ToInt16(textBox6.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            showCube = !showCube;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "") cubeRotationSpeed = float.Parse(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "") cubeXSpeed = float.Parse(textBox3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "") cubeX = float.Parse(textBox4.Text);
            if (textBox5.Text != "") cubeY = float.Parse(textBox5.Text);
        }
    }

    public static class Chedron
    {
        public static void Draw(OpenGL gl, float width, float yRotateSpeed, float X, float Y)
        {
            float[] center = { X, Y, 0 };

            float fi = 2.51f;

            float half = width / 2;
            float b = half / (fi + half);
            float a = b * fi;

            gl.Rotate(yRotateSpeed, 1f, 0.1f, 0f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1f, 1f, 0.5f);
            float[,] points1 = { {half + a,  0, -a}, {half + a, 0, a }, {half, -half, half}, { a, -(half + a), 0}, { half, -half, -half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points1[i - 1, 0], center[1] + points1[i - 1, 1], points1[i - 1, 2]);
                gl.Vertex(center[0] + points1[i, 0], center[1] + points1[i, 1], points1[i, 2]);
            }
            gl.Vertex(center[0] + points1[4, 0], center[1] + points1[4, 1], points1[4, 2]);
            gl.Vertex(center[0] + points1[0, 0], center[1] + points1[0, 1], points1[0, 2]);

            float[,] points2 = { { -a, -(half + a), 0 }, {-half, -half, half }, {-(half+a),0,a }, { -(half + a), 0, -a },{-half,-half,-half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points2[i - 1, 0], center[1] + points2[i - 1, 1], points2[i - 1, 2]);
                gl.Vertex(center[0] + points2[i, 0], center[1] + points2[i, 1], points2[i, 2]);
            }
            gl.Vertex(center[0] + points2[4, 0], center[1] + points2[4, 1], points2[4, 2]);
            gl.Vertex(center[0] + points2[0, 0], center[1] + points2[0, 1], points2[0, 2]);

            float[,] points3 = { { -(half + a), 0, a }, { -(half + a), 0, -a }, { -half, half, -half },{ -a, half+a,0}, {-half, half, half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points3[i - 1, 0], center[1] + points3[i - 1, 1], points3[i - 1, 2]);
                gl.Vertex(center[0] + points3[i, 0], center[1] + points3[i, 1], points3[i, 2]);
            }
            gl.Vertex(center[0] + points3[4, 0], center[1] + points3[4, 1], points3[4, 2]);
            gl.Vertex(center[0] + points3[0, 0], center[1] + points3[0, 1], points3[0, 2]);

            float[,] points4 = { { half + a, 0, a }, { half + a, 0, -a }, { half, half, -half }, { a, half + a, 0 }, { half, half, half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points4[i - 1, 0], center[1] + points4[i - 1, 1], points4[i - 1, 2]);
                gl.Vertex(center[0] + points4[i, 0], center[1] + points4[i, 1], points4[i, 2]);
            }
            gl.Vertex(center[0] + points4[4, 0], center[1] + points4[4, 1], points4[4, 2]);
            gl.Vertex(center[0] + points4[0, 0], center[1] + points4[0, 1], points4[0, 2]);

            float[,] points5 = { { a, half + a, 0 }, { -a, half + a, 0 }, { -half, half, half }, { 0, a, half + a }, { half, half, half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points5[i - 1, 0], center[1] + points5[i - 1, 1], points5[i - 1, 2]);
                gl.Vertex(center[0] + points5[i, 0], center[1] + points5[i, 1], points5[i, 2]);
            }
            gl.Vertex(center[0] + points5[4, 0], center[1] + points5[4, 1], points5[4, 2]);
            gl.Vertex(center[0] + points5[0, 0], center[1] + points5[0, 1], points5[0, 2]);

            float[,] points6 = { { a, half + a, 0 }, { -a, half + a, 0 }, { -half, half, -half }, { 0, a, -(half + a) }, { half, half, -half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points6[i - 1, 0], center[1] + points6[i - 1, 1], points6[i - 1, 2]);
                gl.Vertex(center[0] + points6[i, 0], center[1] + points6[i, 1], points6[i, 2]);
            }
            gl.Vertex(center[0] + points6[4, 0], center[1] + points6[4, 1], points6[4, 2]);
            gl.Vertex(center[0] + points6[0, 0], center[1] + points6[0, 1], points6[0, 2]);

            float[,] points7 = { { a, -(half + a), 0 }, { -a, -(half + a), 0 }, { -half, -half, -half }, { 0, -a, -(half + a) }, { half, -half, -half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points7[i - 1, 0], center[1] + points7[i - 1, 1], points7[i - 1, 2]);
                gl.Vertex(center[0] + points7[i, 0], center[1] + points7[i, 1], points7[i, 2]);
            }
            gl.Vertex(center[0] + points7[4, 0], center[1] + points7[4, 1], points7[4, 2]);
            gl.Vertex(center[0] + points7[0, 0], center[1] + points7[0, 1], points7[0, 2]);

            float[,] points8 = { { a, -(half + a), 0 }, { -a, -(half + a), 0 }, { -half, -half, half }, { 0, -a, half + a }, { half, -half, half } };

            for (int i = 1; i < 5; i++)
            {
                gl.Vertex(center[0] + points8[i - 1, 0], center[1] + points8[i - 1, 1], points8[i - 1, 2]);
                gl.Vertex(center[0] + points8[i, 0], center[1] + points8[i, 1], points8[i, 2]);
            }
            gl.Vertex(center[0] + points8[4, 0], center[1] + points8[4, 1], points8[4, 2]);
            gl.Vertex(center[0] + points8[0, 0], center[1] + points8[0, 1], points8[0, 2]);

            float[,] points9 = { { 0, -a, -(half+a) }, { 0, a, -(half + a) }, { 0, -a, half + a }, { 0, a, half + a }};

            gl.Vertex(center[0] + points9[0, 0], center[1] + points9[0, 1], points9[0, 2]);
            gl.Vertex(center[0] + points9[1, 0], center[1] + points9[1, 1], points9[1, 2]);

            gl.Vertex(center[0] + points9[2, 0], center[1] + points9[2, 1], points9[2, 2]);
            gl.Vertex(center[0] + points9[3, 0], center[1] + points9[3, 1], points9[3, 2]);

            gl.End();
            gl.Rotate(-yRotateSpeed, 1f, 0.1f, 0f);
        }
    }

    public static class Fractal
    {
        public static void Draw(OpenGL gl, float X, float Y, int depth)
        {
            float[] center = { X, Y };
            int nDepth = depth;
            float sideL = 12;
            float[,] firstDots = { { 0, sideL }, { 0, 0 }, { sideL, 0 } };

            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1f, 1f, 1f);

            gl.Vertex(center[0] + firstDots[0, 0], center[1] + firstDots[0, 1]);
            gl.Vertex(center[0] + firstDots[1, 0], center[1] + firstDots[1, 1]);

            gl.Vertex(center[0] + firstDots[1, 0], center[1] + firstDots[1, 1]);
            gl.Vertex(center[0] + firstDots[2, 0], center[1] + firstDots[2, 1]);

            gl.Vertex(center[0] + firstDots[2, 0], center[1] + firstDots[2, 1]);
            gl.Vertex(center[0] + firstDots[0, 0], center[1] + firstDots[0, 1]);

            Fractal.DrawNext(nDepth, gl, new PointF(0, sideL), sideL, center);
            Fractal.DrawNext(nDepth, gl, new PointF(0, sideL/2), sideL, center);
            Fractal.DrawNext(nDepth, gl, new PointF(sideL/2, sideL/2), sideL, center);

            gl.End();
        }

        public static void DrawNext(int currentDepth, OpenGL gl, PointF upDot, float sideL, float[] center)
        {
            if (currentDepth != 0)
            {

                PointF bLeft = new PointF(upDot.X, upDot.Y - (sideL / 2));
                PointF bRight = new PointF(upDot.X + (sideL / 2), upDot.Y - (sideL / 2));

                gl.Vertex(center[0] + upDot.X, center[1] + upDot.Y);
                gl.Vertex(center[0] + bLeft.X, center[1] + bLeft.Y);

                gl.Vertex(center[0] + bLeft.X, center[1] + bLeft.Y);
                gl.Vertex(center[0] + bRight.X, center[1] + bRight.Y);

                gl.Vertex(center[0] + bRight.X, center[1] + bRight.Y);
                gl.Vertex(center[0] + upDot.X, center[1] + upDot.Y);

                Fractal.DrawNext(currentDepth-1, gl, upDot, sideL/2, center);
                Fractal.DrawNext(currentDepth-1, gl, new PointF(bLeft.X, bLeft.Y + sideL/4), sideL/2, center);
                Fractal.DrawNext(currentDepth-1, gl, new PointF(bRight.X -sideL/4, bRight.Y+sideL/4), sideL/2, center);
            }
        }
    }

    public static class Surface
    {
        public static void Draw(OpenGL gl, float X, float Y, float speed)
        {
            float[] center = { X, Y, 0 };
            int n = 8;
            float step = 0.2f;

            gl.Rotate(speed, 1f, 1, 1);
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0.5f, 1f, 0.5f);
            for (float i = step; i <= n; i += step)
            {
                for (float j = 0; j <= n; j += step)
                {
                    gl.Vertex(center[0] + i- step, center[1] + j, Math.Sin(i - step) *Math.Sin(j));
                    gl.Vertex(center[0] + i, center[1] + j, Math.Sin(i)*Math.Sin(j));
                }
            }
            for (float i = 0; i <= n; i += step)
            {
                for (float j = step; j <= n; j += step)
                {
                    gl.Vertex(center[0] + i, center[1] + j - step, Math.Sin(i)*Math.Sin(j- step));
                    gl.Vertex(center[0] + i, center[1] + j, Math.Sin(i)*Math.Sin(j));
                }
            }
            gl.End();
            gl.Rotate(-speed, 1f, 1, 1);
        }
    }

    public static class Cube
    {
        public static void Draw(OpenGL gl, float width, float yRotateSpeed, float X, float Y )
        {
            int n = 8;
            float[,] points = { { 1, 1, 1}, {-1, 1, 1},{-1,-1, 1 },{1, -1, 1 },
                { 1, 1, -1}, {-1, 1, -1},{-1,-1, -1 },{1, -1, -1 } };
            float[] center = {X,Y,0 };

            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    points[i, j] *= width;
                }
            }
            gl.Rotate(yRotateSpeed, 0.0f, 1f, 0.0f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1f, 0f, 0.5f);


            for (int i = 1; i < 4; i++)
            {
                gl.Vertex(center[0] + points[i - 1, 0], center[1] + points[i - 1, 1], points[i - 1, 2]);
                gl.Vertex(center[0] + points[i, 0], center[1] + points[i, 1], points[i, 2]);
            }
            gl.Vertex(center[0] + points[3, 0], center[1] + points[3, 1], points[3, 2]);
            gl.Vertex(center[0] + points[0, 0], center[1] + points[0, 1], points[0, 2]);

            for (int i = 5; i < 8; i++)
            {
                gl.Vertex(center[0] + points[i - 1, 0], center[1] + points[i - 1, 1], points[i - 1, 2]);
                gl.Vertex(center[0] + points[i, 0], center[1] + points[i, 1], points[i, 2]);
            }
            gl.Vertex(center[0] + points[4, 0], center[1] + points[4, 1], points[4, 2]);
            gl.Vertex(center[0] + points[7, 0], center[1] + points[7, 1], points[7, 2]);

            for (int i = 4; i < 8; i++)
            {
                gl.Vertex(center[0] + points[i - 4, 0], center[1] + points[i - 4, 1], points[i - 4, 2]);
                gl.Vertex(center[0] + points[i, 0], center[1] + points[i, 1], points[i, 2]);
            }
            
            gl.End();
            gl.Rotate(-yRotateSpeed, 0.0f, 1f, 0.0f);
        }
    }

    public static class Circle
    {
        public static void Draw(OpenGL gl)
        {
            int n = 6;
            int rotation = 0;
            double radius = 1;
            PointF center = new PointF(0, 0);

            PointF[] points = new PointF[n];
            double angleRad = (360.0 / n) * (Math.PI / 180.0);
            double rotationRad = rotation * (Math.PI / 180.0);

            for (int i = 0; i < n; i++)
            {
                double dY = radius * Math.Cos(angleRad * i - rotationRad);
                double dX = radius * Math.Sin(angleRad * i - rotationRad);

                points[i].X = center.X - (float)dX;
                points[i].Y = center.Y - (float)dY;
            }

            for (int i = 1; i < n; i++)
            {
                gl.Color(0.5f, 1f, 0.5f);
                gl.Vertex(points[i - 1].X, points[i - 1].Y);
                gl.Vertex(points[i].X, points[i].Y);
            }
            gl.Color(0.5f, 1f, 0.5f);
            gl.Vertex(points[n - 1].X, points[n - 1].Y);
            gl.Vertex(points[0].X, points[0].Y);
        }

        public static void DrawSpiral(OpenGL gl)
        {
            gl.LineWidth(2);
            gl.Rotate(30, 1f, 1f, 0.0f);
            gl.Begin(OpenGL.GL_LINES);

            int n = 36;
            int spiralLenngth = 7;
            float spiralAngle = 1f / n;
            int rotation = 0;
            double radius = 3;
            PointF center = new PointF(0, 0);

            PointF[] points = new PointF[n];
            double angleRad = (360.0 / n) * (Math.PI / 180.0);
            double rotationRad = rotation * (Math.PI / 180.0);

            for (int i = 0; i < n; i++)
            {
                double dY = radius * Math.Cos(angleRad * i - rotationRad);
                double dX = radius * Math.Sin(angleRad * i - rotationRad);

                points[i].X = center.X - (float)dX;
                points[i].Y = center.Y - (float)dY;
            }

            float prevPoint = 0;

            for (int j = 0; j < spiralLenngth; j++)
            {
                for (int i = 1; i < n; i++)
                {
                    gl.Color(0.5f, 1f, 0.5f);
                    gl.Vertex(points[i - 1].X, points[i - 1].Y, prevPoint);
                    gl.Vertex(points[i].X, points[i].Y, prevPoint - spiralAngle);
                    prevPoint -= spiralAngle;
                }
                gl.Color(0.5f, 1f, 0.5f);
                gl.Vertex(points[n - 1].X, points[n - 1].Y, prevPoint);
                gl.Vertex(points[0].X, points[0].Y, prevPoint - spiralAngle);
                prevPoint -= spiralAngle;
            }
            gl.End();
            gl.Rotate(-30, 1f, 1f, 0.0f);
        }
    }

    public static class Sphere
    {
        public static void Draw(OpenGL gl, int nCircles, int nCircleSides, double sphereRadius, float centerX, float centerY)
        {
            int n = nCircles;
            PointF center = new PointF(centerX, centerY);
            double radius = sphereRadius;

            for (int i = 0; i < n; i++)
            {
                gl.Rotate(0f, 360f / n, 0f);
                gl.Begin(OpenGL.GL_LINES);
                gl.Color(0f, 1f,1f);
                Sphere.DrawCircle(gl, radius, center, nCircleSides);
                
                gl.End();
            }
        }


        public static void DrawCircle(OpenGL gl, double radius1, PointF center1, int nSides)
        {
            int n = nSides;
            int rotation = 0;
            double radius = radius1;
            PointF center = center1;

            PointF[] points = new PointF[n];
            double angleRad = (360.0 / n) * (Math.PI / 180.0);
            double rotationRad = rotation * (Math.PI / 180.0);

            for (int i = 0; i < n; i++)
            {
                double dY = radius * Math.Cos(angleRad * i - rotationRad);
                double dX = radius * Math.Sin(angleRad * i - rotationRad);

                points[i].X = center.X - (float)dX;
                points[i].Y = center.Y - (float)dY;
            }

            for (int i = 1; i < n; i++)
            {
                gl.Vertex(points[i - 1].X, points[i - 1].Y);
                gl.Vertex(points[i].X, points[i].Y);
            }
            gl.Vertex(points[n - 1].X, points[n - 1].Y);
            gl.Vertex(points[0].X, points[0].Y);
        }


    }
}
