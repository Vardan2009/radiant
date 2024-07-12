using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.sysutil
{
    public class ClearCommand : Command
    {
        public override string[] Alias => new string[] { "cls", "clr", "clear" };
        public override string Help => "Clears screen";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count != 1)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "`clear` takes no arguments");
                return;
            }
            Console.Clear();
        }
    }
}
