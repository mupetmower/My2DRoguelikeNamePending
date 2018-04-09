using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Player : MovingObject
{

	public float restartLevelDelay = .001f;


	//public Stats Stats { get; set; }
	//public PlayerInventory Inventory { get; set; }

	//public float damageDelt;

	public Text playerHealthText;
	public Text damageText;

    private const int TURNS_FOR_HP_REGEN = 7;


    public Button pickupItemButton;
	public GameObject pickUpItemPanel;

	public GameObject itemUnderPlayer;

	public static bool overStairs = false;

	public static bool overItem = false;
	public static bool itemMenuOpen = false;


	// Use this for initialization
	//Start overrides the Start function of MovingObject
	protected override void Start()
	{

		base.Start();

		Init();
	}


	private void Init()
	{
		//Inventory = GameManager.PlayerInv;
		//Stats = GameManager.PlayerStats;


		//damageDelt = Stats.BaseDamage;

		playerHealthText = GameObject.Find("PlayerHealthText").GetComponent<Text>();

		playerHealthText.text = "Health: " + StatsAndItems.PlayerStats.BaseStats["CurrentHP"] + "/" + StatsAndItems.PlayerStats.BaseStats["MaxHP"];


		pickupItemButton = GameObject.Find("PickUpItemButton").GetComponent<Button>();
		damageText = GameObject.Find("PlayerDamageText").GetComponent<Text>();
		damageText.text = StatsAndItems.PlayerStats.CurrentDamage.ToString();

	}


	//Called once per frame
	private void Update()
	{
		if (GameManager.instance.doingSetup)
			return;

		//If it's not the player's turn, exit the function.
		if (!GameManager.instance.playersTurn)
			return;


		CheckForMovement();

		if (overItem && Input.GetKeyDown(KeyCode.G) && !GameUI.instance.itemMenuOpen)
		{
			LookAtItemsOnGround();
		}

		if (Input.GetKeyDown(KeyCode.B) && overStairs)
		{
			NextLevel();
		}

	}


	public void CheckForMovement()
	{
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.

		//Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER

		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int)(Input.GetAxisRaw("Horizontal"));

		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int)(Input.GetAxisRaw("Vertical"));

		//Check if moving horizontally, if so set vertical to zero.
		if (horizontal != 0)
		{
			vertical = 0;
		}
		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif //End of mobile platform dependendent compilation section started above with #elif



		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0)
		{
			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove<Enemy>(horizontal, vertical);
		}
	}


	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove<T>(xDir, yDir);

		RegenHealth();


		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		GameManager.instance.playersTurn = false;

		//Global.turnCount += 1;
	}


	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove<T>(T component)
	{
		////Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		//animator.SetTrigger("playerChop");


		Enemy hitEnemy = component as Enemy;

		hitEnemy.DamageEnemy(StatsAndItems.PlayerStats.CalculateCurrentDamage());
	}


	public void LookAtItemsOnGround()
	{

		GameUI.instance.ShowPickUpItemMenu();
	}


	private void NextLevel()
	{
		//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
		Invoke("Restart", restartLevelDelay);

		//Disable the player object since level is over.
		enabled = false;
	}



	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Stairs Down"))
		{
			overStairs = true;
		}

		if (other.CompareTag("PickupItem"))
		{
			GameUI.instance.currentListItems.Add(other.GetComponent<Item>());
			overItem = true;
			itemUnderPlayer = other.gameObject;
		}

	}



	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Stairs Down"))
		{
			overStairs = false;
		}

		if (other.CompareTag("PickupItem"))
		{
			GameUI.instance.currentListItems.Clear();
			GameUI.instance.ClosePickUpItemMenu();
			overItem = false;
			itemUnderPlayer = null;
		}
	}


	public void TakeItem(Item item, Button button)
	{
		//var item = hit.transform.gameObject.GetComponent<ListItem>().ItemInList;
		if (typeof(Item).IsAssignableFrom(typeof(Weapon)))
		{
			//StatsAndItems.PlayerInventory.EquippedWeapon = ((Weapon)item);
			StatsAndItems.PlayerInventory.EquippedWeapon = ((Weapon)item);
			//GameManager.PlayerInv.EquippedWeapon = (Weapon)item;

			//Stats.WeaponDamage = ((Weapon)item).BaseDamage;

			damageText.text = StatsAndItems.PlayerStats.CalculateCurrentDamage().ToString();

			button.gameObject.SetActive(false);
			itemUnderPlayer.SetActive(false);
			Destroy(itemUnderPlayer);

			GameManager.instance.playersTurn = false;

		}

	}


	public void DamagePlayer(float damage)
	{
		StatsAndItems.PlayerStats.BaseStats["CurrentHP"] -= damage;

		UpdateHealthText();

		CheckIfGameOver();
	}


	public void RegenHealth()
	{
		if (StatsAndItems.PlayerStats.BaseStats["CurrentHP"] < StatsAndItems.PlayerStats.BaseStats["MaxHP"])
		{
			if (Global.turnCount % TURNS_FOR_HP_REGEN == 0)
			{
				StatsAndItems.PlayerStats.BaseStats["CurrentHP"] += Mathf.Clamp(StatsAndItems.PlayerStats.BaseStats["HPRegen"], 0.0f, StatsAndItems.PlayerStats.BaseStats["MaxHP"] - StatsAndItems.PlayerStats.BaseStats["CurrentHP"]);
				UpdateHealthText();				
			}
		}
	}


	private void UpdateHealthText()
	{
		playerHealthText.text = "Health: " + StatsAndItems.PlayerStats.BaseStats["CurrentHP"] + "/" + StatsAndItems.PlayerStats.BaseStats["MaxHP"];
	}


	private void CheckIfGameOver()
	{
		//Check if food point total is less than or equal to zero.
		if (StatsAndItems.PlayerStats.BaseStats["CurrentHP"] <= 0)
		{
			////Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
			//SoundManager.instance.PlaySingle(gameOverSound);

			////Stop the background music.
			//SoundManager.instance.musicSource.Stop();

			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver();
		}
	}


	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable()
	{
		//When Player object is disabled(between levels), store the current local stats in the GameManager so they can be re-loaded in next level.
		//GameManager.PlayerStats.BaseStats["MaxHP"] = StatsAndItems.PlayerStats.BaseStats["MaxHP"];
		//GameManager.PlayerStats.BaseStats["CurrentHP"] = StatsAndItems.PlayerStats.BaseStats["CurrentHP"];
		//GameManager.PlayerStats.BaseStats["MaxMana"] = StatsAndItems.PlayerStats.BaseStats["MaxMana"];
		//GameManager.PlayerStats.BaseStats["CurrentMana"] = StatsAndItems.PlayerStats.BaseStats["CurrentMana"];
	}




	//Restart reloads the scene when called.
	private void Restart()
	{
		//Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
		//and not load all the scene object in the current scene.
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}


}
