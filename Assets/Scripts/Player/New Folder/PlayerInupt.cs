using UnityEngine;

public class PlayerInupt : MonoBehaviour
{
    public InputSystem_Actions inputSystem { get; private set; }
    public InputSystem_Actions.PlayerActions playerActions { get; private set; }

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        playerActions = inputSystem.Player;
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