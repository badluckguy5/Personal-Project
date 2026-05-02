using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private InputSystem_Actions controls;

    public Vector2 Movement { get; private set; }

    public event System.Action OnJump;
    public event System.Action OnAttack;
    public event System.Action OnSprint;
    public event System.Action InventoryToggle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        controls = new InputSystem_Actions();
        controls.Enable();

        controls.Player.Move.performed += ctx => Movement = ctx.ReadValue<Vector2>();

        controls.Player.Move.canceled += ctx => Movement = Vector2.zero;

        controls.Player.Jump.performed += ctx => OnJump?.Invoke();

        controls.Player.Attack.performed += ctx => OnAttack?.Invoke();

        controls.Player.Sprint.performed += ctx => OnSprint?.Invoke();

        controls.Player.ToggleInventory.performed += ctx => InventoryToggle?.Invoke();
    }


    private void OnDestroy()
    {
        controls?.Dispose();
    }

    private void Update()
    {
        
    }

}
