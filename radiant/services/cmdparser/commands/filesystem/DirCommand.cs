using Cosmos.System.FileSystem.Listing;
using radiant.services.filesystem;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class DirCommand : Command
    {
        public override string[] Alias => new string[] { "ls", "dir" };
        public override string Help => "Displays the contents of the present working directory";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            List<DirectoryEntry> entries = Filesystem.GetListing(Kernel.PWD);
            Console.WriteLine($"Listing of {Kernel.PWD}");
            foreach (var entry in entries)
            {
                switch (entry.mEntryType)
                {
                    case DirectoryEntryTypeEnum.File:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[FILE] -> {entry.mName} : {entry.mSize}");
                        break;
                    case DirectoryEntryTypeEnum.Directory:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[DIR]  -> {entry.mName} : {entry.mSize}");
                        break;
                    case DirectoryEntryTypeEnum.Unknown:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"[???]  -> {entry.mName} : {entry.mSize}");
                        break;
                }
            }
        }
    }
}
