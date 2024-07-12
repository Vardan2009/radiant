using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.sysinfo
{
    public class LicenseCommand : Command
    {
        public override string[] Alias => new string[] { "license" };
        public override string Help => "Show project license";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (kwargs.TryGetValue("brief", out string printBrief) && printBrief == "true")
                Console.WriteLine(License.licenseBrief);
            else
                ConsoleUtil.WriteBigText(License.license);
        }
    }
}
