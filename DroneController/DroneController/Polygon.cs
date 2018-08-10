using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class Polygon
    {
        public List<Point> Points { get; set; }
        public List<Line> Path { get; set; }
        public int Angle { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }

        public Polygon(List<Point> points)
        {
            Points = points;
        }

        public Polygon()
        {
            Points = new List<Point>();
        }
        public Polygon(params Point[] point)
        {
            Points = point.ToList();
        }

        public Polygon(List<Point> points, params Point[] point)
        {
            Points = points;
            Points.AddRange(point.ToList());
        }

        public void Draw(Graphics obj, Color color, int size)
        {
            if (Points.Count < 3) return;
            var ppoint = Points[0];
            for (int i = 1; i < Points.Count; i++)
            {
                obj.DrawLine(new Pen(color, size), ppoint, Points[i]);
                ppoint = Points[i];
            }
            obj.DrawLine(new Pen(color, size), Points[Points.Count - 1], Points[0]);
        }
    }
}
