using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Animator animator;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f; // 调整为更合理的跑步速度
    public float gravity = -9.81f; // 重力加速度
    public float rotationSpeed = 720f; // 每秒旋转角度
    public float speedSmoothTime = 0.1f; // 速度平滑过渡时间

    private float currentSpeed; // 当前速度
    private float speedSmoothVelocity; // 速度平滑辅助变量
    private Vector3 moveDirection; // 移动方向
    private Vector3 velocity; // 包含重力的速度向量

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();   
        animator = GetComponent<Animator>();    
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move() 
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // 使用平滑输入
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 计算目标速度
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // 平滑过渡速度
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // 应用重力
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f; // 角色在地面时，重置Y轴速度
        }

        // 计算移动速度
        Vector3 moveVelocity = moveDirection * currentSpeed;

        // 应用移动
        controller.Move((moveVelocity + velocity) * Time.deltaTime);

        // 平滑旋转角色朝向
        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // 更新动画
        if (animator != null)
        {
            animator.SetFloat("Speed", moveVelocity.magnitude);
        }
    }



}
