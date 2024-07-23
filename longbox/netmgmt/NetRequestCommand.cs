using radiant.services.networking;
using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.netmgmt
{
    public class NetRequestCommand : Command
    {
        public override string[] Alias => new string[] { "netrequest" };
        public override string Help => "Send an HTTP/1.1 Request to URL";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: netrequest <url> | --header and/or --content | --timeout (optional)");
                return;
            }

            string[] response;

            if (kwargs.TryGetValue("timeout", out string timeoutstr))
            {
                if (!int.TryParse(timeoutstr, out int timeout))
                {
                    timeout = 10000;
                }
                response = NetManager.Request(args[1], timeout);
            }
            else
            {
                response = NetManager.Request(args[1], 10000);
            }

            string header = response[0];
            string content = response[1];

            if (kwargs.TryGetValue("header", out string printHeader) && printHeader == "true")
                Console.WriteLine($"Header:\n{header}\n");

            if (kwargs.TryGetValue("content", out string printContent) && printContent == "true")
                Console.WriteLine($"Responce:\n{content}\n");
        }
    }
}
