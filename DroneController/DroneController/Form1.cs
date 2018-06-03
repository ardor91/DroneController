﻿using System;
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
        public Pen oriPen = new Pen(Color.RosyBrown, 1);

        public List<Debug> debug;
        public List<Triangle> triangles;
        public List<Line> lines;
        public List<Line> originalLines;
        public Function Func = new Function();
        public Bitmap googleImage = null;

        bool showOriginal = true;
        int offsetX = 0;
        int offsetY = 0;

        StaticMap map = new StaticMap();
        MercatorProjection prj = new MercatorProjection();
        PointF relativeCenter = new PointF(0, 0);

        public Form1()
        {
            InitializeComponent();
            polygon = new List<Point>();
            translatedPolygon = new List<Point>();
            debug = new List<Debug>();
            triangles = new List<Triangle>();
            lines = new List<Line>();
            originalLines = new List<Line>();

            offsetX = panel1.Width / 4;
            offsetY = panel1.Height / 2;
            prepareGraphics();
            
        }

        public void prepareGraphics()
        {
            drawArea = panel1.CreateGraphics();
            drawArea.TranslateTransform(offsetX, offsetY);
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
            if (googleImage != null)
            //panel1.BackgroundImage = googleImage;
            {
                drawArea.DrawImage(googleImage, -panel1.Width/4, -panel1.Height/2);
            }

            if (polygon.Count > 0 && showOriginal)
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

            if (showTemp.Checked)
            {
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
            }

            //draw startPoint
            if (startPoint != null)
            {
                if (isPickingStart)
                    drawRectangle(Color.Red, 1, true, currentMousePosition, new Point(10, 10));
                else
                    drawRectangle(Color.Red, 2, false, startPoint, new Point(10, 10));
            }
            if (showTemp.Checked)
            {
                //draw debug
                foreach (var deb in debug)
                {
                    deb.DrawObject(drawArea, debugPen);
                }
            }
            if (showTemp.Checked)
            {
                //draw lines
                foreach (var lni in lines)
                {
                    lni.DrawObject(drawArea, triPen);
                }
            }
            //draw orlines
            foreach (var lni in originalLines)
            {
                lni.DrawObject(drawArea, oriPen);
            }
            
        }

        private void btnDrawPolygon_Click(object sender, EventArgs e)
        {
            showOriginal = true;
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
                    polygon.Add(new Point(e.Location.X - offsetX, e.Location.Y - offsetY));
                if (e.Button == MouseButtons.Right)
                {
                    isDrawing = false;
                    Func.CalculateSquare(polygon);
                }
            }
            if (isPickingStart)
            {
                startPoint = new Point(e.Location.X - offsetX, e.Location.Y - offsetY);
                isPickingStart = false;
            }
        }
        int oldMouseX = 0;
        int oldMouseY = 0;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed && !isDrawing)
            {
                offsetX += (oldMouseX - e.Location.X) * -1;
                offsetY += (oldMouseY - e.Location.Y) * -1;
                drawArea.TranslateTransform((oldMouseX - e.Location.X) * -1, (oldMouseY - e.Location.Y) * -1);
                oldMouseX = e.Location.X;
                oldMouseY = e.Location.Y;
            }
            currentMousePosition = new Point(
                e.Location.X,// - offsetX, 
                e.Location.Y);// - offsetY);
            lblCursorX.Text = currentMousePosition.X + "";
            lblCursorY.Text = currentMousePosition.Y + "";

            lblLat.Text = (leftBottomCorner.X + currentMousePosition.Y * stepPerX).ToString();
            lblLng.Text = (leftBottomCorner.Y + (640 - currentMousePosition.X) * stepPerY).ToString();
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
            try
            {
                //showOriginal = false;
                originalLines.Clear();
                debug.Clear();
                lines.Clear();
                if (polygon.Count < 3)
                {
                    MessageBox.Show("Create correct polygon!");
                    return;
                }

                double angle = Convert.ToInt32(nudAngle.Value);
                lblAngle.Text = angle.ToString();

                translatedPolygon.Clear();
                foreach (var point in polygon)
                {
                    var translatedPoint = Func.GetTranslatedPoint(angle, point);
                    translatedPolygon.Add(translatedPoint);
                }

                var nearestPoint = translatedPolygon[0];
                var farestPoint = translatedPolygon[0];
                var maxX = nearestPoint.X;
                var minX = nearestPoint.X;

                for (int i = 0; i < translatedPolygon.Count; i++)
                {
                    if (translatedPolygon[i].X > maxX)
                    {
                        farestPoint = translatedPolygon[i];
                        maxX = translatedPolygon[i].X;
                    }
                    if (translatedPolygon[i].X < minX)
                    {
                        nearestPoint = translatedPolygon[i];
                        minX = translatedPolygon[i].X;
                    }
                }

                /*var tempPolygon =  new List<Point>();
                foreach (var point in translatedPolygon)
                {
                    var tPoint = Func.GetTranslatedPoint(0, new Point(point.X - nearestPoint.X, point.Y - nearestPoint.Y));
                    tempPolygon.Add(new Point((int)tPoint.X, (int)tPoint.Y));
                }
                translatedPolygon = tempPolygon;*/

                int step = Convert.ToInt32(nudWaterSpread.Value);
                Line topLine = null, bottomLine = null;

                AddDebug(nearestPoint);
                AddDebug(farestPoint);

                if (true)
                {
                    for (int i = nearestPoint.X + step; i < farestPoint.X; i += step)
                    {
                        var intersectingLines = getIntersectingLines(i);
                        if (intersectingLines.Count % 2 != 0)
                        {
                            MessageBox.Show("Something went wrong. Bad shape!");
                        }
                        List<int> ys = new List<int>();
                        for (int k = 1; k < intersectingLines.Count; k += 2)
                        {

                            topLine = intersectingLines[k - 1];
                            bottomLine = intersectingLines[k];

                            //get next vertical line
                            var nY1L = Math.Abs(topLine.P2.Y) > Math.Abs(topLine.P1.Y) ? topLine.P1.Y : topLine.P2.Y;//Math.Abs(topLine.P1.Y - topLine.P2.Y);                    
                            var nY1G = Math.Abs(topLine.P2.Y) > Math.Abs(topLine.P1.Y) ? topLine.P2.Y : topLine.P1.Y;
                            nY1G -= nY1L;

                            var pY1L = Math.Abs(bottomLine.P2.Y) > Math.Abs(bottomLine.P1.Y) ? bottomLine.P1.Y : bottomLine.P2.Y;
                            var pY1G = Math.Abs(bottomLine.P2.Y) > Math.Abs(bottomLine.P1.Y) ? bottomLine.P2.Y : bottomLine.P1.Y;
                            pY1G -= pY1L;

                            var nC = Math.Abs(topLine.P2.Y) > Math.Abs(topLine.P1.Y) ? Math.Abs(i - topLine.P1.X) : Math.Abs(i - topLine.P2.X);
                            var pC = Math.Abs(bottomLine.P2.Y) > Math.Abs(bottomLine.P1.Y) ? Math.Abs(i - bottomLine.P1.X) : Math.Abs(i - bottomLine.P2.X);
                            var y1 = ((nY1G * nC) / (topLine.P2.X - topLine.P1.X)) + nY1L;
                            var y2 = ((pY1G * pC) / (bottomLine.P2.X - bottomLine.P1.X)) + pY1L;//prevNearestPoint.X;
                            ys.Add(y1);
                            ys.Add(y2);
                        }
                        ys.Sort();
                        for (int l = 1; l < ys.Count; l += 2)
                        {
                            lines.Add(new Line(i, ys[l - 1], i, ys[l]));
                        }
                    }

                    foreach(var line in lines)
                    {
                        originalLines.Add(Func.GetTranslatedLine(-angle, line));
                    }

                    /*foreach(var line in originalLines)
                    {
                        
                    }*/
                    if(!drawTimer.Enabled)
                    {
                        drawTimer_Tick(sender, e);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Build impossible. Sorry... =(");
            }
        }

        public List<Line> getIntersectingLines(int x)
        {
            List<Line> lines = new List<Line>();
            Point prevPoint = translatedPolygon[0];
            bool prevSamePointAdded = false;
            for(int i = 1; i<translatedPolygon.Count; i++)
            {
                if(prevPoint.X <= x && translatedPolygon[i].X >= x ||
                    (prevPoint.X >= x && translatedPolygon[i].X <= x))
                {
                    if (!isPointAlreadyAdded(lines, translatedPolygon[i].X, x) && !isPointAlreadyAdded(lines, prevPoint.X, x))
                    {
                        if (prevPoint.X < translatedPolygon[i].X)
                            lines.Add(new Line(prevPoint, translatedPolygon[i]));
                        else
                            lines.Add(new Line(translatedPolygon[i], prevPoint));
                    }
                }
                prevPoint = translatedPolygon[i];
            }
            if ((prevPoint.X <= x && translatedPolygon[0].X >= x) ||
                    (prevPoint.X >= x && translatedPolygon[0].X <= x))
            {
                if (!isPointAlreadyAdded(lines, translatedPolygon[0].X, x) && !isPointAlreadyAdded(lines, prevPoint.X, x))
                {
                    if (prevPoint.X < translatedPolygon[0].X)
                        lines.Add(new Line(prevPoint, translatedPolygon[0]));
                    else
                        lines.Add(new Line(translatedPolygon[0], prevPoint));
                }
            }
            Line l1 = null, l2 = null;
            List<Line> toRemove = new List<Line>();
            int next = 0;
            foreach(var line in lines)
            {
                if(line.P1.X == x || line.P2.X == x)
                {
                    if(next == 0)
                    {
                        l1 = line;
                        next = 1;
                    }
                    else
                    {
                        l2 = line;
                        next = 0;
                        if(isEqualPoint(l1.P1, l2.P2))
                        {
                            if ((l1.P2.X < x && l2.P1.X > x) || (l1.P2.X > x && l2.P1.X < x))
                                toRemove.Add(l2);
                        }
                        else
                        {
                            if ((l1.P1.X < x && l2.P2.X > x) || (l1.P1.X > x && l2.P2.X < x))
                                toRemove.Add(l2);
                        }
                    }
                }

            }
            foreach(var line in lines)
            {
                if (line.P1.X == line.P2.X)
                    toRemove.Add(line);
            }
            foreach (var line in lines)
            {
                if (line.P1.X == x || line.P2.X == x)
                {
                    if (next == 0)
                    {
                        l1 = line;
                        next = 1;
                    }
                    else
                    {
                        l2 = line;
                        next = 0;
                        if (isEqualPoint(l1.P1, l2.P2))
                        {
                            if ((l1.P2.X < x && l2.P1.X > x) || (l1.P2.X > x && l2.P1.X < x))
                                toRemove.Add(l2);
                        }
                        else
                        {
                            if ((l1.P1.X < x && l2.P2.X > x) || (l1.P1.X > x && l2.P2.X < x))
                                toRemove.Add(l2);
                        }
                    }
                }

            }
            if (toRemove.Count > 0)
            {
                foreach(var r in toRemove)
                {
                    lines.Remove(r);
                }
            }
            return lines;
        }

        public bool isEqualPoint(Point p1, Point p2)
        {
            return p1.X == p2.X;
        }

        public bool isPointAlreadyAdded(List<Line> lines, int x, int step)
        {
            foreach(var line in lines)
            {
                if ((line.P1.X == x || line.P2.X == x) && (line.P1.X == step || line.P2.X == step))
                    return false;
            }
            return false;
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
        public void AddDebug(Color color, params PointF[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE, new List<Point>(), color);
            foreach (var pp in p)
                d.AddPoint(new Point((int) pp.X, (int) pp.Y));
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
        bool mousePressed = false;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePressed = true;
            oldMouseX = e.Location.X;
            oldMouseY = e.Location.Y;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePressed = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            drawTimer.Enabled = !drawTimer.Enabled;
            if (drawTimer.Enabled)
            {
                button4.Text = "Stop Timer";
            }
            else
                button4.Text = "Start Timer";
        }

        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            if (nudAngle.Value == -5) nudAngle.Value = 355;
            if (nudAngle.Value == 365) nudAngle.Value = 5;
            markupBtn_Click(sender, e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            showOriginal = checkBox1.Checked;
        }

        private void nudWaterSpread_ValueChanged(object sender, EventArgs e)
        {
            markupBtn_Click(sender, e);
        }

        private void showTemp_CheckedChanged(object sender, EventArgs e)
        {

        }

        double stepPerX = 0;
        double stepPerY = 0;
        PointF leftBottomCorner = new PointF(0, 0);

        private void button5_Click(object sender, EventArgs e)
        {
            map.Latitude = Convert.ToDouble(txtCenterLat.Text);
            map.Longitude = Convert.ToDouble(txtCenterLng.Text);
            googleImage = map.GetImage();
            relativeCenter = prj.FromLatLngToPoint(Convert.ToDouble(txtCenterLat.Text), Convert.ToDouble(txtCenterLng.Text));
            var scale = Math.Pow(2, map.Zoom);
            var pointSW = new PointF((float)(relativeCenter.X - (640 / 2) / scale), (float)(relativeCenter.Y + (640 / 2) / scale));
            AddDebug(Color.Black, pointSW);
            var latLonSW = prj.FromPointToLatLong(pointSW);

            var pointNE = new PointF((float)(relativeCenter.X + (640 / 2) / scale), (float)(relativeCenter.Y - (640 / 2) / scale));
            AddDebug(Color.Red, pointNE);
            var latLonNE = prj.FromPointToLatLong(pointNE);

            lblLat.Text = latLonSW.X.ToString();
            lblLng.Text = latLonSW.Y.ToString();

            var diffX = Math.Abs(latLonSW.X - latLonNE.X);
            var diffY = Math.Abs(latLonSW.Y - latLonNE.Y);
            stepPerX = diffX / 640.0;
            stepPerY = diffY / 640.0;

            leftBottomCorner = latLonSW;
            //MessageBox.Show(diffX + " : " + diffY);
    }

        private void nudZoom_ValueChanged(object sender, EventArgs e)
        {
            var change = map.Zoom - Convert.ToInt32(nudZoom.Value);
            map.Zoom = Convert.ToInt32(nudZoom.Value);
            googleImage = map.GetImage();
            var tPolygon = new List<Point>();
            foreach(var point in translatedPolygon)
            {
                tPolygon.Add(new Point(point.X * change * 2, point.Y * change * 2));
            }
            translatedPolygon = tPolygon;
        }
    }
}