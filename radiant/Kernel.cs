/*
=================================================================
    ____            ___             __ 
   / __ \____ _____/ (_)___ _____  / /_
  / /_/ / __ `/ __  / / __ `/ __ \/ __/
 / _, _/ /_/ / /_/ / / /_/ / / / / /_  
/_/ |_|\__,_/\__,_/_/\__,_/_/ /_/\__/  
                                       
Main Kernel File
Licensed under GPL-3
=================================================================
*/

using radiant.services.accountmgr;
using radiant.services.cmdparser;
using radiant.services.filesystem;
using radiant.util;
using System;
using cSystem = Cosmos.System;

namespace radiant
{
    public class Kernel : cSystem.Kernel
    {
        static string pwd = @"0:\";
        public static string PWD
        {
            get { return pwd; }
            set
            {
                pwd = value.Replace("/", "\\");
            }
        }
        public static RadiantConfig config = RadiantConfig.DefaultConfig;
        public static Account user;

        protected override void BeforeRun()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            Console.CursorSize = 100;
#pragma warning restore CA1416 // Validate platform compatibility
            Console.Write("Try initializing Filesystem? (y/N) -> ");
            bool initFilesystem = Console.ReadKey().KeyChar == 'y';
            if (initFilesystem)
            {
                Filesystem.Init();
                Console.WriteLine();
                Console.Write("Read/Write radiant files? (this will create files to 0:\\) (y/N) -> ");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    Console.WriteLine();
                    Filesystem.CreateNecessarySystemFiles();
                    AccountManager.InitAccount();
                    config = RadiantConfig.ReadConfig(@"0:\radiant\config.cfg");
                }
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(GlobalData.commandLineLogo);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Radiant {GlobalData.version}");
            Console.WriteLine("Licensed under GPL-3");
            Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY");
            Console.WriteLine("This is free software, and you are welcome to\nredistribute it under certain conditions");
            Console.WriteLine("Type `license` for more information\n");
        }

        public void CommandLine()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (user != null)
                Console.Write(user.Name);
            else
                Console.Write("no-user");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("@");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(pwd);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("$:- ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected override void Run()
        {
            try
            {
                CommandLine();
                string command = Console.ReadLine();
                CommandParser.ParseCommand(command);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"FATAL: {e.Message}\nPlease report this issue at our GitHub Repository `https://github.com/Vardan2009/radiant/issues`. Thank you!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
