using radiant.services.filesystem;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class RemoveCommand : Command
    {
        public override string[] Alias => new string[] { "rm" };
        public override string Help => "Removes file";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: rm <path>");
                return;
            }
            Filesystem.RemoveFile(args[1]);
        }
    }
}
