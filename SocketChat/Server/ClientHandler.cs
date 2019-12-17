using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace ConsoleServer
{
    public class ClientHandler
    {
        public Socket AcceptedSocket;

        public ClientHandler(Socket AcceptedSocket)
        {
            this.AcceptedSocket = AcceptedSocket; 
        }
         
        public void Process() 
        {  
            try
            {  
                byte[] data = new byte[256]; // буфер для получаемых данных

                while (true)
                { 
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();

                    int bytes = 0; // количество полученных байтов 
                     
                    do
                    {
                        bytes = AcceptedSocket.Receive(data,data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (AcceptedSocket.Available > 0);
                     
                    // отправляем ответ
                    string message = builder.ToString();

                    Console.WriteLine(message);

                    message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                    data = Encoding.Unicode.GetBytes(message);
                    AcceptedSocket.Send(data);

                    // закрываем сокет
                    AcceptedSocket.Shutdown(SocketShutdown.Both);
                    AcceptedSocket.Close();
                }
            }  
            catch (Exception ex)
            {}
        } 
    }
}
 