using UnityEngine;

[RequireComponent(typeof(PlayerInupt))]
public class Player : MonoBehaviour
{
    public PlayerInupt playerInput { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
     
    public Transform MainCameraTransform { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInupt>();
        Rigidbody = GetComponent<Rigidbody>();
        MainCameraTransform = Camera.main.transform;
        movementStateMachine = new PlayerMovementStateMachine(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);

    }

    // Update is called once per frame
    void Update()
    {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }
}
