using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DroneController
{
    public class Triangle
    {
        public Point p1 { get; set; }
        public Point p2 { get; set; }
        public Point p3 { get; set; }

        public Triangle(Point p1, Point p2, Point p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public void DrawObject(Graphics g, Pen p)
        {
            g.DrawLine(p, p1, p2);
            g.DrawLine(p, p2, p3);
            g.DrawLine(p, p3, p1);
        }
    }
}
