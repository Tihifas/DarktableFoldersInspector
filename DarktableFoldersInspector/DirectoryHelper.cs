using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarktableFoldersInspector
{
    public static class DirectoryHelper
    {
        public static void PrintDirAndSubDirNames(DirectoryInfo directoryInfo)
        {
            PrintDirectoryNameIndented(directoryInfo);

            DirectoryInfo[] subFolders = directoryInfo.GetDirectories();
            foreach (DirectoryInfo subFolder in subFolders)
            {
                PrintDirAndSubDirNames(subFolder);
            }
        }

        public static int DirectoryDepth(DirectoryInfo directoryInfo)
        {
            int nSlashCount = directoryInfo.FullName.Count(c => c == '\\');
            return nSlashCount;
        }

        public static string IndentationForDirectory(DirectoryInfo directoryInfo, string indentationElement = "  ", int nExtraElements = 0)
        {
            string indentationString = "";
            int dirDepth = DirectoryDepth(directoryInfo);
            
            int nIndentationElements = dirDepth + nExtraElements;

            for (int i = 0; i < nIndentationElements; i++)
            {
                indentationString += indentationElement;
            }

            return indentationString;
        }

        public static void PrintDirectoryNameIndented(DirectoryInfo directoryInfo)
        {
            string indentPrefix = IndentationForDirectory(directoryInfo);
            Console.WriteLine($"{indentPrefix}{directoryInfo.Name}");
        }


    }
}
