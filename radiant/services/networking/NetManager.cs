using Cosmos.HAL;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.IPv4.UDP.DNS;
using radiant.util;
using System;
using System.Net.Sockets;
using System.Text;

namespace radiant.services.networking
{
    public class NetManager
    {
        public static bool Connected;
        public static void ConnectToNetwork()
        {
            try
            {
                NetworkDevice nic = NetworkDevice.GetDeviceByName("eth0");
                IPConfig.Enable(nic, new Address(192, 168, 1, 69), new Address(255, 255, 255, 0), new Address(192, 168, 1, 254));
                using (var xClient = new DHCPClient())
                {
                    xClient.SendDiscoverPacket();
                }

                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Waiting...");

                while (NetworkConfiguration.CurrentAddress.ToString() == "") ;

                ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "IP: " + NetworkConfiguration.CurrentAddress.ToString());

                ConsoleUtil.Message(ConsoleUtil.MessageType.SUCCESS, "Connected Successfully!");
            }
            catch (Exception e)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Connection Failure: {e.Message}");
                return;
            }
            Connected = true;
        }

        public static string[] Request(string url, int timeout)
        {
            if (!Connected)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.WARN, "Not connected to network, connecting...");
                ConnectToNetwork();
            }

            if (!Connected) return Array.Empty<string>();

            string main = url.Split('/')[0];

            ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, $"Sending request to {main}...");
            using TcpClient client = new TcpClient();
            var dnsClient = new DnsClient();

            dnsClient.Connect(DNSConfig.DNSNameservers[0]);
            dnsClient.SendAsk(main);

            Address address = dnsClient.Receive(timeout);
            dnsClient.Close();
            string serverIp = address.ToString();
            int serverPort = 80;

            client.Connect(serverIp, serverPort);
            NetworkStream stream = client.GetStream();

            string[] urlAddress = url.Split('/');
            string webAddress = "";
            for (int i = 1; i < urlAddress.Length; i++)
            {
                webAddress += "/";
                webAddress += urlAddress[i];
            }

            if (webAddress == "") webAddress = "/";

            ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, $"Path -> {webAddress}");

            string getRequestString = "GET " + webAddress + " HTTP/1.1\r\n" +
                                     "Accept: */*\r\n" +
                                     "Accept-Encoding: identity\r\n" +
                                    $"Host: {main}\r\n" +
                                     "Connection: Keep-Alive\r\n\r\n";

            string messageToSend = getRequestString;
            byte[] requestBytes = Encoding.ASCII.GetBytes(messageToSend);
            stream.Write(requestBytes, 0, requestBytes.Length);

            byte[] receivedData = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
            string responce = Encoding.ASCII.GetString(receivedData, 0, bytesRead);

            string[] responceSplit = responce.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

            stream.Close();

            return responceSplit;
        }
    }
}
