using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarktableFoldersInspector
{
    public static class FolderHelper
    {
        public static void PrintFolderAndAllSubfoldersNames(DirectoryInfo directoryInfo)
        {
            PrintDirectoryNameIndented(directoryInfo);

            DirectoryInfo[] subFolders = directoryInfo.GetDirectories();
            foreach (DirectoryInfo subFolder in subFolders)
            {
                PrintFolderAndAllSubfoldersNames(subFolder);
            }
        }

        public static int DirectoryDepth(DirectoryInfo directoryInfo)
        {
            int nSlashCount = directoryInfo.FullName.Count(c => c == '\\');
            return nSlashCount;
        }

        public static void PrintDirectoryNameIndented(DirectoryInfo directoryInfo)
        {
            int dirDepth = DirectoryDepth(directoryInfo);
            string indentPrefix = new string(' ', dirDepth * 2);
            Console.WriteLine($"{indentPrefix}{directoryInfo.Name}");
        }
    }
}
