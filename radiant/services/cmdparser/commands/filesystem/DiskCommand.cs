using radiant.services.filesystem;
using radiant.util;
using System;
using System.Collections.Generic;
using System.IO;

namespace radiant.services.cmdparser.commands.filesystem
{
    public class DiskCommand : Command
    {
        public override string[] Alias => new string[] { "disk" };
        public override string Help => "Disk Utility";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count != 1)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "disk: invalid arguments, use `disk --help` for help");
                return;
            }

            foreach (string key in kwargs.Keys)
            {
                switch (key)
                {
                    case "help":
                        Console.WriteLine("--- Actions ---");
                        Console.WriteLine("disk --list          -> List all available disks");
                        Console.WriteLine("disk --mount [index] -> Mount a disk with index");
                        break;
                    case "list":
                        DriveInfo[] disks = Filesystem.GetDisks();
                        for (int i = 0; i < disks.Length; i++)
                        {
                            Console.WriteLine($"Disk {i}\n    -> Name: {disks[i].Name}\n    -> Volume Label: {disks[i].VolumeLabel}\n    -> Size: {disks[i].TotalSize}\n    -> Format: {disks[i].DriveFormat}");
                        }
                        break;
                    case "mount":
                        ConsoleUtil.Message(ConsoleUtil.MessageType.WARN, "Experimental command");
                        Filesystem.MountDisk(Convert.ToInt32(kwargs[key]));
                        ConsoleUtil.Message(ConsoleUtil.MessageType.SUCCESS, $"Mounted disk {kwargs[key]}");
                        break;
                    default:
                        ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "disk: invalid arguments, use `disk --help` for help");
                        break;
                }
            }
        }
    }

}
