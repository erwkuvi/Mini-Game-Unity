using System;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private Renderer rend;
    public GameObject door1;
    public GameObject movingPlatform;
    private MovingPlatform movingPlatformScript;
    private Rigidbody rbPlatform;
    private bool shouldMove = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material m_Blue;
    public Material m_Red;
    public Material m_Yellow;
    public float movingSpeed;
    public string movingAxis;
    public float movingDistance;
    

    void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        rend = GetComponent<Renderer>();
        
        movingPlatformScript = movingPlatform.GetComponent<MovingPlatform>();
        rbPlatform = movingPlatform.GetComponent<Rigidbody>();
        Debug.Log("Materials " + rend.material.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Material other_material = other.gameObject.GetComponent<Renderer>().material;
        if (movingPlatform.layer == LayerMask.NameToLayer("Blue") && rend.material.name.Contains("white"))
        {
            startPosition = movingPlatform.transform.position;
            endPosition = startPosition + new Vector3(movingDistance, 0, 0);
            shouldMove = true;
            Debug.Log("Here in movingPlatform");
            Debug.Log("Material " + rend.material.name);
            
            return;
        }
        if (other_material.name.Contains("Claire") && other.tag == "Player")
        {
            Debug.Log("Entered " + other_material.name);
            rend.material = m_Blue;
            door1.layer = LayerMask.NameToLayer("Blue");
            door1.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Claire_Platforms_Colour");
        }
        else if (other_material.name.Contains("John") && other.tag == "Player")
        {
            rend.material = m_Yellow;
            door1.layer = LayerMask.NameToLayer("Yellow");
            door1.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/John_Platforms_Colour");
        }
        else if (other_material.name.Contains("Thomas") && other.tag == "Player")
        {
            rend.material = m_Red;
            if (door1.activeSelf)
            {
                door1.layer = LayerMask.NameToLayer("Red");
                door1.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Thomas_Platforms_Colour");
            }

            if (!movingPlatformScript) return;
            movingPlatformScript.speed = movingSpeed;
            movingPlatformScript.axis = movingAxis;
            movingPlatformScript.distance = movingDistance;
        }
            
            //Assign other_material to this
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (shouldMove)
        {
            movingPlatform.transform.position = Vector3.MoveTowards(movingPlatform.transform.position, endPosition, movingSpeed * Time.deltaTime);

            if (Vector3.Distance(movingPlatform.transform.position, endPosition) < 0.01f)
                shouldMove = false; // Stop moving once target is reached
        }
    }

    void Update()
    {
        
        
    }
}
