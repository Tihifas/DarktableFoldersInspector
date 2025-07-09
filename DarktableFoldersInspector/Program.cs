// See https://aka.ms/new-console-template for more information
using DarktableFoldersInspector;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

Console.WriteLine(""); //For better readability

string darktableLibraryRootFolderPath = "F:\\Canon EOS R10\\Darktable Library";
DirectoryInfo darktableLibraryRootFolder = new DirectoryInfo(darktableLibraryRootFolderPath);
if(!darktableLibraryRootFolder.Exists) 
{
    Console.WriteLine("Darktable Library root directory not found.");
    Console.WriteLine($"Path checked: {darktableLibraryRootFolderPath}");
    Console.WriteLine("Press any key to quit");
    Console.ReadKey();
    return 3;
}

Console.WriteLine($"Inspecting Darktable Library folder: {darktableLibraryRootFolder.FullName}");

DirectoryHelper.PrintDirectoryNameIndented(darktableLibraryRootFolder);


PrintStatusOfAllSubFolders();

Console.WriteLine("");
Console.WriteLine("Inspection finished. Press any key to quit.");
Console.ReadKey();
return 0;


void PrintStatusOfAllSubFolders()
{
    var subFolders = darktableLibraryRootFolder.GetDirectories();
    foreach (var subFolder in subFolders)
    {
        string statusIndent = DirectoryHelper.IndentationForDirectory(subFolder, nExtraElements: 1);
        if (!FolderNameMatchesCanonFolderNames(subFolder.Name)){
            Console.WriteLine($"{statusIndent}This folder does not match the expected Canon folder name pattern (starts with number and ends with 'CANON').");
        }
        else
        {
            PrintCanonFolderStatus(subFolder);
        }
    }
}

void PrintCanonFolderStatus(DirectoryInfo canonFolder)
{
    Console.WriteLine(""); //Empty for spacing between folders printed

    DirectoryHelper.PrintDirectoryNameIndented(canonFolder);

    string canonFolderIndent = DirectoryHelper.IndentationForDirectory(canonFolder, nExtraElements: 1);
    //Print if no export folder
    var exportFolders = canonFolder.GetDirectories("darktable_exported*", SearchOption.TopDirectoryOnly);

    if (exportFolders.Length == 0)
    {
        ConsoleHelper.WriteError($"{canonFolderIndent}No export folders found.");
    }
    else
    {
        foreach (var exportFolder in exportFolders)
        {
            DirectoryHelper.PrintDirectoryNameIndented(exportFolder);

            var filesInExportFolder = exportFolder.GetFiles();
            int nFiles = filesInExportFolder.Length;
            string exportFolderStatusIndent = DirectoryHelper.IndentationForDirectory(exportFolder, nExtraElements: 1);
            Console.WriteLine($"{exportFolderStatusIndent}Number of files: {nFiles}");

            bool isStandardExportFolder = (exportFolder.Name == "darktable_exported");

            if (isStandardExportFolder)
            {
                int limitForWarning = 5;
                if (nFiles < limitForWarning)
                {
                    ConsoleHelper.WriteWarning($"{exportFolderStatusIndent}Standard export folder contains less than {limitForWarning} files.");
                }
            }

            if(nFiles > 0)
            {

                string photosTakenDatesString = GetPhotosTakenDatesString(filesInExportFolder);
                Console.WriteLine($"{exportFolderStatusIndent}{photosTakenDatesString}");
            }
        }
    }
}

bool FolderNameMatchesCanonFolderNames(string name)
{
    bool startsWithNumber = Char.IsNumber(name[0]);
    bool endsWithCANON = name.Contains("CANON"); //contains instead of endsWith because things like 112CANON_2 should be ok.
    return startsWithNumber && endsWithCANON;
}

string GetPhotosTakenDatesString(FileInfo[] filesInExportFolder)
{
    FileInfo oldestFile = filesInExportFolder.OrderBy(f => FileHelper.DateTaken(f)).First();
    string oldestFileDateString = FileHelper.DateTaken(oldestFile).ToShortDateString();
    FileInfo newestFile = filesInExportFolder.OrderByDescending(f => FileHelper.DateTaken(f)).First();
    string newestFileDateString = FileHelper.DateTaken(newestFile).ToShortDateString();

    string datesTakenString = $"Photos taken {oldestFileDateString}";
    if (oldestFileDateString != newestFileDateString)
    {
        datesTakenString += $" - {newestFileDateString}";
    }

    return datesTakenString;
}