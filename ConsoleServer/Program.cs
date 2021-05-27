using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ip, 8001);
            tcpListener.Start();

            Console.WriteLine("Waiting for connection...");

            Socket socket = tcpListener.AcceptSocket();
            Console.WriteLine("Connection accpeted from: " + socket.RemoteEndPoint);

            string message = "";

            while (message != "exit")
            {
                Byte[] bMessage = new Byte[256];
                int messageSize = socket.Receive(bMessage);
                Console.WriteLine("Message received!");

                message = Encoding.UTF8.GetString(bMessage, 0, messageSize);
                Console.WriteLine("Message: " + message);

                Byte[] bConfirm = Encoding.UTF8.GetBytes("Server confirmation");
                socket.Send(bConfirm);
            }

            socket.Close();

            tcpListener.Stop();
            Console.WriteLine("Server was shut down!");

            Console.ReadKey();
        }
    }
}
