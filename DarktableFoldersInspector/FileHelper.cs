using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarktableFoldersInspector
{
    public class FileHelper
    {
        public static DateTime DateTaken(FileInfo fileInfo)
        {
            return fileInfo.LastWriteTime; //TODO: this seems to just be when it was exported, not the original date taken
        }
    }
}
