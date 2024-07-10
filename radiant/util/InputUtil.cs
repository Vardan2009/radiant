// Radiant Utilities
// Input

using System;

namespace radiant.util
{
    public class InputUtil
    {


        public static string ValidRead(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().ToLower();
                foreach (char c in input)
                {
                    if (!(c >= 'a' && c <= 'z'))
                    {
                        throw new Exception("Invalid Input!");
                    }
                }
                return input;
            }
        }

        public static string PasswordRead(string prompt)
        {
            Console.Write(prompt);
            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;

            string rawPass = Console.ReadLine();
            string md5 = EncodeUtil.ComputeHash(rawPass);

            Console.ForegroundColor = prevColor;

            return md5;
        }
    }
}
