using radiant.services.accountmgr;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.usrmgmt
{
    public class UseraddCommand : Command
    {
        public override string[] Alias => new string[] { "useradd" };
        public override string Help => "Displays a prompt for creating a new user";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count != 1)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "`useradd` takes no arguments");
                return;
            }
            AccountManager.CreateAcc(true);
        }
    }
}
