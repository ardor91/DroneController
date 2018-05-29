using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class Debug
    {
        public ObjectType ObjectType { get; set; }
        public List<Point> Points { get; set; }

        public Color ObjectColor { get; set; }

        public Debug(ObjectType type, List<Point> points, Color color)
        {
            ObjectType = type;
            Points = points;
            ObjectColor = color;
        }
        public Debug(ObjectType type, List<Point> points)
        {
            ObjectType = type;
            Points = points;
            ObjectColor = Color.Red;
        }

        public Debug(ObjectType type)
        {
            ObjectType = type;
            Points = new List<Point>();
            ObjectColor = Color.Red;
        }

        public void AddPoint(Point p)
        {
            Points.Add(p);
        }

        public void DrawObject(Graphics g, Pen p)
        {
            p.Color = ObjectColor;
            switch(ObjectType)
            {
                case ObjectType.POINT:
                    {
                        g.FillRectangle(new SolidBrush(p.Color), new Rectangle(Points[0], new Size(5,5)));
                        break;
                    }
                case ObjectType.LINE:
                    {
                        g.DrawLine(p, Points[0], Points[1]);
                        break;
                    }
                case ObjectType.RECTANGLE:
                    {
                        for(int i=1; i<Points.Count; i++)
                        {
                            g.DrawLine(p, Points[i - 1], Points[i]);
                        }
                        g.DrawLine(p, Points[Points.Count - 1], Points[0]);
                        break;
                    }
            }
        }
    }
}
