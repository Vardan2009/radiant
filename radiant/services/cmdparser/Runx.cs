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

namespace radiant.services.cmdparser
{
    public class Runx
    {
        public static void Do(string path, string[] args)
        {
            string[] lines = Filesystem.ReadFile(path).Split('\n');
            foreach (string line in lines)
            {
                CommandParser.ParseCommand(line, true, args);
            }
        }

        public static string[] ParseArgumentReferences(string[] oldArgs, string[] runxArgs)
        {
            string[] parsedArgs = new string[oldArgs.Length];
            oldArgs.CopyTo(parsedArgs, 0);

            for (int argidx = 0; argidx < parsedArgs.Length; argidx++)
            {
                string result = "";
                int i = 0;

                while (i < parsedArgs[argidx].Length)
                {
                    if (parsedArgs[argidx][i] == '%' && i + 1 < parsedArgs[argidx].Length && Char.IsDigit(parsedArgs[argidx][i + 1]))
                    {
                        int j = i + 1;
                        string refIdx = "";

                        while (j < parsedArgs[argidx].Length && Char.IsDigit(parsedArgs[argidx][j]))
                        {
                            refIdx += parsedArgs[argidx][j];
                            j++;
                        }

                        if (j < parsedArgs[argidx].Length && parsedArgs[argidx][j] == '%')
                        {
                            int runxIndex = Convert.ToInt32(refIdx);
                            if (runxIndex >= 0 && runxIndex < runxArgs.Length)
                            {
                                result += runxArgs[runxIndex];
                                i = j + 1;
                            }
                            else
                            {
                                throw new IndexOutOfRangeException($"runxArgs index out of bounds: {runxIndex}");
                            }
                        }
                        else
                        {
                            result += parsedArgs[argidx][i];
                            i++;
                        }
                    }
                    else
                    {
                        result += parsedArgs[argidx][i];
                        i++;
                    }
                }

                parsedArgs[argidx] = result;
            }

            return parsedArgs;
        }
    }
}
