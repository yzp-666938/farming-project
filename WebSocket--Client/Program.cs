using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocket__Client;

namespace WebSocket_Client
{
    internal class Program
    {
        static bool isfile = false;
        static bool isJson = false;
        static string filename;
        public 
        static void Main(string[] args)
        {
            Console.WriteLine("Socket TCP Client");
            Client client = new Client();
            Console.WriteLine("请输入IP地址：");
            string IP = Console.ReadLine();
            Console.WriteLine("请输入端口：");
            int port = int.Parse(Console.ReadLine());
            client.Connect(IP, port);

            //定义事件，客户端收到完整数据会触发这里
            client.OnMsgEvent += (byte[] bytes) =>
            {
                //Console.WriteLine($"收到服务器的数据,大小：{bytes.Length},文本内容:{Encoding.UTF8.GetString(bytes)}");
                string clientIP = client.IP;
                string txtContain = Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"收到客户端{clientIP}的数据,大小：{bytes.Length},文本内容:{txtContain}");
                JsonParser.ParseJson(txtContain);
                //if (!isfile)
                //{
                //string txtContain = Encoding.UTF8.GetString(bytes);
                //Console.WriteLine($"收到服务端{clientIP}的数据,大小：{bytes.Length},文本内容:{txtContain}");
                //if (txtContain.StartsWith("文件*"))
                //{
                //isfile = true;
                //filename = txtContain.Replace("文件*", "");
                //Console.WriteLine("收到文件消息");

                //}
                //}
                //else
                //{
                //Console.WriteLine($"收到客户端{clientIP}发过来的文件{filename},文件大小{bytes.Length}");
                //File.WriteAllBytes(@"E:\代码\c#\websocket学习\FromServer-RecieveFile\" + filename, bytes);
                //}
                };

                CommandParser comm = new CommandParser();
            comm.OnCommandTask += (CommandParser.CommArgs commArgs) =>
            {
                switch (commArgs.title)
                {
                    case "sendtxt":

                        string sendingData = ""; commArgs.CommandSeries.Skip(1).ToList().ForEach(x => sendingData += x);
                        byte[] containByte = Encoding.UTF8.GetBytes(sendingData);
                        Console.WriteLine($"发送内容：{sendingData}，字节大小：{containByte.Length}");
                        client.Send(containByte);

                        break;

                    case "sendfile":
                        Console.WriteLine("请输入文件名称:");
                        string name = Console.ReadLine();
                        Console.WriteLine("请输入文件路径：");
                        string filepath = Console.ReadLine();
                        if (File.Exists(filepath))
                        {
                            client.Send(Encoding.UTF8.GetBytes("文件*" + name));
                            client.Send(File.ReadAllBytes(filepath));
                        }
                        else
                        {
                            Console.WriteLine("发送失败，找不到该文件");
                        }

                        break;
                }
            };
            comm.Start();
        }
    }
}
