using radiant.util;
using System;
using System.Collections.Generic;
using System.IO;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class ChdirCommand : Command
    {
        public override string[] Alias => new string[] { "cd", "chdir" };
        public override string Help => "Changes directory";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count < 2)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: cd <path>");
                return;
            }
            string oldpwd = Kernel.PWD;
            try
            {
                if (Path.IsPathRooted(args[1]))
                {
                    Kernel.PWD = args[1];
                }
                else
                {
                    if (args[1] == "..")
                    {
                        DirectoryInfo parent = Directory.GetParent(Kernel.PWD);
                        if (parent != null)
                            Kernel.PWD = parent.FullName;
                    }
                    else if (args[1] == ".") { }
                    else
                    {
                        Kernel.PWD = Path.Join(Kernel.PWD, args[1]);
                    }
                }

                if (!Directory.Exists(Kernel.PWD))
                {
                    throw new Exception("No such path");
                }
            }
            catch
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Failed to change directory!");
                Kernel.PWD = oldpwd;
            }
        }
    }
}
