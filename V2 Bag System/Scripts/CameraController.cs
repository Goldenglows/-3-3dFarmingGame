using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; 
    public float distance = 5f; //摄像机与玩家的距离
    public float height = 2f; //摄像机高度偏移
    public float mouseSensitivity = 100f; //鼠标灵敏度
    public float pitchMin = -30f; //最小俯仰角
    public float pitchMax = 60f; //最大俯仰角

    private float yaw = 0f; //水平旋转角度
    private float pitch = 0f; //垂直旋转角度

    void Start()
    {
        //初始化角度
        yaw = target.eulerAngles.y;
        pitch = 15f; 
    }

    void LateUpdate()
    {
        //获取鼠标输入
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //限制俯仰角
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        //计算摄像机位置
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 position = target.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        //设置摄像机位置和朝向
        transform.position = position;
        transform.LookAt(target.position + Vector3.up * 1f); 
    }
}
