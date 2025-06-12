using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody _rb;
    public InputActionAsset inputActions;
    private InputAction _move;
    private InputAction _jump;
    private bool isGrounded = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        var actionMap = inputActions.FindActionMap("Player");
        _move = actionMap.FindAction("Move");
        _jump = actionMap.FindAction("Jump");

        _move.Enable();
        _jump.Enable();
    }

    void OnDisable()
    {
        _move.Disable();
        _jump.Disable();
    }
    

    void Update()
    {
        if (PlayerSwitch.Instance == null) return;

        var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();
        if (activeCharacter != null && activeCharacter.controller == this)
        {
            Move();
        }
        if (_jump.triggered && isGrounded)
            TryJump();
    }

    private void TryJump()
    {
        if (!isGrounded) return;

        var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();
        if (activeCharacter != null && activeCharacter.controller == this)
        {
            _rb.AddForce(Vector3.up * activeCharacter.jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void Move()
    {
        var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();
        Vector2 moveInput = _move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime * activeCharacter.speed;
        _rb.MovePosition(_rb.position + movement);
    }
    

    private void OnCollisionEnter(Collision other)
    {
        // Only consider collisions with "ground"
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}