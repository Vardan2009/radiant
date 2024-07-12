using radiant.services.filesystem;
using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class CatCommand : Command
    {
        public override string[] Alias => new string[] { "cat", "read", "fread" };
        public override string Help => "Reads file contents";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: cat <path>");
                return;
            }
            Console.WriteLine(Filesystem.ReadFile(args[1]));
        }
    }
}
