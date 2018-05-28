using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DroneController
{
    public partial class Form1 : Form
    {
        public List<Point> polygon;
        public Point startPoint;
        public bool isDrawing = false;
        public bool isPickingStart = false;
        public Graphics drawArea;
        public Pen pen;
        public Point currentMousePosition;
        public Form1()
        {
            InitializeComponent();
            polygon = new List<Point>();
            prepareGraphics();
        }

        public void prepareGraphics()
        {
            drawArea = panel1.CreateGraphics();
        }

        public void drawLine(Color color, int width, bool isDashed, Point start, Point end)
        {
            pen = new Pen(color, width);
            if (isDashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            drawArea.DrawLine(pen, start, end);
        }

        public void drawRectangle(Color color, int width, bool isDashed, Point start, Point size)
        {
            pen = new Pen(color, width);
            if (isDashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            drawArea.DrawRectangle(pen, new Rectangle(start.X - size.X/2, start.Y - size.Y/2, size.X, size.Y));
        }

        private void startBtn_Click(object sender, EventArgs e)
        {

        }

        private void drawTimer_Tick(object sender, EventArgs e)
        {
            drawArea.Clear(Color.White);
            //draw paths

            //draw polygon
            if (polygon.Count > 0)
            {
                var prevPoint = polygon[0];
                for(int i = 1; i < polygon.Count; i++)
                {
                    drawLine(Color.Green, 2, false, prevPoint, polygon[i]);
                    prevPoint = polygon[i];
                }
                if(isDrawing)
                {
                    if (currentMousePosition != null)
                        drawLine(Color.Red, 1, true, prevPoint, currentMousePosition);
                }
                else
                {
                    drawLine(Color.Green, 2, false, prevPoint, polygon[0]);
                }
            }

            //draw startPoint
            if(startPoint != null)
            {
                if(isPickingStart)
                    drawRectangle(Color.Red, 1, true, currentMousePosition, new Point(10, 10));
                else
                    drawRectangle(Color.Red, 2, false, startPoint, new Point(10, 10));
            }
        }

        private void btnDrawPolygon_Click(object sender, EventArgs e)
        {
            if(isDrawing)
            {
                isDrawing = false;
                calculateSquare();
                //end drawing
            }
            else
            {
                polygon.Clear();
                isDrawing = true;
                //start drawing
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if(isDrawing)
            {
                if (e.Button == MouseButtons.Left)
                    polygon.Add(new Point(e.Location.X, e.Location.Y));
                if (e.Button == MouseButtons.Right)
                {
                    isDrawing = false;
                    calculateSquare();
                }
            }
            if(isPickingStart)
            {
                startPoint = new Point(e.Location.X, e.Location.Y);
                isPickingStart = false;
            }


        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePosition = new Point(e.Location.X, e.Location.Y);
            lblCursorX.Text = e.Location.X + "";
            lblCursorY.Text = e.Location.Y + "";
        }

        private void calculateSquare()
        {
            int sum1 = 0, sum2 = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                if (i < polygon.Count - 1)
                {
                    sum1 += (polygon[i].X * polygon[i + 1].Y);
                    sum2 += (polygon[i].Y * polygon[i + 1].X);
                }
                else
                {
                    sum1 += (polygon[i].X * polygon[0].Y);
                    sum2 += (polygon[i].Y * polygon[0].X);
                }
            }
            int result = sum2 - sum1;
            double square = Math.Abs(result / 2);

            lblSquare.Text = square.ToString();
        }

        private void btnPickStart_Click(object sender, EventArgs e)
        {
            if(isPickingStart)
            {
                isPickingStart = false;
                startPoint = new Point(0, 0);
            }
            else
            {
                isPickingStart = true;
            }
        }
    }
}
