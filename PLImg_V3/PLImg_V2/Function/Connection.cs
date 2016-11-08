using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PLImg_V2
{
    delegate void ConnectSeq();

    public class Connection : IDalsaCamera.IConnection
    {
        ConnectSeq connectSeq;


        public Connection()
        {
            SetConnectSeqChain();
            connectSeq();
        }

        public string ConfigFileName        { get; set; }
        public string ConfigFile            { get; set; }
        public int    ResourceIndex         { get; set; }
        public string ServerName            { get; set; }

        public void LoadSetting( )
        {
            String KeyPath = "Software\\Teledyne DALSA\\Sapera LT\\SapAcquisition";
            RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(KeyPath);
            if (RegKey != null)
            {
                ServerName = RegKey.GetValue("Server", "").ToString();
                ResourceIndex = (int)RegKey.GetValue("Resource", 0);
                if (File.Exists(RegKey.GetValue("ConfigFile", "").ToString()))
                    ConfigFile = RegKey.GetValue("ConfigFile", "").ToString();
                ConfigFileName = Path.GetFileName(ConfigFile);
            }
        }

        public void SaveSetting( )
        {
            String KeyPath = "Software\\Teledyne DALSA\\" + Assembly.GetExecutingAssembly().ToString() + "\\SapAcquisition";
            RegistryKey RegKey = Registry.CurrentUser.CreateSubKey(KeyPath);
            RegKey.SetValue("Server", ServerName);
            RegKey.SetValue("ConfigFile", ConfigFile);
            RegKey.SetValue("Resource", ResourceIndex);
        }

        public void SetConnectSeqChain()
        {
            connectSeq = (ConnectSeq)Delegate.Combine( 
                new ConnectSeq(LoadSetting),
                new ConnectSeq(SaveSetting));
        }

    }
}
