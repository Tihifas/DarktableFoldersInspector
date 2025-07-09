// See https://aka.ms/new-console-template for more information
using DarktableFoldersInspector;
using System.IO;

DirectoryInfo darktableLibraryRootDI = new DirectoryInfo("F:\\Canon EOS R10\\Darktable Library");
if(!darktableLibraryRootDI.Exists) 
{
    Console.WriteLine("Darktable Library root directory does not exist. Press eny key to close");
    Console.ReadKey();
    return 3;
}

Console.WriteLine($"Inspecting Darktable Library folder: {darktableLibraryRootDI.FullName}");

PrintStatusOfAllSubFolders();

//FolderHelper.PrintFolderAndAllSubfoldersNames(darktableLibraryRootDI);


Console.WriteLine("Job finished. Press eny key to close.");
Console.ReadKey();
return 0;

void PrintStatusOfAllSubFolders()
{
    var subFolders = darktableLibraryRootDI.GetDirectories();
    foreach (var subFolder in subFolders)
    {
        DirectoryHelper.PrintDirectoryNameIndented(subFolder);
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
    string canonFolderIndent = DirectoryHelper.IndentationForDirectory(canonFolder, nExtraElements: 1);
    //Print if no export folder
    var exportFolders = canonFolder.GetDirectories("darktable_exported*", SearchOption.TopDirectoryOnly);

    if (exportFolders.Length == 0)
    {
        ConsoleHelper.WriteError($"{canonFolderIndent}No export folder found.");
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
            int limitForWarning = 5;
            if (nFiles < limitForWarning)
            {
                ConsoleHelper.WriteWarning($"{exportFolderStatusIndent}Export folder contains less than {limitForWarning} files.");
            }
            if(nFiles > 0)
            {
                FileInfo oldestFile = filesInExportFolder.OrderBy(f => FileHelper.DateTaken(f)).First();
                FileInfo newestFile = filesInExportFolder.OrderByDescending(f => FileHelper.DateTaken(f)).First();
                Console.WriteLine($"{exportFolderStatusIndent}Pictures from {FileHelper.DateTaken(oldestFile).ToShortDateString()} to {FileHelper.DateTaken(newestFile).ToShortDateString()}");
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