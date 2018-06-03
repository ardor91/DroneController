using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class MercatorProjection
    {
        private const int MERCATOR_RANGE = 256;
        private Point PixelOrigin { get; set; }
        private double PixelsPerLonDegree { get; set; } 
        private double PixelsPerLonRadian { get; set; }

        public double Bound(double value, double opt_min, double opt_max)
        {
            value = Math.Max(value, opt_min);
            value = Math.Min(value, opt_max);
            return value;
        }

        public double DegreesToRadians(double degree)
        {
            return degree * (Math.PI / 180);
        }
        public double RadiansToDegrees(double radian)
        {
            return radian / (Math.PI / 180);
        }

        public MercatorProjection()
        {
            PixelOrigin = new Point(MERCATOR_RANGE / 2, MERCATOR_RANGE / 2);
            PixelsPerLonDegree = MERCATOR_RANGE / 360.0;
            PixelsPerLonRadian = MERCATOR_RANGE / (2 * Math.PI);
        }

        public PointF FromLatLngToPoint(double latitude, double longitude)
        {
            var x = PixelOrigin.X + longitude * PixelsPerLonDegree;
            var siny = Bound(Math.Sin(DegreesToRadians(latitude)), -0.9999, 0.9999);
            var y = PixelOrigin.Y + 0.5 * Math.Log((1 + siny) / (1 - siny)) * -PixelsPerLonRadian;
            return new PointF((float)x, (float)y);
        }

        public PointF FromPointToLatLong(PointF optPoint)
        {
            var longitude = (optPoint.X - PixelOrigin.X) / PixelsPerLonDegree;
            var latRadians = (optPoint.Y - PixelOrigin.Y) / -PixelsPerLonRadian;
            var latitude = RadiansToDegrees(2 * Math.Atan(Math.Exp(latRadians)) - Math.PI / 2);
            
            return new PointF((float)latitude, (float)longitude);
        }
    }
}
