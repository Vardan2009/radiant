/*
=================================================================
    ____            ___             __ 
   / __ \____ _____/ (_)___ _____  / /_
  / /_/ / __ `/ __  / / __ `/ __ \/ __/
 / _, _/ /_/ / /_/ / / /_/ / / / / /_  
/_/ |_|\__,_/\__,_/_/\__,_/_/ /_/\__/  
                                       
Main Kernel File
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
            Filesystem.Init();
            Filesystem.CreateNecessarySystemFiles();
            AccountManager.InitAccount();
            config = RadiantConfig.ReadConfig(@"0:\radiant\config.cfg");

            Console.Beep();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(GlobalData.commandLineLogo);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Radiant {GlobalData.version}");
        }

        public void CommandLine()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(user.Name);
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
                while (true)
                {
                    CommandLine();
                    string command = Console.ReadLine();
                    CommandParser.ParseCommand(command);
                }
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
