using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PLImg_V2
{
    public class CheckAndCreateFolder
    {
        string FolderPath;

        public CheckAndCreateFolder(string basepath)
        {
            FolderPath = basepath;
        }

        public void SettingFolder(string basepath)
        {
            if (CheckFolder(basepath)) { return; }
            else
            {
                CreateFolder(basepath);
            }
        }

        bool CheckFolder(string path)
        {
            if (Directory.Exists(path))
            { return true; }
            else
            { return false; }
        }

        void CreateFolder(string path)
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
        }
           

    }
}
