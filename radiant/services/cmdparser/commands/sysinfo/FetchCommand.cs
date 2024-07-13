using System;
using System.Collections.Generic;
using cSystem = Cosmos.System;

namespace radiant.services.cmdparser.commands.sysinfo
{
    public class FetchCommand : Command
    {
        public override string[] Alias => new string[] { "fetch" };
        public override string Help => "Fetch system info";

        // Most of the code taken from z-izz/fetchOS with MIT License
        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (Cosmos.Core.CPU.GetCPUVendorName().Contains("Intel")) // if intel chip
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("88                              88");
                Console.WriteLine("\"\"              ,d              88");
                Console.WriteLine("                88              88");
                Console.WriteLine("88 8b,dPPYba, MM88MMM ,adPPYba, 88");
                Console.WriteLine("88 88P'   `\"8a  88   a8P_____88 88");
                Console.WriteLine("88 88       88  88   8PP\"\"\"\"\"\"\" 88");
                Console.WriteLine("88 88       88  88,  \"8b,   ,aa 88");
                Console.WriteLine("88 88       88  \"Y888 `\"Ybbd8\"' 88");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (Cosmos.Core.CPU.GetCPUVendorName().Contains("AMD")) // if amd chip
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("              *@@@@@@@@@@@@@@@    ");
                Console.WriteLine("                 @@@@@@@@@@@@@    ");
                Console.WriteLine("                @%       @@@@@    ");
                Console.WriteLine("              @@@%       @@@@@    ");
                Console.WriteLine("             @@@@&       @@@@@    ");
                Console.WriteLine("             @@@@@@@@@     @@@    ");
                Console.WriteLine("             #######              ");
                Console.WriteLine();
                Console.WriteLine("            @@     @\\ /@  @@@@*   ");
                Console.WriteLine("           @..@    @ @ @  @.   @  ");
                Console.WriteLine("          @    @   @   @  @@@@*   ");
            }
            int cy = Console.GetCursorPosition().Top - 12;
            Console.ForegroundColor = ConsoleColor.White;
#pragma warning disable CA1416 // Validate platform compatibility
            Console.Beep(250, 50);
            Console.Beep(500, 50);
#pragma warning restore CA1416 // Validate platform compatibility
            Console.SetCursorPosition(36, cy);
            Console.Write($"OS: RadiantOS {GlobalData.version}");
            Console.SetCursorPosition(36, cy + 1);
            Console.Write($"USER: {(Kernel.user != null ? Kernel.user.Name : "None")}");
            Console.SetCursorPosition(36, cy + 2);
            Console.Write($"CPU: {Cosmos.Core.CPU.GetCPUBrandString()}");
            Console.SetCursorPosition(36, cy + 3);
            Console.Write($"RAM: {Cosmos.Core.CPU.GetAmountOfRAM()} MB");
            Console.SetCursorPosition(36, cy + 4);
            Console.Write("VM: ");
            if (cSystem.VMTools.IsVMWare)
                Console.Write("VMware");
            else if (cSystem.VMTools.IsQEMU)
                Console.Write("QEMU (or KVM)");
            else if (cSystem.VMTools.IsVirtualBox)
                Console.Write("VirtualBox");
            else
                Console.Write("Not Virtualized");

            Console.SetCursorPosition(36, cy + 5);
            Console.Write("Shell: Radiant Shell");
            Console.SetCursorPosition(36, cy + 6);
            Console.Write($"Resolution: {Console.WindowWidth}x{Console.WindowHeight}");
            Console.SetCursorPosition(36, cy + 7);
            Console.Write("Drive: 0:\\");
            // This apparently crashes the kernel
            // Console.Write($"Disk (0:\\): {Filesystem.GetFreeSpaceOnDisk(0)} bytes / {Filesystem.GetAllSpaceOnDisk(0)} bytes ({Filesystem.GetFreePercentOnDisk(0)}%)");
            Console.SetCursorPosition(36, cy + 9);
            foreach (ConsoleColor col in new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.DarkGray })
            {
                Console.BackgroundColor = col;
                Console.Write("  ");
            }
            Console.SetCursorPosition(36, cy + 10);
            foreach (ConsoleColor col in new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.Gray })
            {
                Console.BackgroundColor = col;
                Console.Write("  ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, cy + 13);
        }
    }
}