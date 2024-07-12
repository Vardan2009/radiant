using radiant.services.filesystem;
using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class WriteCommand : Command
    {
        public override string[] Alias => new string[] { "write", "fwrite", "fprint" };
        public override string Help => "Writes to a file or overwrites it";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: write <path> <content (optional)>");
                return;
            }
            string allText = "";
            if (args.Count == 3)
            {
                allText = args[2];
            }
            else
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "File write start, write `$EOF` to exit");
                while (true)
                {
                    string line = Console.ReadLine();
                    if (line == "$EOF") break;
                    allText += line + '\n';
                }
            }

            Filesystem.WriteFile(args[1], allText);
        }
    }

}
