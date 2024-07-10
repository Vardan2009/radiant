using Cosmos.System.FileSystem.Listing;
using radiant.services.accountmgr;
using radiant.services.filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace radiant.services.cmdparser
{
    public class CommandParser
    {
        public static readonly List<Command> _commands = new List<Command>();

        static CommandParser()
        {
            RegisterCommand(new EchoCommand());
            RegisterCommand(new ClearCommand());
            RegisterCommand(new HelpCommand());
            RegisterCommand(new DirCommand());
            RegisterCommand(new ChdirCommand());
            RegisterCommand(new CatCommand());
            RegisterCommand(new LogoutCommand());
            RegisterCommand(new UseraddCommand());
            RegisterCommand(new WriteCommand());
            RegisterCommand(new TouchCommand());
        }

        public static void RegisterCommand(Command command)
        {
            _commands.Add(command);
        }

        public static void ParseCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return;

            if (cmd.Trim().StartsWith("//")) return;

            string[] args = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
                return;

            Command command = _commands.FirstOrDefault(c => c.Alias.Contains(args[0], StringComparer.OrdinalIgnoreCase));
            if (command != null)
            {
                try
                {
                    command.Execute(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing command: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No such command");
            }
        }
    }

    public abstract class Command
    {
        public abstract string[] Alias { get; }
        public abstract string Help { get; }
        public abstract void Execute(string[] args);
    }

    // COMMAND DEFINITIONS
    public class EchoCommand : Command
    {
        public override string[] Alias => new string[] { "echo", "out" };
        public override string Help => "prints out given input";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: echo <message>");
                return;
            }
            for (int i = 1; i < args.Length; i++)
            {
                Console.Write($"{args[i]} ");
            }
            Console.WriteLine();
        }
    }

    public class UseraddCommand : Command
    {
        public override string[] Alias => new string[] { "useradd" };
        public override string Help => "Displays a prompt for creating a new user";

        public override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("`useradd` takes no arguments");
                return;
            }
            AccountManager.CreateAcc(true);
        }
    }

    public class ClearCommand : Command
    {
        public override string[] Alias => new string[] { "cls", "clr", "clear" };
        public override string Help => "Clears screen";

        public override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("`clear` takes no arguments");
                return;
            }
            Console.Clear();
        }
    }

    public class LogoutCommand : Command
    {
        public override string[] Alias => new string[] { "logout", "exit" };
        public override string Help => "Logs out current user";

        public override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("`logout` takes no arguments");
                return;
            }
            AccountManager.LogOut();
        }
    }

    public class HelpCommand : Command
    {
        public override string[] Alias => new string[] { "help" };
        public override string Help => "displays this help message";

        public override void Execute(string[] args)
        {
            Console.WriteLine("Available commands:");
            foreach (var command in CommandParser._commands)
            {
                Console.WriteLine($"{string.Join(", ", command.Alias)} - {command.Help}");
            }
        }
    }

    public class DirCommand : Command
    {
        public override string[] Alias => new string[] { "ls", "dir" };
        public override string Help => "Displays the contents of the present working directory";

        public override void Execute(string[] args)
        {
            List<DirectoryEntry> entries = Filesystem.GetListing(Kernel.PWD);
            Console.WriteLine($"Listing of {Kernel.PWD}");
            foreach (var entry in entries)
            {
                switch (entry.mEntryType)
                {
                    case DirectoryEntryTypeEnum.File:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[FILE] -> {entry.mName} : {entry.mSize}");
                        break;
                    case DirectoryEntryTypeEnum.Directory:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[DIR]  -> {entry.mName} : {entry.mSize}");
                        break;
                    case DirectoryEntryTypeEnum.Unknown:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"[???]  -> {entry.mName} : {entry.mSize}");
                        break;
                }
            }
        }
    }

    public class ChdirCommand : Command
    {
        public override string[] Alias => new string[] { "cd", "chdir" };
        public override string Help => "Changes directory";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: cd <path>");
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
                Console.WriteLine("Failed to change directory!");
                Kernel.PWD = oldpwd;
            }
        }
    }

    public class CatCommand : Command
    {
        public override string[] Alias => new string[] { "cat", "read", "fread" };
        public override string Help => "Reads file contents";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: cat <path>");
                return;
            }
            Console.WriteLine(Filesystem.ReadFile(args[1]));
        }
    }

    public class TouchCommand : Command
    {
        public override string[] Alias => new string[] { "touch", "create", "new" };
        public override string Help => "Creates new file";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: touch <path>");
                return;
            }
            Filesystem.CreateFile(args[1]);
        }
    }

    public class WriteCommand : Command
    {
        public override string[] Alias => new string[] { "write", "fwrite", "fprint" };
        public override string Help => "Writes to a file or overwrites it";

        public override void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: write <path>");
                return;
            }
            string allText = "";
            Console.WriteLine("File write start, write `$EOF` to exit");
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "$EOF") break;
                allText += '\n';
                allText += line;
            }
            Filesystem.WriteFile(args[1], allText);
        }
    }
}
