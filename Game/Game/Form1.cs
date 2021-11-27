using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        private NetworkStream stream;
        private TcpClient tcpClient;
        SoundPlayer player = new SoundPlayer("D:/VS2019WORK/Game/帝听sakya_千界_千月兔-暖色胶片.wav");
        //定义发送数据的套接字
        Socket socket_send;
        private int flag;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            //套接字建立连接


            socket_send = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("10.1.230.74"), 3900);
            socket_send.Connect(point);
            try
            {
                //向指定的IP地址的服务器发送连接请求
                tcpClient.Connect("10.1.230.74", 3900);
                listBox1.Items.Add("连接成功");
                stream = tcpClient.GetStream();
                receive_stream();//接收字节流并显示在屏幕上

            }
            catch
            {
                listBox1.Items.Add("服务器未启动");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tcpClient.Connected)
            {
                string action = textBox1.Text.ToString();
                listBox1.Items.Add("输入的信息为：" + action);
                send_stream(action);
                receive_stream();

            }
            else
            {
                listBox1.Items.Add("连接已断开");
            }
        }




        private void button4_Click(object sender, EventArgs e)
        {
                player.Load();
                player.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /*
        * 将字节流用GBK格式编码在listbox里显示
        */
        void receive_stream()
        {
            byte[] receive_data = new byte[1024];
            //定义编码格式
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//为使用GB2312做准备
            System.Text.Encoding GBK = System.Text.Encoding.GetEncoding("GBK");
            if (stream.CanRead)
            {

                int len = stream.Read(receive_data, 0, receive_data.Length);
                string msg = GBK.GetString(receive_data, 0, receive_data.Length);

                string str = "\r\n";
                char[] str1 = str.ToCharArray();
                //乱码集合
                string[] messy_code = { "??[2J ", "[5m", "[44m", "[37;0m", "[1;33m", "[1;32m", "[1;31m" };
                string[] msg1 = msg.Split(str1);//以换行符为分隔符
                for (int j = 0; j < msg1.Length; j++)//逐行显示
                {
                    //过滤乱码
                    msg1[j] = msg1[j].Replace(messy_code[0], " ");
                    msg1[j] = msg1[j].Replace(messy_code[1], " ");
                    msg1[j] = msg1[j].Replace(messy_code[2], " ");
                    msg1[j] = msg1[j].Replace(messy_code[3], " ");
                    msg1[j] = msg1[j].Replace(messy_code[4], " ");
                    msg1[j] = msg1[j].Replace(messy_code[5], " ");
                    msg1[j] = msg1[j].Replace(messy_code[6], " ");
                    listBox1.Items.Add(msg1[j]);
                }
            }
        }
        void send_stream(string str)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//为使用GB2312做准备
            System.Text.Encoding GBK = System.Text.Encoding.GetEncoding("GBK");
            byte[] buffer = GBK.GetBytes(str + "\n");
            stream.Write(buffer, 0, buffer.Length);
        }
        private void start_game_Click(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            //套接字建立连接


            socket_send = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("10.1.230.74"), 3900);
            socket_send.Connect(point);
            try
            {
                //向指定的IP地址的服务器发送连接请求
                tcpClient.Connect("10.1.230.74", 3900);
                listBox1.Items.Add("连接成功");
                stream = tcpClient.GetStream();
                receive_stream();//接收字节流并显示在屏幕上

            }
            catch
            {
                listBox1.Items.Add("服务器未启动");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (stream != null)//关闭连接，关闭流
            {
                stream.Close();
                tcpClient.Close();
                socket_send.Close();
            }
            listBox1.Items.Add("已经退出游戏");
        }

        private void button5_Click(object sender, EventArgs e)
        {
                player.Stop();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread th = new Thread(play_pic);
            th.IsBackground = true;
            th.Start();

        }
        void play_pic()
        {
            flag++;
            string picturePath = @"D:\VS2019WORK\Game\pic\" + flag + ".jpg";
            pictureBox1.Image = Image.FromFile(picturePath);
            if (flag == 4)
            {
                flag = 0;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            flag++;
            string picturePath = @"D:\VS2019WORK\Game\pic\" + flag + ".jpg";
            pictureBox1.Image = Image.FromFile(picturePath);
            if (flag == 4)
            {
                flag = 0;
            }
        }
    }
}
