using radiant.services.filesystem;
using radiant.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace radiant.services.cmdparser
{
    public class CommandParser
    {
        public static readonly List<Command> _commands = new();

        static CommandParser()
        {
            RegisterCommand(new commands.builtin.EchoCommand());
            RegisterCommand(new commands.builtin.HelpCommand());

            RegisterCommand(new commands.filesystem.DirCommand());
            RegisterCommand(new commands.filesystem.ChdirCommand());
            RegisterCommand(new commands.filesystem.CatCommand());
            RegisterCommand(new commands.filesystem.WriteCommand());
            RegisterCommand(new commands.filesystem.TouchCommand());
            RegisterCommand(new commands.filesystem.DiskCommand());
            RegisterCommand(new commands.filesystem.MoveCommand());
            RegisterCommand(new commands.filesystem.CopyCommand());
            RegisterCommand(new commands.filesystem.CreateDirCommand());
            RegisterCommand(new commands.filesystem.RemoveCommand());
            RegisterCommand(new commands.filesystem.RemoveDirCommand());

            RegisterCommand(new commands.sysutil.ClearCommand());

            RegisterCommand(new commands.sysinfo.LicenseCommand());
            RegisterCommand(new commands.sysinfo.VerCommand());
            RegisterCommand(new commands.sysinfo.FetchCommand());

            RegisterCommand(new commands.usrmgmt.LogoutCommand());
            RegisterCommand(new commands.usrmgmt.UseraddCommand());

            RegisterCommand(new commands.netmgmt.NetConnectCommand());
            RegisterCommand(new commands.netmgmt.NetRequestCommand());
            RegisterCommand(new commands.netmgmt.NetConfigCommand());
        }

        public static void RegisterCommand(Command command)
        {
            _commands.Add(command);
        }

        static (List<string>, Dictionary<string, string>) ParseCommandLine(string cmd)
        {
            List<string> args = new List<string>();
            Dictionary<string, string> kwargs = new Dictionary<string, string>();
            bool insideQuotes = false;
            int startIndex = 0;

            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if (cmd[i] == ' ' && !insideQuotes)
                {
                    if (i > startIndex)
                    {
                        string arg = cmd.Substring(startIndex, i - startIndex);
                        args.Add(arg);
                    }
                    startIndex = i + 1;
                }
            }

            if (startIndex < cmd.Length)
            {
                string lastArg = cmd.Substring(startIndex);
                args.Add(lastArg);
            }

            for (int i = 0; i < args.Count; i++)
            {
                if (args[i].StartsWith('"') && args[i].EndsWith('"'))
                {
                    args[i] = args[i].Substring(1, args[i].Length - 2);
                }
            }

            for (int i = args.Count - 1; i >= 0; i--)
            {
                if (args[i].StartsWith("--"))
                {
                    string key = args[i].Substring(2);
                    string value = "true";

                    if (i + 1 < args.Count && !args[i + 1].StartsWith("--"))
                    {
                        value = args[i + 1];
                        args.RemoveAt(i + 1);
                    }

                    kwargs[key] = value;
                    args.RemoveAt(i);
                }
            }

            return (args, kwargs);
        }

        public static void ParseCommand(string cmd, bool runx = false, List<string> runxArgs = null)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return;

            if (cmd.Trim().StartsWith("//")) return;

            (List<string> args, Dictionary<string, string> kwargs) = ParseCommandLine(cmd);

            if (args.Count == 0)
                return;

            Command command = _commands.FirstOrDefault(c => c.Alias.Contains(args[0], StringComparer.OrdinalIgnoreCase));
            if (command != null)
            {
                try
                {
                    if (runx)
                    {
                        command.ExecuteRunx(args, runxArgs, kwargs);
                    }
                    else
                    {
                        command.Execute(args, kwargs);
                    }
                }
                catch (Exception ex)
                {
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Error executing command: {ex.Message}");
                }
            }
            else
            {
                string filePath = Filesystem.FindPathRoot(args[0]);
                if (File.Exists(filePath))
                    Runx.Do(filePath, args);
                else if (File.Exists(filePath + ".runx"))
                    Runx.Do(filePath + ".runx", args);
                else
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"{args[0]} is not a known command or executable");
            }
        }
    }

    public abstract class Command
    {
        public abstract string[] Alias { get; }
        public abstract string Help { get; }
        public abstract void Execute(List<string> args, Dictionary<string, string> kwargs);

        public void ExecuteRunx(List<string> args, List<string> runxArgs, Dictionary<string, string> kwargs)
        {
            Execute(Runx.ParseArgumentReferences(args, runxArgs), Runx.ParseKwargReferences(kwargs, runxArgs));
        }
    }
}
