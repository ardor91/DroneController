using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class Function
    {
        public double GetDistance(Point p1, Point p2)
        {
            return Math.Abs(Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2))));
        }
        public PointF GetTranslatedPointF(double angle, Point p)
        {
            var newPoint = new PointF();
            var tAngle = angle * (Math.PI / 180);
            newPoint.X = (float)(p.X * Math.Cos(tAngle) - p.Y * Math.Sin(tAngle));
            newPoint.Y = (float)(p.X * Math.Sin(tAngle) + p.Y * Math.Cos(tAngle));

            return newPoint;
        }
        public Point GetTranslatedPoint(double angle, Point p)
        {
            var newPoint = new Point();
            var tAngle = angle * (Math.PI / 180);
            newPoint.X = (int)(p.X * Math.Cos(tAngle) - p.Y * Math.Sin(tAngle));
            newPoint.Y = (int)(p.X * Math.Sin(tAngle) + p.Y * Math.Cos(tAngle));

            return newPoint;
        }

        public Point GetTranslatedPoint(double angle, Point offset, Point p)
        {
            p.X -= offset.X;
            p.Y -= offset.Y;
            var newPoint = new Point();
            var tAngle = angle * (Math.PI / 180);
            newPoint.X = (int)(p.X * Math.Cos(tAngle) - p.Y * Math.Sin(tAngle));
            newPoint.Y = (int)(p.X * Math.Sin(tAngle) + p.Y * Math.Cos(tAngle));

            return newPoint;
        }

        public Line GetTranslatedLine(double angle, Line l)
        {
            var tAngle = angle * (Math.PI / 180);
            var p1 = new Point((int)(l.P1.X * Math.Cos(tAngle) - l.P1.Y * Math.Sin(tAngle)), (int)(l.P1.X * Math.Sin(tAngle) + l.P1.Y * Math.Cos(tAngle)));
            var p2 = new Point((int)(l.P2.X * Math.Cos(tAngle) - l.P2.Y * Math.Sin(tAngle)), (int)(l.P2.X * Math.Sin(tAngle) + l.P2.Y * Math.Cos(tAngle)));

            return new Line(p1, p2);
        }
        public Line GetTranslatedLine(double angle, Point offset, Line l)
        {
            l.P1 = new Point(l.P1.X - offset.X, l.P1.Y - offset.Y);
            l.P2 = new Point(l.P2.X - offset.X, l.P2.Y - offset.Y);

            var tAngle = angle * (Math.PI / 180);
            var p1 = new Point((int)(l.P1.X * Math.Cos(tAngle) - l.P1.Y * Math.Sin(tAngle)), (int)(l.P1.X * Math.Sin(tAngle) + l.P1.Y * Math.Cos(tAngle)));
            var p2 = new Point((int)(l.P2.X * Math.Cos(tAngle) - l.P2.Y * Math.Sin(tAngle)), (int)(l.P2.X * Math.Sin(tAngle) + l.P2.Y * Math.Cos(tAngle)));

            return new Line(p1, p2);
        }
        public double GetAngle(Point p0, Point p1, Point p2)
        {
            /*var katet = Math.Abs(p2.X - p0.X);
            var katet2 = Math.Abs(p1.Y - p0.Y);
            var gipo = Math.Sqrt(Math.Pow(katet, 2) + Math.Pow(katet2, 2));

            return Math.Acos(katet / gipo) * (180 / Math.PI);*/

            var skalar = (p1.X - p0.X) * (p2.X - p0.X) + (p1.Y - p0.Y) * (p2.Y - p0.Y);
            var length1 = GetDistance(p0, p1);
            var length2 = GetDistance(p0, p2);

            return Math.Acos(skalar / (length1 * length2)) * (180 / Math.PI);
        }
        public bool IsInsideTriangle(Point point, Point p1, Point p2, Point p3)
        {
            bool b1, b2, b3;

            b1 = Sign(point, p1, p2) < 0.0f;
            b2 = Sign(point, p2, p3) < 0.0f;
            b3 = Sign(point, p3, p1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }
        public bool IsGoodTriangle(Point p1, Point p2, Point p3, int angleThreshold, List<Point> polygon)
        {
            var p1p2 = GetDistance(p1, p2);
            var p1p3 = GetDistance(p1, p3);
            var p2p3 = GetDistance(p2, p3);

            if (p1p2 + p1p3 <= p2p3) return false;
            if (p1p2 + p2p3 <= p1p3) return false;
            if (p2p3 + p1p3 <= p1p2) return false;

            var p1angle = GetAngle(p1, p2, p3);
            var p2angle = GetAngle(p2, p1, p3);
            var p3angle = GetAngle(p3, p2, p1);

            if (p1angle < angleThreshold) return false;
            if (p2angle < angleThreshold) return false;
            if (p3angle < angleThreshold) return false;

            var newPoint = new Point((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);
            var zeroPoint = new Point(0, 0);
            int intersectionsCount = 0;
            for (int i = 1; i < polygon.Count; i++)
            {
                if (IsIntersecting(newPoint, zeroPoint, polygon[i - 1], polygon[i]))
                {
                    intersectionsCount++;
                }
            }

            if (intersectionsCount % 2 == 0) return false;

            //MessageBox.Show((p1angle + p2angle + p3angle).ToString());

            return true;
        }
        private float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
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
        public double CalculateSquare(List<Point> polygon)
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
            return square;
            //lblSquare.Text = square.ToString();
        }

        public List<Line> GetPathLinesFromPolygon(List<Point> polygon, int angle, int step, bool translateBack = true)
        {
            List<Line> lines = new List<Line>();
            List<Line> result = new List<Line>();
            List<Point> translatedPolygon = new List<Point>();
            if (polygon.Count < 3)
            {
                return null;
            }

            foreach (var point in polygon)
            {
                var translatedPoint = GetTranslatedPoint(angle, point);
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

            Line topLine = null, bottomLine = null;

            for (int i = nearestPoint.X + step; i < farestPoint.X; i += step)
            {
                var intersectingLines = getIntersectingLines(translatedPolygon, i);
                if (intersectingLines.Count % 2 != 0)
                {
                    //MessageBox.Show("Something went wrong. Bad shape!");
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
            if (translateBack)
            {
                foreach (var line in lines)
                {
                    result.Add(GetTranslatedLine(-angle, line));
                }
                return result;
            }
            return lines;
        }

        public List<Line> getIntersectingLines(List<Point> polygon, int x)
        {
            List<Line> lines = new List<Line>();
            Point prevPoint = polygon[0];
            for (int i = 1; i < polygon.Count; i++)
            {
                if (prevPoint.X <= x && polygon[i].X >= x ||
                    (prevPoint.X >= x && polygon[i].X <= x))
                {
                    if (!isPointAlreadyAdded(lines, polygon[i].X, x) && !isPointAlreadyAdded(lines, prevPoint.X, x))
                    {
                        if (prevPoint.X < polygon[i].X)
                            lines.Add(new Line(prevPoint, polygon[i]));
                        else
                            lines.Add(new Line(polygon[i], prevPoint));
                    }
                }
                prevPoint = polygon[i];
            }
            if ((prevPoint.X <= x && polygon[0].X >= x) ||
                    (prevPoint.X >= x && polygon[0].X <= x))
            {
                if (!isPointAlreadyAdded(lines, polygon[0].X, x) && !isPointAlreadyAdded(lines, prevPoint.X, x))
                {
                    if (prevPoint.X < polygon[0].X)
                        lines.Add(new Line(prevPoint, polygon[0]));
                    else
                        lines.Add(new Line(polygon[0], prevPoint));
                }
            }
            Line l1 = null, l2 = null;
            List<Line> toRemove = new List<Line>();
            int next = 0;
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
            foreach (var line in lines)
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
                foreach (var r in toRemove)
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
            foreach (var line in lines)
            {
                if ((line.P1.X == x || line.P2.X == x) && (line.P1.X == step || line.P2.X == step))
                    return false;
            }
            return false;
        }
    }
}
