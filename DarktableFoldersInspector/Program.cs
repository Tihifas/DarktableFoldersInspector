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

FolderHelper.PrintFolderAndAllSubfoldersNames(darktableLibraryRootDI);

Console.WriteLine("Job finished. Press eny key to close.");
Console.ReadKey();
return 0;

