using radiant.services.filesystem;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class RemoveDirCommand : Command
    {
        public override string[] Alias => new string[] { "rmdir" };
        public override string Help => "removes directory";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: rmdir <path> | --recursive (optional)");
                return;
            }
            Filesystem.RemoveFolder(args[1], kwargs.TryGetValue("recursive", out string recursive) && recursive == "true");
        }
    }
}
