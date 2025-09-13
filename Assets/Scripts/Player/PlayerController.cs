using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float baseSpeed = 5f;          // 基础移动速度
    public float acceleration = 40f;      // 加速度
    public float jumpForce = 10f;         // 跳跃力度
    public float gravity = 20f;           // 自定义重力

    [Header("旋转参数")]
    public float rotationSmoothTime = 0.14f; // 平滑旋转时间

    [Header("引用")]
    public Transform MainCameraTransform { get; private set; }

    public InputSystem_Actions inputSystem { get; private set; }
    public InputSystem_Actions.PlayerActions playerActions { get; private set; }
    public CharacterController characterController { get; private set; }

    // 输入相关
    private Vector2 moveInput;
    private bool jumpPressed;

    // 移动相关
    private float currentSpeed;
    private float speedModifier = 1f;

    // 旋转相关
    private float rotationVelocity;
    protected Vector3 currentTargetRotation;
    protected Vector3 timeToReachTargetRotation;
    protected Vector3 dampedTargetRotationCurrentVelocity;
    protected Vector3 dampedTargetRotationPassedTime;

    // 跳跃 & 重力
    private bool isGrounded;
    private float yVelocity;
    private float groundCheckDistance = 0.2f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputSystem = new InputSystem_Actions();
        playerActions = inputSystem.Player;

        // 自动获取相机引用
        if (MainCameraTransform == null)
        {
            if (Camera.main != null)
                MainCameraTransform = Camera.main.transform;
            else
                Debug.LogWarning("⚠ PlayerController: 没有找到 MainCamera，相机引用为空！");
        }
    }

    void Update()
    {
        // 内置落地检测
        isGrounded = characterController.isGrounded;

        HandleInput();
        Move();

        ApplyGravity();
        Jump();
    }

    private void HandleInput()
    {
        moveInput = playerActions.Move.ReadValue<Vector2>();

        if (playerActions.Jump.triggered)
        {
            jumpPressed = true;
        }
    }

    private void Move()
    {
        if (moveInput == Vector2.zero || speedModifier == 0f)
            return;

        // 输入方向（本地空间）
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        float targetRotationYAngle = Rotate(moveDir);
        Vector3 targetRotationDirection = Quaternion.Euler(0f, targetRotationYAngle, 0f) * Vector3.forward;

        // 目标速度
        float targetSpeed = baseSpeed * speedModifier;

        // 插值平滑速度
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // 组合移动向量（带重力）
        Vector3 move = targetRotationDirection * currentSpeed;
        move.y = yVelocity; // 垂直方向由重力和跳跃控制

        // 移动
        characterController.Move(move * Time.deltaTime);
    }

    private float Rotate(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (directionAngle < 0f)
            directionAngle += 360f;

        if (MainCameraTransform != null)
            directionAngle += MainCameraTransform.eulerAngles.y;

        if (directionAngle > 360f)
            directionAngle -= 360f;

        if (directionAngle != currentTargetRotation.y)
        {
            currentTargetRotation.y = directionAngle;
            dampedTargetRotationPassedTime.y = 0f;
        }

        float currentYAngle = transform.rotation.eulerAngles.y;

        if (Mathf.Approximately(currentYAngle, currentTargetRotation.y))
            return directionAngle;

        float smoothedYAngle = Mathf.SmoothDampAngle(
            currentYAngle,
            currentTargetRotation.y,
            ref dampedTargetRotationCurrentVelocity.y,
            Mathf.Max(0.01f, rotationSmoothTime)
        );

        dampedTargetRotationPassedTime.y += Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

        return directionAngle;
    }

    private void ApplyGravity()
    {
        if (isGrounded && yVelocity < 0f)
        {
            yVelocity = -2f; // 轻微负值让角色贴地
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (jumpPressed && isGrounded)
        {
            yVelocity = jumpForce;
        }
        jumpPressed = false;
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }
    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
