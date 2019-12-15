using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace ConsoleServer
{
    public class ClientHandler
    {
        public Socket clientSocket;

        public ClientHandler(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
        }

        public void Process()
        { 
            Socket handler = null;
            try
            { 
                handler = clientSocket.Accept();
                byte[] data = new byte[256]; // буфер для получаемых данных
                while (true)
                { 
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов  
                    do
                    {
                        bytes = handler.Receive(data,data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    // отправляем ответ
                    string message = builder.ToString();

                    Console.WriteLine(message);

                    message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {

            }
        } 
    }
}
 