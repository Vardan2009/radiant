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
using System;
using cSystem = Cosmos.System;

namespace radiant
{
    public class Kernel : cSystem.Kernel
    {
        public static string pwd = @"0:\";
        public static RadiantConfig config = RadiantConfig.DefaultConfig;
        public static Account user;

        protected override void BeforeRun()
        {
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
            Console.CursorSize = 100;

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
                Console.WriteLine($"FATAL ERROR: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
