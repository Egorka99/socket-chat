using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace ConsoleServer
{
    public class Server
    {
        const int Port = 8888;
        const string Address = "127.0.0.1"; 
        static Socket ListenSocket;
        static List<ClientHandler> ServerList = new List<ClientHandler>();

        public static List<ClientHandler> GetServerList()
        {
            return ServerList; 
        }
        static void Main(string[] args)  
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                ListenSocket.Bind(ipPoint);

                // начинаем прослушивание
                ListenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)  
                {
                    //прием подключенного клиента 
                    Socket AcceptedSocket = ListenSocket.Accept(); 
                    ClientHandler ClientHandler = new ClientHandler(AcceptedSocket);
                    ServerList.Add(ClientHandler);
                    // создаем новый поток для обслуживания нового клиента
                    Thread ClientThread = new Thread(new ThreadStart(ClientHandler.Process));
                    ClientThread.Start();

                }
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
