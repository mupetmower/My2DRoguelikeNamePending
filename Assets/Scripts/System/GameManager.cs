using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    private BoardCreator boardCreator;                      //Hold reference to boardManager to call InitBoard

    
    public Node[][] gridNodes;

    public bool playersTurn = true;

    //public float turnDelay = 0.001f;                    //turn delay per enemy in enemies.Count

    public static int floor = 0;


    //player stats
    //public static Stats PlayerStats { get; set; }
    //public static PlayerInventory PlayerInv { get; set; }

    private List<Enemy> enemies;
    private bool enemiesMoving;

    public bool doingSetup = false;


    //Awake is always called before any Start functions
    void Awake()
    {   
        //Enforce singelton pattern, so there will always only ever be one GameManager
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else if (instance != this)                //If instance already exists and it's not this:
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        
        //Assign enemies to a new List of Enemy objects.
        enemies = new List<Enemy>();

        
    }


    //this is called only once, and the paramter tells it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    //This is called each time a scene is loaded. Could also use OnLevelWasLoaded
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        

        if (arg0.name.Equals("Main"))
        {
            instance.enabled = true;
            floor++;
            instance.InitGame();
        }

    }

    
    void InitGame()
    {
        doingSetup = true;
        Global.turnCount = 0;

        enemies.Clear();        

        boardCreator = GetComponent<BoardCreator>();

        GameUI.instance.FindAndInitUI();


        boardCreator.InitBoard(floor);

        gridNodes = boardCreator.GetNodes();

    }


    //Update is called every frame.
    void Update()
    {

        if (Global.turnCount >= 9999)
            Global.turnCount = 0;

        

        if (playersTurn || enemiesMoving || doingSetup || GameUI.instance.itemMenuOpen || GameUI.instance.statsMenuOpen)
            return;


        //Start moving enemies.
        StartCoroutine(MoveEnemies());

    }


    //Call this to add the passed in Enemy to the List of Enemy objects.
    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies.
        enemies.Add(script);
    }


    //Coroutine to move enemies in sequence.
    IEnumerator MoveEnemies()
    {

        //While enemiesMoving is true player is unable to move.
        enemiesMoving = true;

       
        //Loop through List of Enemy objects.
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                //Call the MoveEnemy function of Enemy at index i in the enemies List.
                enemies[i].MoveEnemy();

            }
            //Wait a small amount of time before moving next enemy, so they can't move to the same position and overlap.
            //This works better here because now it will loop through the entire enemy list  each time and wait, even if the enemy is disabled(dead).
            yield return new WaitForSecondsRealtime(.001f);             //This takes much longer if using a variable of same value(.001f).
            

        }

       
        //yield return null;


        //Enemies are done moving, set enemiesMoving to false.
        enemiesMoving = false;

        //Once Enemies are done moving, set playersTurn to true so player can move.
        playersTurn = true;

        Global.turnCount += 1;

    }



    public void GameOver()
    {

        GameUI.instance.GameOver();
        enabled = false;

    }



}
