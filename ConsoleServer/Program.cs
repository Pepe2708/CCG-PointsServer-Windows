using System;
using System.Collections.Generic;
using System.IO;
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
            List<string> lines = new List<string>();

            if (File.Exists("scores.txt"))
            {
                Console.Title = "Server";
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                TcpListener tcpListener = new TcpListener(ip, 8001);

                while (true)
                {
                    tcpListener.Start();

                    Console.WriteLine("Waiting for connection...");

                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("Connection accpeted from: " + socket.RemoteEndPoint);

                    string message = "";

                    Byte[] bMessage = new Byte[256];
                    int messageSize = socket.Receive(bMessage);
                    Console.WriteLine("Message received!");

                    message = Encoding.UTF8.GetString(bMessage, 0, messageSize);
                    Console.WriteLine("Message: " + message);

                    lines = File.ReadAllLines("scores.txt").ToList();

                    if (message.Contains("D")) { Read(socket, lines); }
                    else { Write(socket, lines, message); }

                    tcpListener.Stop();
                }
            }
            else
            {
                File.Create("scores.txt");
                Console.WriteLine("Scores.txt file created. Please restart the server.");
            }
            


            Console.ReadKey();
        }

        public static void Write(Socket socket, List<string> lines, string message)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string ipAdress = socket.RemoteEndPoint.ToString().Split(':')[0];
                if (lines[i].Contains(ipAdress))
                {
                    lines.RemoveAt(i);
                    i--;
                }
            }

            lines.Add(socket.RemoteEndPoint + "|" + message);
            File.WriteAllLines("scores.txt", lines);
        }

        public static void Read(Socket socket, List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                string ipAdress = socket.RemoteEndPoint.ToString().Split(':')[0];
                if (lines[i].Contains(ipAdress))
                {
                    Byte[] bConfirm = Encoding.UTF8.GetBytes(lines[i].Split('|')[1]);
                    socket.Send(bConfirm);
                    Console.WriteLine(bConfirm);
                }
            }
        }
    }
}
