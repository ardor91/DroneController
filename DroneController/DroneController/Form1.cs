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
        public List<Point> translatedPolygon;
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
        public List<Line> lines;
        public Function Func = new Function();
        public Form1()
        {
            InitializeComponent();
            polygon = new List<Point>();
            translatedPolygon = new List<Point>();
            debug = new List<Debug>();
            triangles = new List<Triangle>();
            lines = new List<Line>();
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

            //draw translated polygon
            if (translatedPolygon.Count > 0)
            {
                var prev = translatedPolygon[0];
                for (int i = 1; i < translatedPolygon.Count; i++)
                {
                    drawLine(Color.OrangeRed, 2, false, prev, translatedPolygon[i]);
                    prev = translatedPolygon[i];
                }
                drawLine(Color.OrangeRed, 2, false, prev, translatedPolygon[0]);
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
            foreach(var deb in debug)
            {
                deb.DrawObject(drawArea, debugPen);
            }

            //draw triangles
            foreach (var tri in triangles)
            {
                tri.DrawObject(drawArea, triPen);
            }

            //draw lines
            foreach (var lni in lines)
            {
                lni.DrawObject(drawArea, triPen);
            }
        }

        private void btnDrawPolygon_Click(object sender, EventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                Func.CalculateSquare(polygon);
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
                    Func.CalculateSquare(polygon);
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
            /*bool isSplitted = true;
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
            MessageBox.Show("Finished");*/
            /*foreach(Point rem in toRemove)
            {
                polygon.Remove(rem);
            }*/
            //return;

            //select start & end point
            var basePoint = startPoint;
            var nearestPoint = polygon[0];
            var nearestPointIndex = 0;
            var farestPointIndex = 0;
            var farestPoint = polygon[0];
            var minDistance = Func.GetDistance(basePoint, nearestPoint);
            var maxDistance = minDistance;

            for (int i = 0; i < polygon.Count; i++)
            {
                var distance = Func.GetDistance(basePoint, polygon[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = polygon[i];
                    nearestPointIndex = i;
                }
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farestPoint = polygon[i];
                    farestPointIndex = i;
                }
            }

            AddDebug(nearestPoint);
            AddDebug(farestPoint);
            AddDebug(nearestPoint, farestPoint);
            //get angle of new polared coord axis
            double angle = Func.GetAngle(nearestPoint, farestPoint, new Point(farestPoint.X, nearestPoint.Y));
            lblAngle.Text = angle.ToString();

            var d = new Debug(ObjectType.RECTANGLE);


            translatedPolygon.Clear();
            foreach (var point in polygon)
            {
                var tPoint = Func.GetTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                translatedPolygon.Add(new Point((int)tPoint.X, (int)tPoint.Y));
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

            foreach (var point in translatedPolygon)
            {
                //var tPoint = Func.GetTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (point.X < 0)
                {
                    havePointsLess0 = true;
                    less++;
                    //break;
                }
            }

            foreach (var point in translatedPolygon)
            {
                //var tPoint = Func.GetTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (point.X > 0)
                {
                    havePointsMore0 = true;
                    more++;
                    //break;
                }
            }

            foreach (var point in translatedPolygon)
            {
                //var tPoint = Func.GetTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (point.Y > 0)
                {
                    havePointsUpper0 = true;
                    up++;
                    //break;
                }
            }

            foreach (var point in translatedPolygon)
            {
                //var tPoint = Func.GetTranslatedPoint(angle, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                if (point.Y < 0)
                {
                    havePointsDown0 = true;
                    down++;
                    //break;
                }
            }

            int step = 20;
            int nextNearestIndex = nearestPointIndex;
            int prevNearestIndex = nearestPointIndex;
            Line topLine = null, bottomLine = null;
            farestPoint = Func.GetTranslatedPoint(angle, nearestPoint, farestPoint);
            nearestPoint = Func.GetTranslatedPoint(angle, nearestPoint, nearestPoint);
            
            Point nextNearestPoint = nearestPoint;
            Point prevNearestPoint = nearestPoint;
            bool topFinished = false;
            bool bottomFinished = false;

            if(!havePointsLess0)
            {
                for (int i = nearestPoint.X + step; i < farestPoint.X; i += step)
                {
                    //if (topFinished && bottomFinished) break;
                    if (!topFinished && (topLine == null || i >= nextNearestPoint.X))
                    {
                        //get next point clockwise
                        nextNearestIndex++;
                        if (nextNearestIndex >= farestPointIndex)
                        {
                            nextNearestIndex = farestPointIndex;
                            topFinished = true;
                        }

                        var t = nextNearestPoint;
                        nextNearestPoint = translatedPolygon[nextNearestIndex];
                        topLine = new Line(t, nextNearestPoint);
                        //AddDebug(Color.Black, t, nextNearestPoint);
                    }

                    if (!bottomFinished && (bottomLine == null || i >= prevNearestPoint.X))
                    {
                        //get next point counter clockwise
                        prevNearestIndex--;
                        if (prevNearestIndex <= farestPointIndex && prevNearestIndex >= 0)
                        {
                            prevNearestIndex = farestPointIndex;
                            bottomFinished = true;
                        }
                        if (prevNearestIndex < 0) prevNearestIndex = translatedPolygon.Count - 1;

                        var t = prevNearestPoint;
                        prevNearestPoint = translatedPolygon[prevNearestIndex];
                        bottomLine = new Line(t, prevNearestPoint);
                        //AddDebug(Color.Black, t, prevNearestPoint);
                    }

                    //if (nextNearestIndex == prevNearestIndex) break;

                    //get next vertical line
                    var nY1L = topLine.P2.Y > topLine.P1.Y ? topLine.P1.Y : topLine.P2.Y;//Math.Abs(topLine.P1.Y - topLine.P2.Y);
                    var nY1G = topLine.P2.Y > topLine.P1.Y ? topLine.P2.Y : topLine.P1.Y;
                    nY1G -= nY1L;

                    var pY1L = bottomLine.P2.Y > bottomLine.P1.Y ? bottomLine.P1.Y : bottomLine.P2.Y;
                    var pY1G = bottomLine.P2.Y > bottomLine.P1.Y ? bottomLine.P2.Y : bottomLine.P1.Y;
                    pY1G -= pY1L;

                    var y1 = ((nY1G * (i - topLine.P1.X)) / (topLine.P2.X - topLine.P1.X)) + nY1L;
                    var y2 = ((pY1G * (i - bottomLine.P1.X)) / (bottomLine.P2.X - bottomLine.P1.X)) + pY1L;//prevNearestPoint.X;
                    lines.Add(new Line(i, y1, i, y2));
                }
            }

            lblMoreCount.Text = more.ToString();
            lblLessCount.Text = less.ToString();
            lblUpCount.Text = up.ToString();
            lblDownCount.Text = down.ToString();
        }

        public void AddDebug(params Point[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE);
            foreach(var pp in p)
                d.AddPoint(pp);
            debug.Add(d);
        }
        public void AddDebug(Color color, params Point[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE, new List<Point>(), color);
            foreach (var pp in p)
                d.AddPoint(pp);
            debug.Add(d);
        }

        private void markupBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            polygon.Clear();
            polygon.Add(new Point(0, 0));
            polygon.Add(new Point(132, 132));
            polygon.Add(new Point(204, 302));
            polygon.Add(new Point(50, 209));
            startPoint = new Point(-40, 10);
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
                    if(Func.GetDistance(polygon[i-1], polygon[i]) > threshold)
                    {
                        flag = true;
                        break;
                    }
                }
                if (Func.GetDistance(polygon[polygon.Count-1], polygon[0]) > threshold)
                {
                    flag = true;
                    
                }
            }
        }

        public bool SplitLine(int startIndex, int endIndex, int threshold)
        {
            var p1 = polygon[startIndex];
            var p2 = polygon[endIndex];
            var distance = Func.GetDistance(p1, p2);

            if(distance > threshold)
            {
                var newPoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                polygon.Insert(endIndex, newPoint);
                AddDebug(newPoint);
                return true;
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            debug.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lines.Clear();
        }
    }
}