using radiant.services.graphics;
using radiant.util;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.sysutil
{
    public class GraphicsCommand : Command
    {
        public override string[] Alias => new string[] { "graphics" };
        public override string Help => "Starts graphical mode";

        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count != 1)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Usage: graphics | --width ## --height ## (optional)");
                return;
            }
            GraphicsMain.Init(kwargs.TryGetValue("width", out string width) ? Convert.ToUInt32(width) : GraphicsMain.defaultScreenW, kwargs.TryGetValue("height", out string height) ? Convert.ToUInt32(height) : GraphicsMain.defaultScreenH);
        }
    }
}
