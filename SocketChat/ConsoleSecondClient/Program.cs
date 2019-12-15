using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConsoleSecondClient
{
    class Client 
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8888; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static string userName;
        static void Main(string[] args)
        {
            Console.Write("Введите свое имя:");
            userName = Console.ReadLine();
            while (true)
            {

                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // подключаемся к удаленному хосту
                    socket.Connect(ipPoint);
                    // ввод сообщения
                    Console.Write(userName + ":");

                    string message = Console.ReadLine();
                    message = string.Format("{0}: {1}", userName, message);
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    // отправка сообщения 
                    socket.Send(data);

                    // получаем ответ
                    data = new byte[256]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байт 

                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine("сообщение доставлено! ");

                    // закрываем сокет
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
