using System;
using UnityEngine;

/// <summary>
/// 玩家移动状态基类
/// 实现了角色的移动、旋转逻辑（例如原神角色的移动方式）
/// </summary>
public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine stateMachine; // 状态机引用，用于访问 Player 的属性和组件
    protected Vector2 movementInput;                   // 玩家输入的2D向量（WASD 或 摇杆输入）

    protected float baseSpeed = 5f;        // 基础移动速度
    protected float speedModifier = 1f;    // 速度倍率（可用于加速、减速、行走模式）

    // 旋转相关字段

    protected Vector3 currentTargetRotation;  // 当前目标旋转方向（用于记录当前旋转方向，避免重复计算）
    protected Vector3 timeToReachTargetRotation; // 记录到达目标旋转的时间（可用于平滑过渡）
    protected Vector3 dampedTargetRotationCurrentVelocity; // 用于平滑旋转的速度缓存变量
    protected Vector3 dampedTargetRotationPassedTime; // 平滑旋转的目标方向

    public PlayerMovementState(PlayerMovementStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        Initialize(); // 初始化状态相关变量
    }

    private void Initialize()
    {
        // 初始化旋转相关变量
        timeToReachTargetRotation.y = 0.14f;
    }

    #region IState Methods
    public virtual void Enter()
    {
        Debug.Log("PlayerMovementState: Enter " + GetType().Name);
    }

    public virtual void Exit()
    {
        // 离开该状态时执行（此处可扩展）
    }

    public virtual void HandleInput()
    {
        // 读取玩家输入（WASD/摇杆）
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        // 在 FixedUpdate 中执行物理移动
        Move();
    }

    public virtual void Update()
    {


    }
    #endregion

    #region Main Methods
    /// <summary>
    /// 从 Input System 读取玩家移动输入（2D 向量）
    /// </summary>
    private void ReadMovementInput()
    {
        movementInput = stateMachine.Player.playerInput.playerActions.Move.ReadValue<Vector2>();
    }

    /// <summary>
    /// 移动逻辑：根据输入方向对刚体施加力
    /// </summary>
    private void Move()
    {
        // 如果没有输入 或者 速度倍率为 0，则不移动
        if (movementInput == Vector2.zero || speedModifier == 0f)
        {
            return;
        }

        // 1. 获取移动方向
        Vector3 movementDirection = GetMovementDirection();

        // 如果有输入，则执行旋转逻辑
        float targetRotationYAngle = Rotate(movementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        // 2. 计算实际移动速度
        float movementSpeed = GetMovementSpeed();

        // 3. 获取玩家当前的水平速度（忽略Y轴）
        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        // 4. 用 AddForce 施加力，修正当前速度 → 保证恒定的移动速度
        stateMachine.Player.Rigidbody.AddForce(
            movementSpeed  * targetRotationDirection - currentPlayerHorizontalVelocity,
            ForceMode.VelocityChange);


    }

    /// <summary>
    /// 执行旋转逻辑，使玩家朝向输入方向
    /// </summary>
    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);

        RotateTowardsTargetRotation();

        return directionAngle;
    }
    #endregion

    #region Reusable Methods

    /// <summary>
    /// 计算并更新目标旋转角度
    /// 可选择是否考虑摄像机方向
    /// </summary>
    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        // 1. 获取输入方向对应的角度
        float directionAngle = GetDirectionAngle(direction);

        // 2. 加上摄像机角度，使移动方向与相机朝向一致
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }


        if (directionAngle != currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }


        return directionAngle;
    }

    /// <summary>
    /// 将方向向量转换为角度（0 ~ 360°）
    /// </summary>
    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // 转换为正角度
        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    /// <summary>
    /// 将相机的Y轴角度加到方向角度上
    /// 确保角色移动方向与相机一致
    /// </summary>
    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    /// <summary>
    /// 平滑地旋转到目标角度（避免瞬间转向）
    /// </summary>
    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;

        if (currentYAngle == currentTargetRotation.y)
        {
            return;
        }
        // 平滑插值到目标角度
        float smoothedYAngle = Mathf.SmoothDampAngle(
            currentYAngle,
            currentTargetRotation.y,
            ref dampedTargetRotationCurrentVelocity.y,
            timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);

        dampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        // 更新刚体的旋转
        stateMachine.Player.Rigidbody.MoveRotation(targetRotation);

    }

    /// <summary>
    /// 获取旋转角度对应的方向向量
    /// </summary>
    protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
    {
        return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
    }

    /// <summary>
    /// 获取玩家的水平速度（忽略Y轴）
    /// </summary>
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.linearVelocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
    }


    /// <summary>
    /// 将二维输入向量（x, y）转换为三维方向向量
    /// </summary>
    protected Vector3 GetMovementDirection()
    {
        return new Vector3(movementInput.x, 0f, movementInput.y).normalized;
    }

    /// <summary>
    /// 获取最终的移动速度（基础速度 × 倍率）
    /// </summary>
    private float GetMovementSpeed()
    {
        return baseSpeed * speedModifier;
    }

    /// <summary>
    /// 更新目标旋转角度数据
    /// </summary>
    private void UpdateTargetRotationData(float targetAngle)
    {
        currentTargetRotation.y = targetAngle;
        dampedTargetRotationPassedTime.y = 0f; // 重置平滑旋转的时间
    }
    #endregion
}
