using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class PointD
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public string ToString()
        {
            return "X: " + X + "; Y: " + Y;
        }

        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }
    }
}
