using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public int CamId;
    public GameObject[] cameras;
    public string[] shotcuts;
    public bool changeAudioListener = true;
    
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        // 订阅事件
        buttoncontronl.OnButtonClicked += HandleButtonClick;
    }

    private void OnDisable()
    {
        // 取消订阅事件
        buttoncontronl.OnButtonClicked -= HandleButtonClick;
    }

    // 处理按钮点击事件
    private void HandleButtonClick(int value)
    {
        CamId = value;
        Debug.Log("Button clicked with value: " + value);
    }

        // Update is called once per frame
        void Update()
    {

        int i = 0;
        for (i = 0; i < cameras.Length; i++)
        {
            if (i == CamId)
            {
                SwitchCamera(i);
            }
        }
    }

    void SwitchCamera(int index)
    {
        int i = 0;
        for (i = 0; i < cameras.Length; i++)
        {
            if (i != index)
            {
                if (changeAudioListener)
                {
                    cameras[i].GetComponent<AudioSource>().enabled = false;
                }
                cameras[i].GetComponent<Camera>().enabled = false;
            }
            else
            {
                if (changeAudioListener)
                {
                    cameras[i].GetComponent<AudioSource>().enabled = true;
                }
                cameras[i].GetComponent<Camera>().enabled = true;
            }
        }
    }
}
