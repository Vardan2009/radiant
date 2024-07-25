using radiant.services.filesystem;
using radiant.services.networking;
using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.netmgmt
{
    public class CurlCommand : Command
    {
        public override string[] Alias => new string[] { "curl" };
        public override string Help => "cURL CLI implementation";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: curl <url> [options]");
                return;
            }

            string timeoutStr = kwargs.TryGetValue("t", out string t) ? t : "10000";
            NetRequest req = new()
            {
                Url = args[1],
                Body = kwargs.TryGetValue("d", out string d) ? d : "",
                Timeout = int.TryParse(timeoutStr, out int to) ? to : 10000,
                Method = kwargs.TryGetValue("X", out string m) ? m : "GET",
                Headers = kwargs.TryGetValue("H", out string h) ? h : "",
                Verbose = kwargs.TryGetValue("v", out string verb)
            };

            bool noPrintBody = kwargs.TryGetValue("npb", out string nb);
            bool printRaw = kwargs.TryGetValue("pr", out string nh);
            bool printHeaders = kwargs.TryGetValue("i", out string pr);

            NetResponse res = NetManager.SendRequest(req);

            if (printHeaders)
                Console.WriteLine($"{res.Headers}\n\n");

            if (printRaw)
                Console.WriteLine($"{res.RawResponce}\n\n");

            if (!noPrintBody)
                Console.WriteLine($"{res.Body}\n");

            if (kwargs.TryGetValue("o", out string outputPath))
            {
                Filesystem.WriteFile(outputPath, res.Body);
            }
        }
    }
}
