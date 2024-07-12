// radiant runx file parser

// runx is a file extension for radiant command files (similar to sh in linux or batch in windows)
// each line is executed as a command
// to reference arguments given to the executable (like `test` in `custom.runx test`), use %n% where n is the argument (starting from one)

// RUNX EXAMPLE (greet.runx)
// echo Hello, %1%!

// to run the file
// greet World
// -- or --
// greet.runx World
// -- or --
// path/to/greet World
// -- or --
// path/to/greet.runx World

using radiant.services.filesystem;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser
{
    public class Runx
    {
        public static void Do(string path, List<string> args)
        {
            string[] lines = Filesystem.ReadFile(path).Split('\n');
            foreach (string line in lines)
            {
                CommandParser.ParseCommand(line, true, args);
            }
        }

        public static Dictionary<string, string> ParseKwargReferences(Dictionary<string, string> oldKwargs, List<string> runxArgs)
        {
            Dictionary<string, string> parsedKwargs = new(oldKwargs);

            foreach (string key in parsedKwargs.Keys)
            {
                parsedKwargs[key] = ParseArgumentReference(parsedKwargs[key], runxArgs);
            }

            return parsedKwargs;
        }

        public static string ParseArgumentReference(string oldArg, List<string> runxArgs)
        {
            string result = "";
            int i = 0;

            while (i < oldArg.Length)
            {
                if (oldArg[i] == '%' && i + 1 < oldArg.Length && Char.IsDigit(oldArg[i + 1]))
                {
                    int j = i + 1;
                    string refIdx = "";

                    while (j < oldArg.Length && Char.IsDigit(oldArg[j]))
                    {
                        refIdx += oldArg[j];
                        j++;
                    }

                    if (j < oldArg.Length && oldArg[j] == '%')
                    {
                        int runxIndex = Convert.ToInt32(refIdx);
                        if (runxIndex >= 0 && runxIndex < runxArgs.Count)
                        {
                            result += runxArgs[runxIndex];
                            i = j + 1;
                        }
                        else
                        {
                            throw new IndexOutOfRangeException($"Not enough arguments given!");
                        }
                    }
                    else
                    {
                        result += oldArg[i];
                        i++;
                    }
                }
                else
                {
                    result += oldArg[i];
                    i++;
                }
            }

            return result;
        }

        public static List<string> ParseArgumentReferences(List<string> oldArgs, List<string> runxArgs)
        {
            List<string> parsedArgs = new(oldArgs);

            for (int argidx = 0; argidx < parsedArgs.Count; argidx++)
            {
                parsedArgs[argidx] = ParseArgumentReference(parsedArgs[argidx], runxArgs);
            }

            return parsedArgs;
        }
    }
}
