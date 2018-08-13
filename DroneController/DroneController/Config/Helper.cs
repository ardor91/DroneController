using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DroneController.Config
{
    public class Helper
    {
        public static void SaveConfig(Settings config)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, config);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Properties.Settings.Default.settings = Convert.ToBase64String(buffer);
                Properties.Settings.Default.Save();
            }
        }

        public static Settings LoadConfig()
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.settings)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    return (Settings)bf.Deserialize(ms);
                }
                catch(Exception ex)
                {
                    return new Settings();
                }
            }
        }
    }
}
