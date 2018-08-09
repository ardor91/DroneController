using DroneController.Helpers;
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
        public List<Polygon> polygons;
        public List<Point> translatedPolygon;
        public List<Polygon> sectors;
        public Point startPoint;
        public Polygon currentSector;
        public bool isDrawing = false;
        public bool isPickingStart = false;
        public bool isSectorDrawing = false;
        public bool isWrongPosition = false;
        public bool isWrongNearestPosition = false;
        public Point nearestIntersection;
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
        public bool REDRAW_NEEDED = true;

        bool showOriginal = true;
        int offsetX = 0;
        int offsetY = 0;

        StaticMap map = new StaticMap();
        MercatorProjection prj = new MercatorProjection();
        PointD relativeCenter = new PointD(0, 0);

        public Form1()
        {
            InitializeComponent();
            polygon = new List<Point>();
            translatedPolygon = new List<Point>();
            debug = new List<Debug>();
            triangles = new List<Triangle>();
            lines = new List<Line>();
            originalLines = new List<Line>();
            polygons = new List<Polygon>();
            sectors = new List<Polygon>();
            currentSector = new Polygon();
            nearestIntersection = new Point();

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

        public void drawCircle(Color color, int radius, bool isDashed, Point center)
        {
            pen = new Pen(color, 1);
            if (isDashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            drawArea.DrawEllipse(pen, new Rectangle(center.X - radius / 2, center.Y - radius / 2, radius, radius));
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
            if (!REDRAW_NEEDED) return;
            drawArea.Clear(Color.Black);
            //draw paths

            //draw polygon
            if (googleImage != null)
            //panel1.BackgroundImage = googleImage;
            {
                drawArea.DrawImage(googleImage, -panel1.Width / 4, -panel1.Height / 2);
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
            if(currentSector != null && isSectorDrawing && currentSector.Points.Count > 0)
            {
                var prev = currentSector.Points[0];
                for (int i = 1; i < currentSector.Points.Count; i++)
                {
                    drawLine(Color.Orange, 3, false, prev, currentSector.Points[i]);
                    prev = currentSector.Points[i];
                }
                var ccolor = isWrongPosition ? Color.Red : Color.GreenYellow;
                drawLine(ccolor, 2, false, prev, currentMousePosition);
                prev = currentMousePosition;
                drawLine(ccolor, 2, false, prev, currentSector.Points[0]);
                
                //currentSector.Draw(drawArea, Color.Orange, 3);
            }
            if(isSectorDrawing)
            {
                if (nearestIntersection != null && nearestIntersection != Point.Empty)
                {
                    SolidBrush brush = isWrongNearestPosition ? new SolidBrush(Color.Red) : new SolidBrush(Color.Blue);
                    drawArea.FillRectangle(brush, new Rectangle(nearestIntersection.X - 3, nearestIntersection.Y - 3, 6, 6));
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
                {
                    drawRectangle(Color.Red, 1, true, currentMousePosition, new Point(10, 10));
                    drawCircle(Color.Red, Convert.ToInt32(nudRad.Value), true, currentMousePosition);
                }
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

            //draw polygons
            foreach (var poly in sectors)
            {
                poly.Draw(drawArea, Color.Green, 2);
            }
            //if(currentSector != null)
                

            if (startPoint != null)
            {
                drawCircle(Color.Red, Convert.ToInt32(nudRad.Value), true, startPoint);
            }
            REDRAW_NEEDED = false;
        }

        public void Info(Object text)
        {
            log.Text = text.ToString() + "\n";
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
            REDRAW_NEEDED = true;
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
            if (isSectorDrawing)
            {
                if (e.Button == MouseButtons.Left && !isWrongPosition)
                {
                    if (nearestIntersection != null && nearestIntersection != Point.Empty && !isWrongNearestPosition)
                    {
                        currentSector.Points.Add(nearestIntersection);
                    }
                    else
                        currentSector.Points.Add(new Point(e.Location.X - offsetX, e.Location.Y - offsetY));
                }
                if (e.Button == MouseButtons.Right)
                {
                    isSectorDrawing = false;
                    sectors.Add(currentSector);
                    currentSector = null;
                }
            }
            if (isPickingStart)
            {
                startPoint = new Point(e.Location.X - offsetX, e.Location.Y - offsetY);
                isPickingStart = false;
            }
            var longitude = (leftBottomCorner.Longitude + e.Location.X * stepPerX);
            var latitude = (leftBottomCorner.Latitude + (640 - e.Location.Y) * stepPerY);
            map.Points.Add(new GpsPoint(latitude, longitude));
            REDRAW_NEEDED = true;
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
                e.Location.X - offsetX,
                e.Location.Y - offsetY);
            lblCursorX.Text = currentMousePosition.X + "";
            lblCursorY.Text = currentMousePosition.Y + "";
            // x - latitude
            // y - longitude
            lblLat.Text = (leftBottomCorner.Latitude + (640 - e.Location.Y) * stepPerY).ToString();
            lblLng.Text = (leftBottomCorner.Longitude + (e.Location.X) * stepPerX).ToString();
            label16.Text = "IS In Polygon: " + IsInPolygon3(polygon.ToArray(), currentMousePosition);
            if (isSectorDrawing)
            {
                label15.Text = startPoint.X + ";" + startPoint.Y + "------" + currentMousePosition.X + ";" + currentMousePosition.Y;
                isWrongPosition = false;
                isWrongNearestPosition = false;
                //(x - center_x)^2 + (y - center_y)^2 <= radius^2
                if (Math.Pow((currentMousePosition.X - startPoint.X), 2) + Math.Pow((currentMousePosition.Y - startPoint.Y), 2) > Math.Pow(Convert.ToInt32(nudRad.Value) / 2, 2))
                {
                    isWrongPosition = true;                    
                }
                if (!IsInPolygon3(polygon.ToArray(), currentMousePosition))
                {
                    isWrongPosition = true;
                }

                nearestIntersection = getNearestToLinePoint(currentMousePosition, polygon, int.MaxValue, int.MaxValue, 40);
                if (Math.Pow((nearestIntersection.X - startPoint.X), 2) + Math.Pow((nearestIntersection.Y - startPoint.Y), 2) > Math.Pow(Convert.ToInt32(nudRad.Value) / 2, 2))
                {
                    isWrongNearestPosition = true;
                }
                
            }

            REDRAW_NEEDED = true;
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

        public static bool IsInPolygon(Point[] poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }

        public static bool IsInPolygon2(Point[] polygon, Point testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        private bool IsInPolygon3(Point[] polygon, Point point)
        {
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }

        public Point getNearestToLinePoint(Point mousePosition, List<Point> polygon, int boundX, int boundY, int lambda)
        {
            var minDistance = double.MaxValue;
            foreach (var point in polygon)
            {
                var distance = getDistance(point, mousePosition);
                if (distance <= lambda && distance < minDistance)
                {
                    minDistance = distance;
                    return point;

                }
            }
            Point prevPoint = polygon[0];

            
            var minPoint = new Point();
            string text = "";
            var array = polygon.Skip(1);
            
            for (int i = 0; i<=polygon.Count; i++)
            {
                Point p = new Point();
                if (i == polygon.Count)
                    p = polygon[0];
                else
                    p = polygon[i];
                var line = new Line(prevPoint, p);
                text += line.P1.X + ":" + line.P1.Y + ";" + line.P2.X + ":" + line.P2.Y + "\n";
                var hLine = new Line(new Point(mousePosition.X, 0), new Point(mousePosition.X, boundY));
                var vLine = new Line(new Point(0, mousePosition.Y), new Point(boundX, mousePosition.Y));

                var intersectionX = new PointD(0, 0);
                var intersectionY = new PointD(0, 0);

                var intersectsX = Function.LineSegmentsIntersect(line, vLine, out intersectionX);
                var intersectsY = Function.LineSegmentsIntersect(line, hLine, out intersectionY);

                text += "MOUSE - " + mousePosition.X + ":" + mousePosition.Y + "\n";
                if (intersectsX)
                {
                    text += "Intersects X - "+ intersectionX.X + ":" + intersectionX.Y + "\n";
                    var t = Math.Abs(intersectionX.ToPoint().X - mousePosition.X);
                    text += "Distance - " + t + "\n";
                    if (t < minDistance)
                    {
                        minDistance = t;
                        minPoint = intersectionX.ToPoint();
                    }                    
                }

                if(intersectsY)
                {
                    text += "Intersects Y - " + intersectionY.X + ":" + intersectionY.Y + "\n";
                    var t = Math.Abs(intersectionY.ToPoint().Y - mousePosition.Y);
                    text += "Distance - " + t + "\n";
                    if (t < minDistance)
                    {
                        minDistance = t;
                        minPoint = intersectionY.ToPoint();
                    }
                }
                prevPoint = p;
            }
            text += "MinDistance: " + minDistance;
            Info(text);
            if (minDistance <= lambda)
            {
                return minPoint;
            }
            else
                return Point.Empty;
        }

        private double getDistance(Point p1, Point p2)
        {
            return Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
        }

        private void markupBtn_Click(object sender, EventArgs e)
        {
            try
            {
                originalLines.Clear();
                debug.Clear();
                lines.Clear();

                int angle = Convert.ToInt32(nudAngle.Value);
                lblAngle.Text = angle.ToString();

                int step = Convert.ToInt32(nudWaterSpread.Value);

                if(checkBox2.Checked || sectors.Count == 0)
                    originalLines.AddRange(Func.GetPathLinesFromPolygon(polygon, angle, step));
                else
                {
                    foreach(var polygon in sectors)
                    {
                        originalLines.AddRange(Func.GetPathLinesFromPolygon(polygon.Points, angle, step));
                    }
                }
                //originalLines.AddRange(Func.GetPathLinesFromPolygon(polygon, angle + 90, step));

                REDRAW_NEEDED = true;

                if (!drawTimer.Enabled)
                {
                    drawTimer_Tick(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Build impossible. Sorry... =(");
            }
        }

        #region Debug
        public void AddDebug(params Point[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE);
            foreach (var pp in p)
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
        public void AddDebug(params PointD[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE);
            foreach (var pp in p)
                d.AddPoint(pp.ToPoint());
            debug.Add(d);
        }
        public void AddDebug(Color color, params PointD[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE, new List<Point>(), color);
            foreach (var pp in p)
                d.AddPoint(pp.ToPoint());
            debug.Add(d);
        }
        public void AddDebug(Color color, params PointF[] p)
        {
            var d = new Debug(p.Length == 1 ? ObjectType.POINT : p.Length == 2 ? ObjectType.LINE : ObjectType.RECTANGLE, new List<Point>(), color);
            foreach (var pp in p)
                d.AddPoint(new Point((int)pp.X, (int)pp.Y));
            debug.Add(d);
        }

        #endregion

        private void markupBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            polygon.Clear();
            polygon.Add(new Point(70, -8));
            polygon.Add(new Point(147, -96));
            polygon.Add(new Point(224, 4));
            polygon.Add(new Point(134, 73));
            startPoint = new Point(-40, 10);
        }

        public void triangulationMarkup(int threshold)
        {
            var flag = true;
            int currentIndex = 0;
            while (flag)
            {
                for (int i = 1; i < polygon.Count; i++)
                {
                    SplitLine(i - 1, i, threshold);
                }
                SplitLine(polygon.Count - 1, 0, threshold);

                flag = false;
                for (int i = 1; i < polygon.Count; i++)
                {
                    if (Func.GetDistance(polygon[i - 1], polygon[i]) > threshold)
                    {
                        flag = true;
                        break;
                    }
                }
                if (Func.GetDistance(polygon[polygon.Count - 1], polygon[0]) > threshold)
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

            if (distance > threshold)
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
            originalLines.Clear();
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
            REDRAW_NEEDED = true;
            showOriginal = checkBox1.Checked;
        }

        private void nudWaterSpread_ValueChanged(object sender, EventArgs e)
        {
            markupBtn_Click(sender, e);
        }

        private void showTemp_CheckedChanged(object sender, EventArgs e)
        {
            REDRAW_NEEDED = true;
        }

        double stepPerX = 0;
        double stepPerY = 0;
        GpsPoint leftBottomCorner = new GpsPoint(0, 0);

        private void button5_Click(object sender, EventArgs e)
        {
            //map.Points.Clear();
            map.Latitude = Convert.ToDouble(txtCenterLat.Text);
            map.Longitude = Convert.ToDouble(txtCenterLng.Text);

            relativeCenter = prj.FromLatLngToPoint(new GpsPoint(Convert.ToDouble(txtCenterLat.Text), Convert.ToDouble(txtCenterLng.Text)));
            var scale = Math.Pow(2, map.Zoom);
            var pointSW = new PointD((relativeCenter.X - (640.0 / 2.0) / scale), (relativeCenter.Y + (640.0 / 2.0) / scale));
            AddDebug(Color.Black, pointSW);
            var latLonSW = prj.FromPointToLatLong(pointSW);

            var pointNE = new PointD((relativeCenter.X + (640.0 / 2) / scale), (relativeCenter.Y - (640.0 / 2) / scale));
            AddDebug(Color.Red, pointNE);
            var latLonNE = prj.FromPointToLatLong(pointNE);

            lblLat.Text = latLonSW.Latitude.ToString();
            lblLng.Text = latLonSW.Longitude.ToString();

            var diffY = Math.Abs(latLonSW.Latitude - latLonNE.Latitude); // latitude
            var diffX = Math.Abs(latLonSW.Longitude - latLonNE.Longitude); // longitude
            stepPerX = diffX / 640.0;
            stepPerY = diffY / 640.0;

            leftBottomCorner = latLonSW;

            map.Points.AddRange(new List<GpsPoint>() { latLonSW, latLonNE });

            if (originalLines.Count > 0)
            {
                var order = true;
                foreach (var line in originalLines)
                {
                    var p1 = new GpsPoint((leftBottomCorner.Latitude + (640 - (line.P1.Y + offsetY)) * stepPerY),
                                          (leftBottomCorner.Longitude + ((line.P1.X + offsetX)) * stepPerX));
                    var p2 = new GpsPoint((leftBottomCorner.Latitude + (640 - (line.P2.Y + offsetY)) * stepPerY),
                                          (leftBottomCorner.Longitude + ((line.P2.X + offsetX)) * stepPerX));

                    /*var longitude = (leftBottomCorner.Longitude + e.Location.X * stepPerX);
                        var latitude = (leftBottomCorner.Latitude + (640 - e.Location.Y) * stepPerY);*/

                    if (order)
                    {
                        map.Path.Add(p1);
                        map.Path.Add(p2);
                    }
                    else
                    {
                        map.Path.Add(p2);
                        map.Path.Add(p1);
                    }
                    order = !order;
                }
            }

            googleImage = map.GetImage();
            REDRAW_NEEDED = true;
            //MessageBox.Show(diffX + " : " + diffY);
        }

        private void nudZoom_ValueChanged(object sender, EventArgs e)
        {
            var change = map.Zoom - Convert.ToInt32(nudZoom.Value);
            map.Zoom = Convert.ToInt32(nudZoom.Value);
            googleImage = map.GetImage();
            var tPolygon = new List<Point>();
            foreach (var point in translatedPolygon)
            {
                tPolygon.Add(new Point(point.X * change * 2, point.Y * change * 2));
            }
            translatedPolygon = tPolygon;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            map.Points.Clear();
            map.Path.Clear();
            REDRAW_NEEDED = true;
        }

        private void splitToPolygons_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            int step = Convert.ToInt32(nudWaterSpread.Value);
            int angle = 0;
            //List<Polygon> polygons = new List<Polygon>();
            List<Line> horizontallines = Func.GetPathLinesFromPolygon(polygon, angle, step, false);
            List<Line> verticallines = Func.GetPathLinesFromPolygon(polygon, angle + 90, step, false);
            horizontallines.Reverse();
            verticallines.Reverse();
            List<Point> translatedPolygon = new List<Point>();
            foreach (var point in polygon)
            {
                var translatedPoint = Func.GetTranslatedPoint(angle, point);
                translatedPolygon.Add(translatedPoint);
            }

            var mode = true;
            if (horizontallines.Count > 0)
            {
                if (horizontallines[0].P1.Y == horizontallines[0].P2.Y)
                    mode = false;
                else
                    mode = true;
            }
            Line prevHLine = null, prevVLine = null;

            foreach (var hline in mode ? horizontallines : verticallines)
            {
                foreach (var vline in mode ? verticallines : horizontallines)
                {
                    var tVline = Func.GetTranslatedLine(-90, vline);
                    if (prevHLine == null)
                    {
                        //if (vline.P1.X >= hline.P1.X && vline.P2.X >= hline.P1.X) continue;
                        if (prevVLine == null)
                        {
                            var intersection = new PointD(0, 0);
                            var firstPointIndex = -1;
                            var lastPointIndex = -1;
                            if (Function.LineSegmentsIntersect(hline, tVline, out intersection))
                            {
                                var prevPoint = translatedPolygon[0];
                                var pIntH = new PointD(0, 0);
                                var pIntV = new PointD(0, 0);
                                var intersectingH = new PointD(0, 0);
                                var intersectingV = new PointD(0, 0);
                                for (int i = 1; i < translatedPolygon.Count; i++)
                                {
                                    if (Function.LineSegmentsIntersect(new Line(prevPoint, translatedPolygon[i]), hline, out pIntH))
                                    {
                                        if (pIntH.Y < intersection.Y)
                                        {
                                            firstPointIndex = i - 1;
                                            intersectingH = pIntH;
                                        }
                                    }
                                    if (Function.LineSegmentsIntersect(new Line(prevPoint, translatedPolygon[i]), tVline, out pIntV))
                                    {
                                        if (pIntV.X < intersection.X)
                                        {
                                            lastPointIndex = i - 1;
                                            intersectingV = pIntV;
                                        }
                                    }
                                    prevPoint = translatedPolygon[i];
                                }

                                if (firstPointIndex != -1 && lastPointIndex != -1)
                                {
                                    if (firstPointIndex == lastPointIndex)
                                    {
                                        polygons.Add(new Polygon(intersection.ToPoint(), pIntH.ToPoint(), pIntV.ToPoint()));
                                    }
                                    else
                                    {
                                        var points = new List<Point>();
                                        int start = firstPointIndex > lastPointIndex ? lastPointIndex : firstPointIndex;
                                        int end = firstPointIndex < lastPointIndex ? lastPointIndex : firstPointIndex;
                                        for (int i = start; i < end; i++)
                                        {
                                            points.Add(translatedPolygon[i + 1]);
                                        }
                                        polygons.Add(new Polygon(points, intersectingH.ToPoint(), intersection.ToPoint(), intersectingV.ToPoint()));
                                    }
                                }
                            }
                            else
                            {
                                var polygon = new Polygon();
                                var prevPoint = translatedPolygon[0];
                                var pIntH = new PointD(0, 0);
                                var start = -1;
                                var end = -1;
                                for (int i = 1; i < translatedPolygon.Count; i++)
                                {
                                    if (Function.LineSegmentsIntersect(new Line(prevPoint, translatedPolygon[i]), tVline, out pIntH))
                                    {
                                        polygon.Points.Add(pIntH.ToPoint());
                                        if (start == -1)
                                            start = i - 1;
                                        else
                                            end = i - 1;
                                    }
                                    prevPoint = translatedPolygon[i];
                                }
                                var points = new List<Point>();
                                int startInd = start > end ? end : start;
                                int endInd = start < end ? end : start;
                                bool flag = false;
                                for (int i = startInd; i < endInd; i++)
                                {
                                    if (i + 1 >= translatedPolygon.Count)
                                    {
                                        i = -1;
                                    }
                                    if (translatedPolygon[i + 1].Y > tVline.P1.Y)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    points.Add(translatedPolygon[i + 1]);
                                }
                                if (flag)
                                {
                                    for (int i = startInd; i >= endInd; i--)
                                    {
                                        if (i - 1 < 0)
                                        {
                                            i = translatedPolygon.Count;
                                        }
                                        if (translatedPolygon[i - 1].Y > tVline.P1.Y)
                                        {
                                            flag = true;
                                            break;
                                        }
                                        points.Add(translatedPolygon[i - 1]);

                                    }
                                }
                                polygon.Points.AddRange(points);
                                polygons.Add(polygon);
                                continue;
                            }
                        }
                        else
                        {
                            //there is a prev V line
                            var li = prevVLine;
                        }

                    }
                    else
                    {
                        //there is a prev H line
                    }
                    prevVLine = tVline;

                }
                prevHLine = hline;
                prevVLine = null;
            }

            //case where there are no vertical lines

            //TODO


            //case where there are no horizontal lines

            //TODO

            REDRAW_NEEDED = true;
            return;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            string[] numbers = text.Split(',');
            List<int> nbr = new List<int>();
            foreach (var n in numbers)
                nbr.Add(Convert.ToInt32(n));
            if (nbr.Count != 8) return;
            Vector intersection = new Vector();
            if (Function.LineSegmentsIntersect(new Vector(nbr[0], nbr[1]), new Vector(nbr[2], nbr[3]), new Vector(nbr[4], nbr[5]), new Vector(nbr[6], nbr[7]), out intersection))
            {
                MessageBox.Show("Intersects!");
                textBox2.Text = "X: " + intersection.X + "; Y: " + intersection.Y;
            }
            else
                MessageBox.Show("No intersection =(");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (polygon == null || polygon.Count <= 2)
            {
                MessageBox.Show("Create a valid Polygon first!");
                return;
            }
            if (isSectorDrawing)
            {
                isSectorDrawing = false;
                sectors.Add(currentSector);
                currentSector = null;
            }
            else
            {
                isSectorDrawing = true;
                currentSector = new Polygon();
            }
        }
    }
}