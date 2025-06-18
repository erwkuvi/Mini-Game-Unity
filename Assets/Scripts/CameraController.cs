using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerSwitch playerSwitch;

    private Vector3 _offset;
    private float _lowBound = -4.0f;

    void Start()
    {
        if (playerSwitch == null || playerSwitch.activeCharacter == null)
        {
            Debug.LogError("PlayerSwitch or activeCharacter not assigned!");
            return;
        }

        transform.position = playerSwitch.activeCharacter.controller.transform.position;

        _offset = new Vector3(0, 0, -5);
    }

    void LateUpdate()
    {
        if (playerSwitch.activeCharacter != null)
        {
            if (playerSwitch.activeCharacter.controller.transform.position.y > _lowBound)
                transform.position = playerSwitch.activeCharacter.controller.transform.position + _offset;
            else
            {
                Debug.Log("Gone");
            }
        }
        
    }
}
