using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebSocket__Client
{
    public class Commands1
    {
        public string type { get; set; }
        public string action { get; set; }
        public string method { get; set; }
        public int quantity { get; set; }
    }
    public class Commands2
    {
        public string type { get; set; }
        public string action { get; set; }
        public string method { get; set; }
        public int quantity { get; set; }
    }
    public class JsonParser
    {
        public static void ParseJson(string json)
        {
            Console.WriteLine("开始调用JsonParser");
            // 先读取公共字段 "type"
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;
                string type = root.GetProperty("type").GetString();

                // 根据类型决定如何解析
                switch (type)
                {
                    case "person":
                        Commands1 commands1 = JsonSerializer.Deserialize<Commands1>(json);
                        Console.WriteLine($"Parsed Person: {commands1.action}, {commands1.method},{commands1.quantity}");
                        break;

                    case "product":
                        Commands2 commands2 = JsonSerializer.Deserialize<Commands2>(json);
                        Console.WriteLine($"Parsed Person: {commands2.action}, {commands2.method},{commands2.quantity}");
                        break;

                    default:
                        Console.WriteLine("Unknown type");
                        break;
                }
            }
        }
        //public static Commands1 Parse(string json)
        //{
            //Console.WriteLine("开始调用JsonParser");
            //Console.WriteLine($"收到的JSON字符串: {json}");
            //try 
            //{
                //Commands1 commands = JsonSerializer.Deserialize<Commands1>(json);
                //Console.WriteLine($"解析结果 - action: {commands.action}, method: {commands.method}, quantity: {commands.quantity}");
                //return commands;
            //
            //catch (Exception ex)
            //{
                //Console.WriteLine($"JSON解析错误: {ex.Message}");
                //return null;
            //}
        //}
    }
}
