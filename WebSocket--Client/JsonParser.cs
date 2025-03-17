using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebSocket__Client
{
    public class Commands
    {
        public string action { get; set; }
        public string method { get; set; }
        public int quantity { get; set; }
    }
    public class JsonParser
    {
        public static Commands JsonParse(string json)
        {
            Console.WriteLine("开始调用JsonParser");
            Console.WriteLine($"收到的JSON字符串: {json}");
            try 
            {
                Commands commands = JsonSerializer.Deserialize<Commands>(json);
                Console.WriteLine($"解析结果 - action: {commands.action}, method: {commands.method}, quantity: {commands.quantity}");
                return commands;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON解析错误: {ex.Message}");
                return null;
            }
        }
    }
}
