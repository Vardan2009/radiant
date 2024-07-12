using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.builtin
{
    public class HelpCommand : Command
    {
        public override string[] Alias => new string[] { "help" };
        public override string Help => "displays this help message";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            Console.WriteLine("Available commands:");
            foreach (var command in CommandParser._commands)
            {
                Console.WriteLine($"{string.Join(", ", command.Alias)} - {command.Help}");
            }
        }
    }
}
