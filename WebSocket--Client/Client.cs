using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace WebSocket_Client
{
    public class Client
    {
        public Action<byte[]> OnMsgEvent = (byte[] bytes ) => {};
        private Socket client;
        private List<byte> cash; //缓存

        private Queue<byte[]> SendList; //发送列表
        private bool isSend; //是否发送
        private bool isRecieving = false; //是否发送中

        public string IP
        {
            get
            {
                try
                {
                    return client.RemoteEndPoint.ToString();
                }
                catch
                {
                    return "错误";
                }
            }
        }

        //初始化一些数据
        public Client()
        {
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            cash = new List<byte>();
            SendList = new Queue<byte[]>();
            isSend = false;
        }

        public bool Connect(string ip, int port)
        {
            try
            {
                client.Connect(ip, port);
                Console.WriteLine("连接成功");
                StartRecieve();
                return true;
            }
            catch(Exception ex) {
                Console.WriteLine("连接失败：Msg=" + ex.Message);
                return false;
            }
        }

        public void StartRecieve()
        {
            Console.WriteLine("准备开始接收");
            byte[] DataPool = new byte[1024 * 1024];
            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine("循环开始");
                    try
                    {
                        Console.WriteLine("try开始");
                        int recBit = client.Receive(DataPool);
                        Console.WriteLine("datapool结果为："+ DataPool);
                        if (recBit <= 0)
                        {
                            Console.WriteLine("空包");
                            return;
                        }

                        byte[] RealData = new byte[recBit];
                        Buffer.BlockCopy(DataPool, 0, RealData, 0, recBit);

                        cash.AddRange(RealData);

                        if (!isRecieving)
                        {
                            Console.WriteLine("准备开始处理包");
                            DealPack();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error" + ex.Message);
                    }
                }
            });
        }

        public void DealPack()
        {
            Console.WriteLine("开始处理包");
            isRecieving = true;
            byte[] packer = Tools.OpenPack(ref cash);
            if (packer == null)
            {
                isRecieving = false;
                return;
            }

            OnMsgEvent.Invoke(packer);

            DealPack(); //递归
        }

        public void Send(byte[] sendPack)
        {
            Console.WriteLine("开始发送");
            SendList.Enqueue(sendPack);
            if (!isSend)
            {
                Console.WriteLine("if 语句执行");
                sendbuffer();
            }
        }

        public void sendbuffer()
        {
            Console.WriteLine("sendbuffer执行");
            isSend = true;
            if (SendList.Count <= 0)
            {
                isSend = false;
                return;
            }
            client.Send(Tools.WrapPack(SendList.Dequeue())); //从列表中取出数据并发送出去
            sendbuffer();
        }

    }

}
