using System;
using Sys = Cosmos.System;

namespace radiant
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Console.WriteLine("Radiant booted successfully. Type a line of text to get it echoed back.");
        }

        protected override void Run()
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            Console.WriteLine(input);
        }
    }
}
