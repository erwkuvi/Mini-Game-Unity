using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerSwitch.Character character;

    public float speed;
    private Rigidbody _rb;
    public InputActionAsset inputActions;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _reset;
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
        _reset = actionMap.FindAction("ResetGame");

        _reset.Enable();
        _move.Enable();
        _jump.Enable();
    }

    void OnDisable()
    {
        _reset.Disable();
        _move.Disable();
        _jump.Disable();
    }

    void Update()
    {
        if (PlayerSwitch.Instance == null || character.hasFinishStage) return;

        var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();
        if (activeCharacter != null && activeCharacter.controller == this)
            Move();

        if (_jump.triggered && _isGrounded)
            TryJump();

        if (_reset.triggered)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        CheckFinishCondition();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") ||
            other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag(character.uniquePlayerPlatform))
        {
            _isGrounded = false;
        }
    }

    private void CheckFinishCondition()
    {
        GameObject finishObj = GameObject.FindWithTag(character.uniquePlayerFinish);
        if (finishObj == null) return;

        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 finishPos = new Vector2(finishObj.transform.position.x, finishObj.transform.position.y);

        float distance = Vector2.Distance(playerPos, finishPos);
        float playerWidth = GetComponent<Collider>().bounds.size.x;

        if (distance < playerWidth / 2f)
        {
            character.hasFinishStage = true;
            transform.position = finishObj.transform.position;
            _rb.linearVelocity = Vector3.zero;
            _rb.isKinematic = true;
            Debug.Log($"{character.name} has finished!");

        }
    }


    private void TryJump()
    {
        if (!_isGrounded) return;

        var activeCharacter = PlayerSwitch.Instance.GetActiveCharacter();
        if (activeCharacter != null && activeCharacter.controller == this)
        {
            _rb.AddForce(Vector3.up * character.jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void Move()
    {
        Vector2 moveInput = _move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime * character.speed;
        _rb.MovePosition(_rb.position + movement);
    }

    private void OnCollisionEnter(Collision other)
    {
        HandleGroundCheck(other);
    }

    private void OnCollisionStay(Collision other)
    {
        HandleGroundCheck(other);
    }


private void HandleGroundCheck(Collision other)
{
    // Check if this is a finish object
    if (other.gameObject.tag.StartsWith("Finish_"))
    {
        // Always ignore collisions with finish zones
        Collider ownCollider = _rb.GetComponent<Collider>();
        Collider otherCollider = other.collider;

        if (ownCollider != null && otherCollider != null)
            Physics.IgnoreCollision(ownCollider, otherCollider);
        
        return;
    }

    // Get the angle between contact normal and Vector3.up
    foreach (ContactPoint contact in other.contacts)
    {
        float angle = Vector3.Angle(contact.normal, Vector3.up);

        // If it's a fairly horizontal surface (standable)
        if (angle < 45f)
        {
            if (other.gameObject.CompareTag("Ground") ||
                other.gameObject.CompareTag("Player") ||
                other.gameObject.CompareTag(character.uniquePlayerPlatform))
            {
                _isGrounded = true;
                return;
            }
        }
    }

    // Ignore other players’ platforms if not standable
    if (!other.gameObject.CompareTag("Ground") &&
        !other.gameObject.CompareTag("Player") &&
        !other.gameObject.CompareTag(character.uniquePlayerPlatform) &&
        other.gameObject.tag.EndsWith("_platform")) // player-specific platform
    {
        Collider ownCollider = _rb.GetComponent<Collider>();
        Collider otherCollider = other.collider;

        if (ownCollider != null && otherCollider != null)
            Physics.IgnoreCollision(ownCollider, otherCollider);
    }
}


}
