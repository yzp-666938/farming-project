using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WebSocket__Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.Json;
    using static httpt;
    using System.Resources;
    


    //  cs类表示json数据结构


    public class httpt : MonoBehaviour
    {

        void Start()
        {
            //string jsonString = @"
        //{
            //""modelName"": ""farmer"",
            //""position_position"": { ""x"": -22, ""y"": 0, ""z"": 13 },
            //""target_target"": { ""x"": -22, ""y"": 0, ""z"": 30 }
        //}";

        }


        public void GenerateModel(ModelData modelData)
        {
            // 根据modelName加载模型预制体   重点 重点 重点 !!!!!!!!!!!!!!!!!!!!
            GameObject prefab = Resources.Load<GameObject>(modelData.modelName);
            if (prefab == null)
            {
                Debug.LogError($"Prefab '{modelData.modelName}' not found in Resources folder.");
                return;
            }

            GameObject modelInstance = Instantiate(prefab);

            // 设置模型的位置、旋转和缩放
            modelInstance.transform.position = modelData.position_position;
        }

        public void Move(Modelmove modelmove)
        {
            GameObject prefab = Resources.Load<GameObject>(modelmove.modelName);
            if (prefab == null)
            {
                Debug.LogError($"Prefab '{modelmove.modelName}' not found in Resources folder.");
                return;
            }

            GameObject modelInstance = Instantiate(prefab);

            modelInstance.transform.position = modelmove.position_position;
            Vector3 startposition = modelmove.position_position;
            Invoke("moveychi", 4.0f);

            void moveychi()
            {
                modelInstance.transform.position = Vector3.Lerp(startposition, modelmove.target_target, 4.0f);
            }
        }


    }
}
