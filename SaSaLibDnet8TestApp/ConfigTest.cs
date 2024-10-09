using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaSaLibDNet8TestAPP
{
    public class ConfigTest
    {
        public static ConfigTest Config;

        public string ABCDEFG;

        public DateTime StartUpDateTime;

        public ConfigTest()
        {
            StartUpDateTime = DateTime.Now;
        }
    }
}
