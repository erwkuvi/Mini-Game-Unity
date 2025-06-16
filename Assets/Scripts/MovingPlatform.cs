using UnityEngine;

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
