using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {
    
    public static GameManager instance = null;
    private BoardManager boardScript;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each scene.
    void InitGame()
    {
        boardScript.SetupScene();
    }

    //Update is called every frame.
    void Update()
    {

    }
}

