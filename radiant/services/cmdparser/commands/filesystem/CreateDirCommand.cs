using radiant.services.filesystem;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class CreateDirCommand : Command
    {
        public override string[] Alias => new string[] { "mkdir" };
        public override string Help => "removes directory";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: mkdir <path>");
                return;
            }
            Filesystem.CreateFolder(args[1]);
        }
    }
}
