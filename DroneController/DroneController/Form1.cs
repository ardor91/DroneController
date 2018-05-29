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
        public Pen debugPen = new Pen(Color.Red, 2);
        public Pen triPen = new Pen(Color.Blue, 1);

        public List<Debug> debug;
        public List<Triangle> triangles;
        public Form1()
        {
            InitializeComponent();
            polygon = new List<Point>();
            debug = new List<Debug>();
            triangles = new List<Triangle>();
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

            drawArea.DrawRectangle(pen, new Rectangle(start.X - size.X / 2, start.Y - size.Y / 2, size.X, size.Y));
        }

        public void drawTriangle(Color color, int width, bool isDashed, Point p1, Point p2, Point p3)
        {
            pen = new Pen(color, width);
            if (isDashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            drawArea.DrawLine(pen, p1, p2);
            drawArea.DrawLine(pen, p2, p3);
            drawArea.DrawLine(pen, p3, p1);
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
                for (int i = 1; i < polygon.Count; i++)
                {
                    drawArea.FillRectangle(new SolidBrush(Color.Orange), new Rectangle(polygon[i], new Size(5, 5)));
                    drawLine(Color.Green, 2, false, prevPoint, polygon[i]);
                    prevPoint = polygon[i];
                }
                if (isDrawing)
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
            if (startPoint != null)
            {
                if (isPickingStart)
                    drawRectangle(Color.Red, 1, true, currentMousePosition, new Point(10, 10));
                else
                    drawRectangle(Color.Red, 2, false, startPoint, new Point(10, 10));
            }

            //draw debug
            /*foreach(var deb in debug)
            {
                deb.DrawObject(drawArea, debugPen);
            }*/

            //draw triangles
            foreach (var tri in triangles)
            {
                tri.DrawObject(drawArea, triPen);
            }
        }

        private void btnDrawPolygon_Click(object sender, EventArgs e)
        {
            if (isDrawing)
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
            if (isDrawing)
            {
                if (e.Button == MouseButtons.Left)
                    polygon.Add(new Point(e.Location.X, e.Location.Y));
                if (e.Button == MouseButtons.Right)
                {
                    isDrawing = false;
                    calculateSquare();
                }
            }
            if (isPickingStart)
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
            if (isPickingStart)
            {
                isPickingStart = false;
                startPoint = new Point(0, 0);
            }
            else
            {
                isPickingStart = true;
            }
        }

        private void markupBtn_Click(object sender, EventArgs e)
        {
            debug.Clear();
            if (polygon.Count < 3)
            {
                MessageBox.Show("Create correct polygon!");
                return;
            }
            bool isSplitted = true;
            while (polygon.Count >= 3 && isSplitted)
            {
                isSplitted = false;
                //triangulationMarkup(20);
                List<Point> toRemove = new List<Point>();
                for (int i = 2; i < polygon.Count; i++)
                {
                    if (IsGoodTriangle(polygon[i - 2], polygon[i - 1], polygon[i], 5))
                    {
                        triangles.Add(new Triangle(polygon[i - 2], polygon[i - 1], polygon[i]));
                        //polygon.RemoveAt(i - 1);
                        polygon.Remove(polygon[i - 1]);
                        isSplitted = true;
                        //toRemove.Add(polygon[i - 1]);
                        //AddDebug(polygon[i - 2], polygon[i - 1], polygon[i]);
                        //drawTriangle(Color.Blue, 2, true, polygon[i - 2], polygon[i - 1], polygon[i]);
                    }
                }
            }
            MessageBox.Show("Finished");
            /*foreach(Point rem in toRemove)
            {
                polygon.Remove(rem);
            }*/
            return;

            //select start & end point
            var basePoint = startPoint;
            var nearestPoint = polygon[0];
            var farestPoint = polygon[0];
            var minDistance = getDistance(basePoint, nearestPoint);
            var maxDistance = minDistance;

            foreach (var point in polygon)
            {
                var distance = getDistance(basePoint, point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = point;
                }
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farestPoint = point;
                }
            }

            AddDebug(nearestPoint);
            AddDebug(farestPoint);
            AddDebug(nearestPoint, farestPoint);
            //get angle of new polared coord axis
            double angle = getAngle(nearestPoint, farestPoint, new Point(farestPoint.X, nearestPoint.Y));
            lblAngle.Text = angle.ToString();

            var d = new Debug(ObjectType.RECTANGLE);
                
            

            foreach (var point in polygon)
            {
                var tPoint = getTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                //AddDebug(new Point((int)tPoint.X + nearestPoint.X, (int)tPoint.Y + nearestPoint.Y));
                d.AddPoint(new Point((int)tPoint.X + nearestPoint.X, (int)tPoint.Y + nearestPoint.Y));                
            }
            debug.Add(d);

            //search points less X0 and more X0
            bool havePointsLess0 = false;
            bool havePointsMore0 = false;
            bool havePointsUpper0 = false;
            bool havePointsDown0 = false;
            int less = 0, more = 0, up = 0, down = 0;

            foreach (var point in polygon)
            {
                var tPoint = getTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (tPoint.X < 0)
                {
                    havePointsLess0 = true;
                    less++;
                    //break;
                }
            }

            foreach (var point in polygon)
            {
                var tPoint = getTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (tPoint.X > 0)
                {
                    havePointsMore0 = true;
                    more++;
                    //break;
                }
            }

            foreach (var point in polygon)
            {
                var tPoint = getTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (tPoint.Y > 0)
                {
                    havePointsUpper0 = true;
                    up++;
                    //break;
                }
            }

            foreach (var point in polygon)
            {
                var tPoint = getTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (tPoint.Y < 0)
                {
                    havePointsDown0 = true;
                    down++;
                    //break;
                }
            }

            lblMoreCount.Text = more.ToString();
            lblLessCount.Text = less.ToString();
            lblUpCount.Text = up.ToString();
            lblDownCount.Text = down.ToString();
        }

        public double getDistance(Point p1, Point p2)
        {
            return Math.Abs(Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2))));
        }

        public PointF getTranslatedPoint(double angle, Point p)
        {
            var newPoint = new PointF();
            var tAngle = angle * (Math.PI / 180);
            newPoint.X = (float)(p.X * Math.Cos(tAngle) - p.Y * Math.Sin(tAngle));
            newPoint.Y = (float)(p.X * Math.Sin(tAngle) + p.Y * Math.Cos(tAngle));

            return newPoint;
        }

        public double getAngle(Point p0, Point p1, Point p2)
        {
            /*var katet = Math.Abs(p2.X - p0.X);
            var katet2 = Math.Abs(p1.Y - p0.Y);
            var gipo = Math.Sqrt(Math.Pow(katet, 2) + Math.Pow(katet2, 2));

            return Math.Acos(katet / gipo) * (180 / Math.PI);*/

            var skalar = (p1.X - p0.X) * (p2.X - p0.X) + (p1.Y - p0.Y) * (p2.Y - p0.Y);
            var length1 = getDistance(p0, p1);
            var length2 = getDistance(p0, p2);

            return Math.Acos(skalar / (length1 * length2)) * (180 / Math.PI);
        }

        public void AddDebug(params Point[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE);
            foreach(var pp in p)
                d.AddPoint(pp);
            debug.Add(d);
        }

        private void markupBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            polygon.Clear();
            polygon.Add(new Point(300, 300));
            polygon.Add(new Point(300, 200));
            polygon.Add(new Point(200, 200));
            polygon.Add(new Point(200, 300));
        }

        public void triangulationMarkup(int threshold)
        {
            var flag = true;
            int currentIndex = 0;
            while(flag)
            {
                for(int i = 1; i < polygon.Count; i++)
                {
                    SplitLine(i - 1, i, threshold);
                }
                SplitLine(polygon.Count - 1, 0, threshold);

                flag = false;
                for(int i=1; i<polygon.Count;i++)
                {
                    if(getDistance(polygon[i-1], polygon[i]) > threshold)
                    {
                        flag = true;
                        break;
                    }
                }
                if (getDistance(polygon[polygon.Count-1], polygon[0]) > threshold)
                {
                    flag = true;
                    
                }
            }
        }

        public bool SplitLine(int startIndex, int endIndex, int threshold)
        {
            var p1 = polygon[startIndex];
            var p2 = polygon[endIndex];
            var distance = getDistance(p1, p2);

            if(distance > threshold)
            {
                var newPoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                polygon.Insert(endIndex, newPoint);
                AddDebug(newPoint);
                return true;
            }
            return false;
        }

        public float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        public bool IsInsideTriangle(Point point, Point p1, Point p2, Point p3)
        {
            bool b1, b2, b3;

            b1 = Sign(point, p1, p2) < 0.0f;
            b2 = Sign(point, p2, p3) < 0.0f;
            b3 = Sign(point, p3, p1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        public bool IsGoodTriangle(Point p1, Point p2, Point p3, int angleThreshold)
        {
            var p1p2 = getDistance(p1, p2);
            var p1p3 = getDistance(p1, p3);
            var p2p3 = getDistance(p2, p3);

            if (p1p2 + p1p3 <= p2p3) return false;
            if (p1p2 + p2p3 <= p1p3) return false;
            if (p2p3 + p1p3 <= p1p2) return false;

            var p1angle = getAngle(p1, p2, p3);
            var p2angle = getAngle(p2, p1, p3);
            var p3angle = getAngle(p3, p2, p1);

            if (p1angle < angleThreshold) return false;
            if (p2angle < angleThreshold) return false;
            if (p3angle < angleThreshold) return false;

            var newPoint = new Point((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);
            var zeroPoint = new Point(0, 0);
            int intersectionsCount = 0;
            for (int i = 1; i < polygon.Count; i++)
            {
                if(IsIntersecting(newPoint, zeroPoint, polygon[i-1], polygon[i]))
                {
                    intersectionsCount++;
                }
            }

            if (intersectionsCount % 2 == 0) return false;

            //MessageBox.Show((p1angle + p2angle + p3angle).ToString());

            return true;
        }

        public bool IsIntersecting(Point a, Point b, Point c, Point d)
        {
            float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            debug.Clear();
        }
    }
}