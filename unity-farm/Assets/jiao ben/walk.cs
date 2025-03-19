using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour
{
    public Transform targetPosition; // 目标位置
    public float moveDuration = 6.0f; // 移动持续时间
    private float elapsedTime; // 已经过去的时间
    private Vector3 startPosition; // 起始位置

    void Start()
    {
        startPosition = transform.position; // 记录起始位置
    }

    void Update()
    {
        if (elapsedTime < moveDuration)
        {
            // 插值计算当前位置
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime; // 累加时间
        }
        else
        {
            // 确保最终到达目标位置
            transform.position = targetPosition.position;
        }
        
    }
}
