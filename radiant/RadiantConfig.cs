// Class for `radiant/.config` file

using radiant.util;
using System;
using System.IO;

namespace radiant
{
    public class RadiantConfig
    {
        public static readonly RadiantConfig DefaultConfig = new RadiantConfig
        {
            PCName = "root"
        };

        public static RadiantConfig ReadConfig(string path)
        {
            try
            {
                RadiantConfig customConfig = new RadiantConfig();
                string[] configContents = File.ReadAllLines(path);
                foreach (string line in configContents)
                {
                    string[] strings = line.Split("=");
                    string property = strings[0].Trim();
                    string value = strings[1].Trim();

                    // I can't find a better solution to this
                    customConfig.PCName = property switch
                    {
                        "PCName" => value,
                        _ => throw new Exception($"Invalid Property `{property}`"),
                    };
                }
                return customConfig;
            }
            catch
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Failed to read config file!");
                return DefaultConfig;
            }
        }

        public string PCName { get; private set; }
    }
}