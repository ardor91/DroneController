using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DroneController
{
    public class StaticMap
    {
        private const string API_KEY = "AIzaSyB8hHRgKtsoZeYP9zeDgC-ZeQHXb9iqDRY";
        private const string STATIC_MAP_URL = "https://maps.googleapis.com/maps/api/staticmap?";
        private const string MAP_TYPE = "satellite";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Zoom { get; set; }
        public Size Size { get; set; }
        public int Scale { get; set; }
        public List<GpsPoint> Points { get; set; }
        public List<GpsPoint> Path { get; set; }

        public StaticMap()
        {
            Latitude = 52.453901;
            Longitude = 30.959621;
            Zoom = 16;
            Size = new Size(640, 640);
            Scale = 2;
            Points = new List<GpsPoint>();
            Path = new List<GpsPoint>();
        }

        public Bitmap GetImage()
        {
            string markerks = "";
            string path = "&path=color:0xff0000ff%7Cweight:5";
            int i = 1;
            //path=color:0xff0000ff|weight:5|40.737102,-73.990318|40.749825,-73.987963
            foreach (var point in Points)
            {
                markerks += "&markers=color:blue%7Clabel:" + (i++) + "%7C" + point.Latitude + "," + point.Longitude;
            }
            foreach(var point in Path)
            {
                path += "%7C" + point.Latitude + "," + point.Longitude;
            }
            string textUri = STATIC_MAP_URL +
                "center=" + Latitude + "," + Longitude + "&" +
                "zoom=" + Zoom + "&" +
                "size=" + Size.Width + "x" + Size.Height + "&" +
                "maptype=" + MAP_TYPE;

            if (Points.Count > 0)
            {
                textUri += markerks;
            }
            if (Path.Count > 0)
            {
                textUri += path;
            }

            textUri += "&key=" + API_KEY;

            Uri uri = new Uri(textUri);

            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Stream imageStream = httpResponse.GetResponseStream();
            Bitmap staticMap = new Bitmap(imageStream);
            httpResponse.Close();
            imageStream.Close();

            return staticMap;
        }
        
    }
}
