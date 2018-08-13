using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneController.Config
{
    [Serializable()]
    public class Settings
    {
        public int FieldId { get; set; }
        public List<int> Sectors { get; set; }
        public int DroneCount { get; set; }
        public String testLabel { get; set; }
    }
}
