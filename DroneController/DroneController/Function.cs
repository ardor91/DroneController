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
    }
}
