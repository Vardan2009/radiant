using radiant.services.networking;
using radiant.util;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.netmgmt
{
    public class NetConnectCommand : Command
    {
        public override string[] Alias => new string[] { "netconnect" };
        public override string Help => "Connect to network";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count != 1)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "`netconnect` takes no arguments");
                return;
            }
            NetManager.ConnectToNetwork();
        }
    }
}
