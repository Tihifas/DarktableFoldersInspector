using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DarktableFoldersInspector
{
    public class FileHelper
    {
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        /// <summary>
        /// Returns the date taken from a file's metadata. Timezone have NOT been taken into account
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static DateTime DateTaken(FileInfo fileInfo)
        {
            string imagePath = fileInfo.FullName;
            //string imagePath = "F:\\Canon EOS R10\\Darktable Library\\114CANON\\darktable_exported\\IMG_6750.jpg;

            //This is slow, so for 106CANON-114CANON in debug it takes over 2 seconds, but fast enough in release build. This link had a way to do it faster for .net framework, but i could not get it to work https://stackoverflow.com/a/7713780
            IReadOnlyList<MetadataExtractor.Directory> metdadataDirectories = ImageMetadataReader.ReadMetadata(imagePath);
            string metadataDateTakenString = metdadataDirectories.Where(d => d.Name == "Exif IFD0").First().Tags.Where(t => t.Name == "Date/Time Original").FirstOrDefault().Description;
            //DateTime dateTaken = DateTime.Parse(dateTakenString);
            DateTime dateTaken = MetadataDateTimeStringToDateTime(metadataDateTakenString);

            return dateTaken;
        }

        private static DateTime MetadataDateTimeStringToDateTime(string metadataDateTimeString)
        {
            //Example metadatastring: 2025:07:06 14:26:18
            //ParseExact documentation: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parseexact?view=net-9.0#system-datetime-parseexact(system-string-system-string-system-iformatprovider)
            string format = "yyyy:MM:dd HH:mm:ss";
            DateTime dateTime = DateTime.ParseExact(metadataDateTimeString, format, null);
            return dateTime;
        }
    }
}
