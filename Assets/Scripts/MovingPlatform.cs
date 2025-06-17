using UnityEngine;

/*
public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public string axis;

    public float distance;
    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        if (axis == "x")
            transform.position = startPosition + new Vector3(offset, 0, 0);
        else if (axis == "y")
            transform.position = startPosition + new Vector3(0, offset, 0);

    }
}
*/

/*
public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public string axis;
    public float distance;

    private Vector3 startPosition;
    private Rigidbody _rb;

    void Start()
    {
        startPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        Vector3 newPos = startPosition;

        if (axis == "x")
            newPos += new Vector3(offset, 0, 0);
        else if (axis == "y")
            newPos += new Vector3(0, offset, 0);

        _rb.MovePosition(newPos);
    }
}
*/

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public string axis;
    public float distance;

    private Vector3 startPosition;
    private Rigidbody _rb;
    private Vector3 _lastPosition;
    private Vector3 _velocity;

    void Start()
    {
        startPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        Vector3 newPos = startPosition;

        if (axis == "x")
            newPos += new Vector3(offset, 0, 0);
        else if (axis == "y")
            newPos += new Vector3(0, offset, 0);

        _velocity = (newPos - _lastPosition) / Time.fixedDeltaTime;  // 🛠️ Fix: calculate velocity before MovePosition
        _lastPosition = newPos;

        _rb.MovePosition(newPos);
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}