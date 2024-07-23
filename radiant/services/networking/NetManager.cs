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
    public class NetRequest
    {
        public string Url { get; set; }
        public string Method { get; set; } = "GET";
        public int Timeout { get; set; } = 10000;
        public string Headers { get; set; } = "";
        public string Body { get; set; } = null;
        public bool Verbose { get; set; } = false;
    }

    public class NetResponse
    {
        public int StatusCode { get; set; }
        public string Headers { get; set; }
        public string Body { get; set; }
    }

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

        public static NetResponse SendRequest(NetRequest req)
        {
            if (req.Verbose) ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "Checking network connection...");
            if (!Connected)
            {
                if (req.Verbose) ConsoleUtil.Message(ConsoleUtil.MessageType.WARN, "Not connected to network, connecting...");
                ConnectToNetwork();
            }

            if (!Connected) return new NetResponse();

            while (NetworkConfiguration.CurrentAddress.ToString() == "") ;
            if (req.Verbose) ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, "IP: " + NetworkConfiguration.CurrentAddress.ToString());
            if (req.Verbose) ConsoleUtil.Message(ConsoleUtil.MessageType.SUCCESS, "Network connection present!");

            string host = req.Url.Split('/')[0];

            if (req.Verbose) ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, $"Sending request to {host}...");
            using TcpClient client = new TcpClient();
            var dnsClient = new DnsClient();

            dnsClient.Connect(DNSConfig.DNSNameservers[0]);
            dnsClient.SendAsk(host);

            Address address = dnsClient.Receive(req.Timeout);
            dnsClient.Close();
            string serverIp = address.ToString();
            int serverPort = 80;

            client.Connect(serverIp, serverPort);
            NetworkStream stream = client.GetStream();

            string[] urlAddress = req.Url.Split('/');
            string webAddress = "";
            for (int i = 1; i < urlAddress.Length; i++)
            {
                webAddress += "/";
                webAddress += urlAddress[i];
            }

            if (webAddress == "") webAddress = "/";

            if (req.Verbose) ConsoleUtil.Message(ConsoleUtil.MessageType.INFO, $"Path -> {webAddress}");

            string requestString = req.Method + " " + webAddress + " HTTP/1.1\r\n" +
                                     "Accept: */*\r\n" +
                                     "Accept-Encoding: identity\r\n" +
                                    $"Host: {host}\r\n" +
                                     "Connection: Keep-Alive\r\n\r\n" +
                                     req.Headers;

            if (!string.IsNullOrEmpty(req.Body))
            {
                requestString += $"\r\nContent-Length: {Encoding.ASCII.GetByteCount(req.Body)}\r\n\r\n";
                requestString += req.Body;
            }

            string messageToSend = requestString;
            byte[] requestBytes = Encoding.ASCII.GetBytes(messageToSend);
            stream.Write(requestBytes, 0, requestBytes.Length);

            byte[] receivedData = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
            string responce = Encoding.ASCII.GetString(receivedData, 0, bytesRead);

            string[] responceSplit = responce.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

            stream.Close();

            return new NetResponse()
            {
                Headers = responceSplit[0],
                Body = responceSplit[1],
                StatusCode = ParseStatusCode(responceSplit[0])
            };
        }

        private static int ParseStatusCode(string headerPart)
        {
            var lines = headerPart.Split('\r', '\n');
            if (lines.Length > 0)
            {
                var statusLine = lines[0].Split(' ');
                if (statusLine.Length >= 3 && int.TryParse(statusLine[1], out int statusCode))
                {
                    return statusCode;
                }
            }
            return 0;
        }
    }
}
