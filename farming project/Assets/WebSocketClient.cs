using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace WebSocket__Client
{
    public class WebSocketClient : MonoBehaviour
    {
        private Socket socket;
        private List<byte> cash = new List<byte>();
        private bool isRecieving = false;
        private string serverIP = "26.237.114.125";
        private int serverPort = 6666;
        private bool isConnected = false;

        private void Start()
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                Debug.Log($"尝试连接到服务器 {serverIP}:{serverPort}");
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                // 设置KeepAlive
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                
                // 异步连接
                IAsyncResult result = socket.BeginConnect(serverIP, serverPort, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(5000);
                
                if (!success)
                {
                    throw new Exception("连接超时");
                }
                
                socket.EndConnect(result);
                isConnected = true;
                Debug.Log("成功连接到服务器");
                StartReceive();
            }
            catch (Exception e)
            {
                Debug.LogError($"连接服务器失败: {e.Message}\n{e.StackTrace}");
                isConnected = false;
                // 尝试重新连接
                Invoke("ConnectToServer", 5f);
            }
        }

        private void StartReceive()
        {
            Task.Run(() =>
            {
                byte[] dataPool = new byte[1024 * 1024];
                while (isConnected)
                {
                    try
                    {
                        int recBit = socket.Receive(dataPool);
                        if (recBit <= 0)
                        {
                            Debug.Log("收到空包或连接断开");
                            isConnected = false;
                            break;
                        }

                        byte[] realData = new byte[recBit];
                        Buffer.BlockCopy(dataPool, 0, realData, 0, recBit);
                        cash.AddRange(realData);

                        if (!isRecieving)
                        {
                            DealPack();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"接收数据错误: {ex.Message}\n{ex.StackTrace}");
                        isConnected = false;
                        break;
                    }
                }
                
                // 如果连接断开，尝试重新连接
                if (!isConnected)
                {
                    Debug.Log("连接断开，准备重新连接...");
                    Invoke("ConnectToServer", 5f);
                }
            });
        }

        private void DealPack()
        {
            isRecieving = true;
            byte[] packer = OpenPack(ref cash);
            if (packer == null)
            {
                isRecieving = false;
                return;
            }

            string message = Encoding.UTF8.GetString(packer);
            Debug.Log($"收到服务器消息: {message}");
            
            // 解析收到的JSON消息
            JsonParser.ParseJson(message);

            DealPack();
        }

        private byte[] OpenPack(ref List<byte> cashList)
        {
            if (cashList.Count < 4)
            {
                return null;
            }

            try
            {
                byte[] cash = cashList.ToArray();
                using (MemoryStream fs = new MemoryStream(cash))
                using (BinaryReader read = new BinaryReader(fs))
                {
                    int packStaLen = read.ReadInt32();
                    int instByteLen = (int)fs.Length - 4;
                    if (instByteLen < packStaLen)
                    {
                        return null;
                    }
                    byte[] allByte = read.ReadBytes(packStaLen);
                    byte[] restByte = read.ReadBytes(instByteLen);

                    cashList.Clear();
                    cashList.AddRange(restByte);

                    return allByte;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"解包错误: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public void SendMessage(string message)
        {
            if (!isConnected)
            {
                Debug.LogWarning("未连接到服务器，无法发送消息");
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                byte[] wrappedData = WrapPack(data);
                socket.Send(wrappedData);
                Debug.Log($"发送消息成功: {message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"发送消息失败: {e.Message}\n{e.StackTrace}");
                isConnected = false;
                Invoke("ConnectToServer", 5f);
            }
        }

        private byte[] WrapPack(byte[] data)
        {
            using (MemoryStream fs = new MemoryStream())
            using (BinaryWriter ws = new BinaryWriter(fs))
            {
                ws.Write(data.Length);
                ws.Write(data);
                return fs.ToArray();
            }
        }

        private void OnDestroy()
        {
            isConnected = false;
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Debug.LogError($"关闭连接时出错: {e.Message}");
                }
            }
        }

        // 测试连接的方法
        public void TestConnection()
        {
            if (isConnected)
            {
                SendMessage("测试连接");
            }
            else
            {
                Debug.Log("未连接到服务器，正在尝试重新连接...");
                ConnectToServer();
            }
        }
    }
} 