using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttoncontronl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public delegate void OnButtonClick(int value);
    public static event OnButtonClick OnButtonClicked;

    // 按钮点击时调用的方法
    public void ReturnValueOnClick(int value)
    {
        // 触发事件，传递int值
        OnButtonClicked?.Invoke(value);
    }

}
