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

        public static void Request(string url, int timeout = 5000)
        {
            if (!Connected)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.WARN, "Not connected to network, connecting...");
                ConnectToNetwork();
            }

            if (!Connected) return;

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

            string httpget = "GET " + "/" + " HTTP/1.1\r\n" +
                         "Accept: */*\r\n" +
                         "Accept-Encoding: identity\r\n" +
                        $"Host: {main}\r\n" +
                         "Connection: Keep-Alive\r\n\r\n";

            string messageToSend = httpget;
            byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
            stream.Write(dataToSend, 0, dataToSend.Length);

            byte[] receivedData = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
            string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);

            string[] responseParts = receivedMessage.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

            if (responseParts.Length == 2)
            {
                string headers = responseParts[0];
                string content = responseParts[1];
                Console.WriteLine(headers);
                Console.WriteLine(content);
            }

            stream.Close();
        }
    }
}
