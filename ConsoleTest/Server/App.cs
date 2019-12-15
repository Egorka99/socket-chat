using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading; 

namespace ConsoleServer
{
    public class App
    {
        const int Port = 8888;
        const string Address = "127.0.0.1";
        static Socket ListenSocket;

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
                    ClientHandler clientHandler = new ClientHandler(ListenSocket);

                    Thread clientThread = new Thread(new ThreadStart(clientHandler.Process));
                    clientThread.Start();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
}
