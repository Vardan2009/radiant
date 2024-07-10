using radiant.services.filesystem;
using radiant.util;
using System;
using System.IO;
using System.Text;

namespace radiant.services.accountmgr
{
    public class AccountManager
    {
        public static Account GetAccount(string name)
        {
            if (File.Exists(@$"0:\radiant\users\{name}\pass.private"))
            {
                return new Account
                {
                    Name = name,
                    PasswordEncrypted = File.ReadAllText(@$"0:\radiant\users\{name}\pass.private")
                };
            }
            else
            {
                return null;
            }
        }

        public static void LogIn()
        {
            while (true)
            {
                string name = InputUtil.ValidRead("LogIn Username -> ");
                string pass = InputUtil.PasswordRead("LogIn Password -> ");
                Account acc;
                if ((acc = GetAccount(name)) != null)
                {
                    if (acc.PasswordEncrypted == pass)
                    {
                        ConsoleUtil.Message(ConsoleUtil.MessageType.SUCCESS, $"Welcome, {name}!");
                        Kernel.user = acc;
                        break;
                    }
                    else
                    {
                        ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Wrong password");
                    }
                }
                else
                {
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "No such account");
                }
            }
        }

        public static void LogOut()
        {
            if (Kernel.user == null) return;
            Kernel.user = null;
            ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Logged out");
            LogIn();
        }

        public static void CreateAcc(bool allowExit = false)
        {
            while (true)
            {
                string name = InputUtil.ValidRead(allowExit ? "New Account Username (`exit` to exit prompt) -> " : "New Account Username -> ");
                if (name == "exit" && allowExit)
                    break;

                if (GetAccount(name) != null)
                {
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Account already exists");
                    continue;
                }

                string pass = InputUtil.PasswordRead("New Account Password -> ");

                Directory.CreateDirectory($@"0:\radiant\users\{name}");
                Byte[] defconfig = new UTF8Encoding(true).GetBytes(pass);
                using (FileStream fs = File.Create($@"0:\radiant\users\{name}\pass.private"))
                {
                    fs.Write(defconfig, 0, defconfig.Length);
                }
                break;
            }
        }

        public static void InitAccount()
        {
            if (Filesystem.GetListing(@"0:\radiant\users").Count == 0)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.WARN, "No accounts found, creating one");
                CreateAcc();
            }
            LogIn();
        }

    }
}
