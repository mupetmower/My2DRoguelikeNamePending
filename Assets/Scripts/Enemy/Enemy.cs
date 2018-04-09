using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject {

    public float damageDelt;                            //The amount of health points to subtract from the players total health.

    public float health;

    public int expGiven;

    //public AudioClip attackSound1;                      //First of two audio clips to play when attacking the player.
    //public AudioClip attackSound2;                      //Second of two audio clips to play when attacking the player.


    //private Animator animator;                          //Variable of type Animator to store a reference to the enemy's Animator component.

    private AStarScout scout;

    private Transform target;                           //Transform to attempt to move toward each turn.
    //private bool skipMove;                              //Boolean to determine whether or not enemy should skip a turn or move this turn.
    [SerializeField]
    private int turnsToWait;                            //How many turns to wait between each movements

    public int visibilityRange;

    private bool patrol = false;

    private Direction direction;


    //private BoxCollider2D boxCollider;

    private Text enemyHPText;



    //Start overrides the virtual Start function of the base class.
    protected override void Start()
    {
        //Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
        //This allows the GameManager to issue movement commands.
        GameManager.instance.AddEnemyToList(this);

        //Get and store a reference to the attached Animator component.
        //animator = GetComponent<Animator>();

        //Find the Player GameObject using it's tag and store a reference to its transform component.
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //boxCollider = GetComponent<BoxCollider2D>();


        enemyHPText = GameObject.Find("CurrentEnemyHealthText").GetComponent<Text>();

        scout = new AStarScout((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, false);

        //Call the start function of our base class MovingObject.
        base.Start();
    }


    public void DamageEnemy(float damage)
    {
        health -= damage;

        CheckIfAlive();

        enemyHPText.text = "Enemy HP: " + health;
    }

    private void CheckIfAlive()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
            StatsAndItems.PlayerStats.AddExp(expGiven);

        }
    }

    //Choose a random direction to MoveTo.. Enemy will use this when player is too far away to chase.
    protected virtual void RandomPatrol<T>()
        where T : Component
    {
        int xDir = 0;
        int yDir = 0;

        direction = (Direction)Random.Range(0, 4);
        
        switch (direction)
        {
            case Direction.North:
                xDir = 0;
                yDir = 1;
                break;
            case Direction.East:
                xDir = 1;
                yDir = 0;
                break;
            case Direction.South:
                xDir = 0;
                yDir = -1;
                break;
            case Direction.West:
                xDir = -1;
                yDir = 0;
                break;
        }

        base.AttemptMove<T>(xDir, yDir);

    }




    //Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
    //See comments in MovingObject for more on how base AttemptMove function works.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        
        if (turnsToWait >= 2)
        {
            if (Global.turnCount % turnsToWait == 0)
            {
                return;
            }
        }
        
        
        

        //if (skipMove)
        //{
        //    skipMove = false;
        //    return;
        //}

        
        if ((Mathf.Abs(target.position.x - transform.position.x) > visibilityRange))
        {
            patrol = true;
        }
        if (Mathf.Abs(target.position.y - transform.position.y) > visibilityRange)
        {
            patrol = true;
        }



        if (patrol)
        {
            RandomPatrol<T>();
            patrol = false;
            //skipMove = true;
            return;
        }
        else
        {
            //System.Console.WriteLine("Enemy at: " + (int)gameObject.transform.position.x + ", " + (int)gameObject.transform.position.y + "starting chase to: " + (int)target.position.x + ", " + (int)target.position.y);
            ChasePlayer<T>((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)target.position.x, (int)target.position.y);
        }        

        //Now that Enemy has moved, set skipMove to true to skip next move.
        //skipMove = true;
        
    }



    public void ChasePlayer<T>(int x, int y, int targetX, int targetY)
        where T : Component
    {
        Path pathToPlayer = scout.AStarSearch(x, y, targetX, targetY);

        if (pathToPlayer != null)
            TakeStep<T>(pathToPlayer.GetStepAt(1));
        else
            RandomPatrol<T>();

        //skipMove = true;
    }

    private void TakeStep<T>(Path.Step step)
        where T : Component
    {
        base.AttemptMoveToPos<T>(step.X, step.Y);
    }



    //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        
        AttemptMove<Player>(xDir, yDir);

    }




    //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
    //and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
    protected override void OnCantMove<T>(T component)
    {

        //Declare hitPlayer and set it to equal the encountered component.
        Player hitPlayer = component as Player;

 
        hitPlayer.DamagePlayer(damageDelt);



        //Set the attack trigger of animator to trigger Enemy attack animation.
        //animator.SetTrigger("enemyAttack");

        //Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
        //SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
    }


}
