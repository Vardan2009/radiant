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
                        Console.WriteLine($"Welcome, {name}!");
                        Kernel.user = acc;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Password");
                    }
                }
                else
                {
                    Console.WriteLine("No such account");
                }
            }
        }

        public static void CreateAcc()
        {
            while (true)
            {
                string name = InputUtil.ValidRead("New Account Username -> ");
                string pass = InputUtil.PasswordRead("New Account Password -> ");
                if (GetAccount(name) != null)
                {
                    Console.WriteLine("Account already exists!");
                }
                else
                {
                    Directory.CreateDirectory($@"0:\radiant\users\{name}");
                    Byte[] defconfig = new UTF8Encoding(true).GetBytes(pass);
                    using (FileStream fs = File.Create($@"0:\radiant\users\{name}\pass.private"))
                    {
                        fs.Write(defconfig, 0, defconfig.Length);
                    }
                    break;
                }
            }

        }

        public static void InitAccount()
        {
            if (Filesystem.GetListing(@"0:\radiant\users").Count == 0)
            {
                Console.WriteLine("No Accounts found!");
                CreateAcc();
            }
            LogIn();
        }
    }
}
