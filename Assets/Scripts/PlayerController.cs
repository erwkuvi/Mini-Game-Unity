using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody _rb;
    public InputActionAsset inputActions;
    private InputAction _move;
    private InputAction _jump;
    private bool _isGrounded = true;

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
            Move();
        if (_jump.triggered && _isGrounded)
            TryJump();
        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void TryJump()
    {
        if (!_isGrounded) return;

        var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();
        if (activeCharacter != null && activeCharacter.controller == this)
        {
            _rb.AddForce(Vector3.up * activeCharacter.jumpForce, ForceMode.Impulse);
            _isGrounded = false;
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
<<<<<<< Updated upstream
        HandleGroundCheck(other);
=======
        // Only consider collisions with "ground"
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = true;
>>>>>>> Stashed changes
    }

private void OnCollisionStay(Collision other)
{
    HandleGroundCheck(other);
}

private void HandleGroundCheck(Collision other)
{
    var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();

    // Allow collision with the ground
    if (other.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
        return;
    }

    // Allow collision with own platform
    if (other.gameObject.CompareTag(activeCharacter.uniquePlayerPlatform))
    {
        isGrounded = true;
        return;
    }

    // ✨ NEW: Allow collision with other players (don't ignore)
    if (other.gameObject.CompareTag("Player"))
    {
        isGrounded = true;
        return;
    }

    // Ignore other platforms
    Physics.IgnoreCollision(_rb.GetComponent<Collider>(), other.collider);
}


}