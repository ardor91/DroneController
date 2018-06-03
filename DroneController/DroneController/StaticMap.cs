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

        public StaticMap()
        {
            Latitude = 52.453901;
            Longitude = 30.959621;
            Zoom = 16;
            Size = new Size(640, 640);
            Scale = 2;
        }

        public Bitmap GetImage()
        {
            string textUri = STATIC_MAP_URL +
                "center=" + Latitude + "," + Longitude + "&" +
                "zoom=" + Zoom + "&" +
                "size=" + Size.Width + "x" + Size.Height + "&" +
                "maptype=" + MAP_TYPE + "&" +
                "key=" + API_KEY;

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
