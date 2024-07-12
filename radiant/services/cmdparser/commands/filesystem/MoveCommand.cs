using radiant.services.filesystem;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class MoveCommand : Command
    {
        public override string[] Alias => new string[] { "mv", "move" };
        public override string Help => "Moves file";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 3)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: mv <path1> <path2>");
                return;
            }
            Filesystem.MoveFile(args[1], args[2]);
        }
    }
}
