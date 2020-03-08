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

                    data = Encoding.Unicode.GetBytes(message);

                    int c = 1;
                    foreach (var item in Server.GetServerList())
                    {
                        item.AcceptedSocket.Send(data);
                        Console.WriteLine("Отправлено клиенту " + c);
                        c++;
                    } 
                   
                } 
            }  
            catch (Exception ex) 
            {}
        } 
    }
}
 