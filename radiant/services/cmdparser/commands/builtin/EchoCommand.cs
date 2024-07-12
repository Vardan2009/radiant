using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.builtin
{
    public class EchoCommand : Command
    {
        public override string[] Alias => new string[] { "echo", "out" };
        public override string Help => "prints out given input";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            string seperator = " ";
            if (kwargs.TryGetValue("seperator", out string newSeperator)) seperator = newSeperator;
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: echo <message> | --seperator ## (optional)");
                return;
            }
            for (int i = 1; i < args.Count; i++)
            {
                Console.Write($"{args[i]}{(i < args.Count - 1 ? seperator : "")}");
            }
            Console.WriteLine();
        }
    }
}
