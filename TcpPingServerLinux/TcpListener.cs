using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPPingServerLinux
{
    internal class TcpListener
    {
        public static void Listen()
        {
            var name = Dns.GetHostName(); // get container id
            var addressslocal = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            
            //Just to list out address
            foreach (var add in Dns.GetHostEntry(name).AddressList)
            {
                Console.WriteLine("---> " + add.ToString() + " AddressFamily" + add.AddressFamily.ToString());
            }
            IPAddress address = IPAddress.Any;
            string port = "1234";
            IPEndPoint ipEndPoint = new IPEndPoint(address, Int32.Parse(port));
            Socket listener = new Socket(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(100);
            Console.WriteLine($"Listening...{ipEndPoint.ToString()}");
            Console.WriteLine("Waiting for Connection...");
            while (true)
            {
                var handler = listener.Accept();
                Console.WriteLine("New Connection from:" + handler.RemoteEndPoint.ToString());
                while (true)
                {
                    // Receive message.
                    var buffer = new byte[1_024];
                    var received = handler.Receive(buffer, SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);

                    Console.WriteLine(response.ToString());
                    var eom = "<|EOM|>";
                    if (response.IndexOf(eom) > -1 /* is end of message */)
                    {
                        Console.WriteLine(
                            $"Socket server received message: \"{response.Replace(eom, "")}\"");

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                        handler.Send(echoBytes, 0);
                        Console.WriteLine(
                            $"Socket server sent acknowledgment: \"{ackMessage}\"");

                        break;
                    }
                }
            }
        }

    }
}
