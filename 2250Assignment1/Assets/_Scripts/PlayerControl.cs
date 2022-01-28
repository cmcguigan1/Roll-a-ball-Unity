using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //player rigidbody
    private Rigidbody rb;
    //holds number of pickups collected
    private int counter;
    private Vector3 kickback;
    //holds the score  of the player
    private int total_Points;

    //All the private variables associated with the coins (the pickup objects)
    //array holds the positions the pickup objects will generate
    private Vector3[] coins_Positions = {new Vector3(-16.9f,1.0f,16.7f),new Vector3(7.43f,1f,11.6f),new Vector3(-16.7f,1f,-8.4f),new Vector3(15.8f,1f,-5.6f),
        new Vector3(-8.7f,1f,7.7f),new Vector3(6.6f,1f,0.6f),new Vector3(-8.85f,1f,-17.3f),new Vector3(15.3f,1f,-15.8f),
        new Vector3(17.6f,1.5f,17.4f),new Vector3(15.2f,1.5f,4.2f),new Vector3(4.2f,1.5f,-9.6f),new Vector3(-5.6f,1.5f,-3.33f),new Vector3(-16.5f,1.5f,2.6f)};
    //will hold all the pcikup objects after instantaition and after their positions are set
    private GameObject[] coins = new GameObject[13];
    //holds value of first game to see if the pickups need to be initialized
    private bool isFirstGame = true;
    //holds the GameObjects of the pickups
    private GameObject cube;
    private GameObject capsule;
    private GameObject cylinder;

    //prefabs of the pcikups
    public GameObject cubePrefabVar;
    public GameObject capsulePrefabVar;
    public GameObject cylinderPrefabVar;

    //text values displayed to player
    public Text score;
    public Text gameOver;

    // Start is called before the first frame update
    void Start()
    {
        //initializes and resets values
        rb = GetComponent<Rigidbody>();
        counter = 0;
        total_Points = 0;
        SetScore(0);
        gameOver.text = "";
        //checks if its the first game and the pickups need to be initialized
        if (isFirstGame)
        {
            InitializeCoins();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //controls player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal,0.0f,vertical);
        rb.AddForce(move * 11);
    }


    //responsible for handling the triggers with the pickups
    void OnTriggerEnter(Collider other)
    {
        //increments the score with different values depending on the pickup triggered
        if (other.gameObject.CompareTag("Coin1"))
        {
            other.gameObject.SetActive(false);
            counter+=1;
            SetScore(1);
        }
        if (other.gameObject.CompareTag("Coin2"))
        {
            other.gameObject.SetActive(false);
            counter += 1;
            SetScore(2);
        }
        if (other.gameObject.CompareTag("Coin3"))
        {
            other.gameObject.SetActive(false);
            counter += 1;
            SetScore(3);
        }
        //checks if all the pickups have been collected
        if (counter >= 13)
        {
            //displays game over to player
            gameOver.text = "Game Over";
            //invokes the method to reactivate the pickups in 5 seconds
            Invoke("ReactivateCoins", 5.0f);
        }

    }

    //sets the score of the player for collecting pickups
    void SetScore(int pointValue)
    {
        total_Points += pointValue; 
        score.text = "Score: " + total_Points.ToString();
    }

    //handles collisions with the walls to ensure ball bounces off the wall with the relative velocity it hit it with
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall") //have to set each wall's tag to Wall
        {
            kickback = collision.relativeVelocity;
            rb.velocity = kickback;
        }
    }

    //initializes the pickups the first round of the game
    void InitializeCoins()
    {
        //initializes the pickups with the prefabs and dynamically adds them to the game
        //saves each GameObject into the array "coins" to prevent reinitialization in future rounds
        for(int i = 0;i < 4; i++)
        {
            cylinder = Instantiate(cylinderPrefabVar);
            cylinder.transform.position = coins_Positions[i];
            coins[i] = cylinder;
        }
        for (int i = 4; i < 8; i++)
        {
            cube = Instantiate(cubePrefabVar);
            cube.transform.position = coins_Positions[i];
            coins[i] = cube;
        }
        for (int i = 8; i < 13; i++)
        {
            capsule = Instantiate(capsulePrefabVar);
            capsule.transform.position = coins_Positions[i];
            coins[i] = capsule;
        }
        //sets boolean value to false so the program will no longer enter this method
        isFirstGame = false;
    }

    //reactivates all of the pickups rather than reinitialize all of them over again
    //called right before restaring the game
    void ReactivateCoins()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetActive(true);
        }
        Start();
    }
}
