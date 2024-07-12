using radiant.services.accountmgr;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.usrmgmt
{
    public class LogoutCommand : Command
    {
        public override string[] Alias => new string[] { "logout", "exit" };
        public override string Help => "Logs out current user";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count != 1)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "`logout` takes no arguments");
                return;
            }
            AccountManager.LogOut();
        }
    }
}
