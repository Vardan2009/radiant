using Cosmos.System.Network.Config;
using System;
using System.Collections.Generic;

namespace radiant.services.cmdparser.commands.netmgmt
{
    public class NetConfigCommand : Command
    {
        public override string[] Alias => new string[] { "netconfig" };
        public override string Help => "Get IP config";

        // TODO: fix this
        //       currently it just gives an error
        public override void Execute(List<string> args, Dictionary<string, string> kwargs)
        {
            while (NetworkConfiguration.CurrentAddress.ToString() == "") ;
            Console.WriteLine($"IP              -> {NetworkConfiguration.CurrentAddress}");
            //Console.WriteLine($"Default Gateway -> {NetworkConfiguration.CurrentNetworkConfig.IPConfig.DefaultGateway}");
            //Console.WriteLine($"Subnet Mask     -> {NetworkConfiguration.CurrentNetworkConfig.IPConfig.SubnetMask}");
        }
    }
}
