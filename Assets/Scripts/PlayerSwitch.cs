using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSwitch : MonoBehaviour
{
    public InputActionAsset inputActions;
    public static PlayerSwitch Instance { get; private set; }  // Singleton pattern

    private InputAction moveRightAction;
    public class Character
    {
        public string name;
        public float speed;
        public float jumpForce;
        public PlayerController controller;
        public string uniquePlayerPlatform;
        public string uniquePlayerFinish;
        public bool hasFinishStage;

        public Character(string name, float speed, float jumpForce, PlayerController controller)
        {
            this.name = name;
            this.speed = speed;
            this.jumpForce = jumpForce;
            this.controller = controller;
            this.uniquePlayerPlatform = name + "_platform";
            this.uniquePlayerFinish =  "Finish_" + name;
            this.hasFinishStage = false;
            
        }
    }

    public PlayerController claireController;
    public PlayerController thomasController;
    public PlayerController johnController;

    private Character claire;
    private Character thomas;
    private Character john;
    
    public Character activeCharacter;

    private InputAction switchToClaireAction;
    private InputAction switchToThomasAction;
    private InputAction switchToJohnAction;

	public Character[] characters;

    void OnEnable()
    {
        var actionMap = inputActions.FindActionMap("PlayerSwitch");

        moveRightAction = actionMap.FindAction("MoveRight");
        switchToClaireAction = actionMap.FindAction("SwitchToClaire");
        switchToThomasAction = actionMap.FindAction("SwitchToThomas");
        switchToJohnAction = actionMap.FindAction("SwitchToJohn");

        moveRightAction.Enable();
        switchToClaireAction.Enable();
        switchToThomasAction.Enable();
        switchToJohnAction.Enable();
    }

    void OnDisable()
    {
        moveRightAction.Disable();
        switchToClaireAction.Disable();
        switchToThomasAction.Disable();
        switchToJohnAction.Disable();
    }


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initialize characters
        claire = new Character("Claire", 0.2f, 15.0f, claireController);
        thomas = new Character("Thomas", 1.0f, 12.0f, thomasController);
        john = new Character("John", 5.0f, 30.0f, johnController);
        activeCharacter = claire;
		characters = new Character[3];
		characters[0] = claire;
		characters[1] = thomas;
		characters[2] = john;

        claire.controller.character = claire;
        thomas.controller.character = thomas;
        john.controller.character = john;

    }

	public Character[] GetOtherCharacter()
	{
		return characters;
	}

    public Character GetActiveCharacter()
    {
        return activeCharacter;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (switchToClaireAction.triggered)
            activeCharacter = claire;
        if (switchToThomasAction.triggered)
            activeCharacter = thomas;
        if (switchToJohnAction.triggered)
            activeCharacter = john;
        
        if (claire.hasFinishStage && john.hasFinishStage && thomas.hasFinishStage)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
