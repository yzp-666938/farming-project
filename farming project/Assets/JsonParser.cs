using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebSocket__Client
{
    [Serializable]
    public class BaseModel
    {
        public string type;
    }

    [Serializable]
    public class ModelData : BaseModel
    {
        public string modelName;
        public Vector3 position_position;
    }

    [System.Serializable]
    public class Modelmove : BaseModel
    {
        public string modelName;
        public Vector3 position_position;
        public Vector3 target_target;
    }

    public class JsonParser : MonoBehaviour
    {
        private static JsonParser instance;
        private httpt httptInstance;
        private Queue<Action> mainThreadActions = new Queue<Action>();

        private void Awake()
        {
            instance = this;
            httptInstance = FindObjectOfType<httpt>();
        }

        private void Update()
        {
            // 执行主线程操作
            while (mainThreadActions.Count > 0)
            {
                Action action = mainThreadActions.Dequeue();
                action?.Invoke();
            }
        }

        public static void ParseJson(string json)
        {
            Debug.Log("开始调用JsonParser");
            Debug.Log($"收到的原始JSON: {json}");
            
            try
            {
                // 清理JSON字符串
                json = json.Trim();
                if (json.StartsWith("\""))
                {
                    json = json.Substring(1);
                }
                if (json.EndsWith("\""))
                {
                    json = json.Substring(0, json.Length - 1);
                }
                json = json.Replace("\\\"", "\"");
                
                Debug.Log($"清理后的JSON: {json}");

                // 先解析基础类型获取type字段
                BaseModel baseModel = JsonUtility.FromJson<BaseModel>(json);
                if (baseModel == null)
                {
                    Debug.LogError("JSON解析失败：无法解析为BaseModel");
                    return;
                }

                string type = baseModel.type;
                Debug.Log($"解析到的类型: {type}");

                // 根据类型决定如何解析
                switch (type)
                {
                    case "ModelData":
                        ModelData commands1 = JsonUtility.FromJson<ModelData>(json);
                        Debug.Log($"Parsed Person: {commands1.modelName}, {commands1.position_position}");
                        if (instance.httptInstance != null)
                        {
                            instance.mainThreadActions.Enqueue(() => instance.httptInstance.GenerateModel(commands1));
                        }
                        break;

                    case "Modelmove":
                        Modelmove commands2 = JsonUtility.FromJson<Modelmove>(json);
                        Debug.Log($"Parsed Product: {commands2.modelName}, {commands2.position_position}, {commands2.target_target}");
                        if (instance.httptInstance != null)
                        {
                            instance.mainThreadActions.Enqueue(() => instance.httptInstance.Move(commands2));
                        }
                        break;

                    default:
                        Debug.Log($"Unknown type: {type}");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"JSON解析错误: {e.Message}\n{e.StackTrace}");
                Debug.LogError($"问题JSON: {json}");
            }
        }
    }
}
