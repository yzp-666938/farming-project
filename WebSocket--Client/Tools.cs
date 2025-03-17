using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebSocket_Client
{
    public class Tools
    {
        //解决半包连包的关键， 封装包头+包体 = 内容字节长度 + 内容字体
        public static byte[] OpenPack(ref List<byte> CashList) //解包
        {
            Console.WriteLine("开始解包");
            if (CashList.Count < 4)
            {
                Console.WriteLine("解包：大小不足4 无法解析");
                return null;
            }

            try
            {
                byte[] Cash = CashList.ToArray(); //缓存列表
                using (MemoryStream fs = new MemoryStream(Cash))
                using (BinaryReader read = new BinaryReader(fs))
                {
                    int PackStaLen = read.ReadInt32(); //读取包头长度
                    int InstByteLen = (int)fs.Length - 4; //定义当前字节长度
                    if (InstByteLen < PackStaLen)
                    {
                        Console.WriteLine($"解包：预设长度：{PackStaLen}, 实际长度：{InstByteLen}, 数据不完整");
                        return null;
                    }
                    byte[] AllByte = read.ReadBytes(PackStaLen); //完整消息
                    byte[] RestByte = read.ReadBytes(InstByteLen); //剩余消息 以便下载使用

                    CashList.Clear();
                    CashList.AddRange(RestByte);

                    return AllByte;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("解包错误" + ex.Message + ":" + ex.StackTrace);
                return null;
            }
        }

        public static byte[] WrapPack(byte[] data) //封包
        {
            using (MemoryStream fs = new MemoryStream())
                using(BinaryWriter ws = new BinaryWriter(fs)) 
            {
                ws.Write(data.Length); //确保这个数是int类
                ws.Write(data);

                return fs.ToArray();
            }
        }
    }
}
