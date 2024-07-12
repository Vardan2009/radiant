using radiant.services.filesystem;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class CopyCommand : Command
    {
        public override string[] Alias => new string[] { "cp", "copy" };
        public override string Help => "Copies file";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 3)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: cp <path1> <path2>");
                return;
            }
            Filesystem.CopyFile(args[1], args[2]);
        }
    }
}
