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
using System.Threading;
using Microsoft.VisualBasic;


namespace SocketChat
{
    public partial class FirstClient : Form
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8888;
        static string address = "127.0.0.1";
        static string userName = "";
        Socket socket;
        Thread Listener;
        delegate void updater(string Data);
        updater Updater; 
        public FirstClient()
        {
            InitializeComponent();
            textBoxMessage.KeyDown += new KeyEventHandler(textBoxMessage_KeyDown);
            Updater = new updater(Update);
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

                message = string.Format("{0}: {1}", userName, message);

                // преобразуем сообщение в массив байтов 
                byte[] data = Encoding.Unicode.GetBytes(message);
                // отправка сообщения 
                socket.Send(data);    
            }  
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);      
            }
        }


        void ReceiveMessages()
        {
            while(socket.Connected) 
            {
                // получаем ответ
                byte[] data = new byte[256]; // буфер для ответа 
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do 
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                String Data = builder.ToString();

                Update(Data);
            }
        } 
          
        void Update(String Data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(Updater, Data);
                return;
            }
            listBoxChat.Items.Add(Data);
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
            Listener = new Thread(new ThreadStart(ReceiveMessages));
            Listener.IsBackground = true;
            Listener.Start();
            buttonSend.Enabled = true;
            buttonEnter.Enabled = false;
        }
    }
} 
