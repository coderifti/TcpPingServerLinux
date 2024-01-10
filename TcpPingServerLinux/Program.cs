
using TCPPingServerLinux;

namespace TcpPingServerLinux
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            Console.WriteLine("Server Starting .. !");
            TcpListener.Listen();
            Console.WriteLine("Server Started .. !");
        }
    }
}