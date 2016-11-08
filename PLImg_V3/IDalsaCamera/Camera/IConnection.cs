using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDalsaCamera
{
    public interface IConnection
    {
        string ConfigFileName { get; set; }
        string ServerName     { get; set; }
        string ConfigFile     { get; set; }
        int ResourceIndex     { get; set; }

        void LoadSetting();
        void SaveSetting();
    }
}
