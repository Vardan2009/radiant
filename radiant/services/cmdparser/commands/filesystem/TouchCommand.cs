using radiant.services.filesystem;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class TouchCommand : Command
    {
        public override string[] Alias => new string[] { "touch", "create", "new", "mk" };
        public override string Help => "Creates new file";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: touch <path>");
                return;
            }
            Filesystem.CreateFile(args[1]);
        }
    }
}
