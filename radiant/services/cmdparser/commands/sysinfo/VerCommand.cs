using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.sysinfo
{
    public class VerCommand : Command
    {
        public override string[] Alias => new string[] { "ver" };
        public override string Help => "Show OS version info";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(GlobalData.commandLineLogo);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Radiant {GlobalData.version}");
            Console.WriteLine($"Report issues at https://github.com/Vardan2009/radiant/issues");
            Console.WriteLine("Author: Vardan2009 (github.com/Vardan2009)");
            Console.WriteLine("Contributors:");
            Console.WriteLine("--- gamma63 (github.com/gamma63)");
        }
    }
}
