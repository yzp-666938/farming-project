using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_c : MonoBehaviour
{
    Vector2 borderX = new Vector2(7, 76);
    Vector2 borderZ = new Vector2(-29, 43);
    float movespeed = 50;
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        //左右
        float h = Input.GetAxis("Horizontal");
        //前后
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(-v, 0, h);

        transform.position += dir * Time.deltaTime * movespeed;

        //纠正坐标
        if (transform.position.x > borderX.y)
        {
            transform.position = new Vector3(borderX.y, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < borderX.x)
        {
            transform.position = new Vector3(borderX.x, transform.position.y, transform.position.z);
        }

        if (transform.position.z > borderZ.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,borderZ.y);
        }
        else if (transform.position.z < borderZ.x)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, borderZ.x);
        }
        //鼠标滚轮
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseScrollWheel > 0)
        {
            camera.fieldOfView += mouseScrollWheel * 2 * 5;
        }
        else if (mouseScrollWheel < 0 )
        {
            camera.fieldOfView -= mouseScrollWheel * -2 * 5;
        }
    }
}
