using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private Renderer rend;
    public GameObject door1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material m_Blue;
    public Material m_Red;
    public Material m_Yellow;

    void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        rend = GetComponent<Renderer>();
        Debug.Log("Materials " + rend.material.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Material other_material = other.gameObject.GetComponent<Renderer>().material;
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
            door1.layer = LayerMask.NameToLayer("Red");
            door1.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Thomas_Platforms_Colour");
            
        }
            
            //Assign other_material to this
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
