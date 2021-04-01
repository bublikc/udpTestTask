using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Config
    {
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        public List<string> Adresses { get; set; } = new List<string>();

        public Config() { }


    }
}
