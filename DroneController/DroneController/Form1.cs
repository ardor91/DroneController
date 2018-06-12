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
            if (!REDRAW_NEEDED) return;
            drawArea.Clear(Color.Black);
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
            REDRAW_NEEDED = false;
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

                originalLines.AddRange(Func.GetPathLinesFromPolygon(polygon, angle, step));
                originalLines.AddRange(Func.GetPathLinesFromPolygon(polygon, angle + 90, step));

                REDRAW_NEEDED = true;

                if (!drawTimer.Enabled)
                {
                    drawTimer_Tick(sender, e);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Build impossible. Sorry... =(");
            }
        }

        #region Debug
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
                d.AddPoint(new Point((int) pp.X, (int) pp.Y));
            debug.Add(d);
        }

        #endregion

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
            
            map.Points.AddRange(new List<GpsPoint>() { latLonSW, latLonNE});

            if(originalLines.Count > 0)
            {
                var order = true;
                foreach(var line in originalLines)
                {
                    var p1 = new GpsPoint((leftBottomCorner.Latitude + (640 - (line.P1.Y + offsetY)) * stepPerY),
                                          (leftBottomCorner.Longitude + ( (line.P1.X + offsetX)) * stepPerX));
                    var p2 = new GpsPoint((leftBottomCorner.Latitude + (640 - (line.P2.Y + offsetY)) * stepPerY),
                                          (leftBottomCorner.Longitude + ( (line.P2.X + offsetX)) * stepPerX));

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
            foreach(var point in translatedPolygon)
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
            int step = 50;
            int angle = 0;

            List<Line> horizontallines = Func.GetPathLinesFromPolygon(polygon, angle, step, false);
            List<Line> verticallines = Func.GetPathLinesFromPolygon(polygon, angle + 90, step, false);
            List<Point> translatedPolygon = new List<Point>();
            foreach (var point in polygon)
            {
                var translatedPoint = Func.GetTranslatedPoint(angle, point);
                translatedPolygon.Add(translatedPoint);
            }

            var mode = true;
            if (horizontallines[0].P1.Y == horizontallines[0].P2.Y)
                mode = false;
            else
                mode = true;

            foreach (var hline in mode ? horizontallines : verticallines)
            {
                foreach(var vline in mode ? verticallines : horizontallines)
                {
                    

                }
            }
        }
    }
}