using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets; 

namespace SocketChat
{
    public partial class SecondClient : Form
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8888;
        static string address = "127.0.0.1";
        static string userName = "";
        Socket socket;

        public SecondClient()
        {
            InitializeComponent();

            textBoxMessage.KeyDown += new KeyEventHandler(textBoxMessage_KeyDown);
        }

        public void Connect()
        {
            // получаем адрес для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
        }

        public void SendMessage() 
        {
            try
            {

                string message = textBoxMessage.Text;

                // ввод сообщения
                listBoxChat.Items.Add(userName + ": " + message);

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
                listBoxChat.Items.Add("Сообщение доставлено!");

                // закрываем сокет 
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void textBoxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // событие пробела на текстбоксе 
            {
                SendMessage();
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            // кнопка входа клиента 
            userName = textBoxName.Text;
            Connect();
            buttonSend.Enabled = true;
            buttonEnter.Enabled = false;
        }
    }

}
 
  