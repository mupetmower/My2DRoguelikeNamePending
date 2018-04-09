using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {

	//object and classtype for StatsClass Initialization
	private Stats examplePlayer;
	private ClassType classType  = ClassType.Adventurer;

	private PlayerInventory startingInv = new PlayerInventory();

	//the canvas for class menu
	public GameObject classMenuCanvas;
	
	//class toggles
	public Toggle advToggle;
	public Toggle mageToggle;
	public Toggle rogueToggle;

	public Toggle customToggle;


	//Text for stats labels
	public Text healthText;
	public Text manaText;
	public Text vitText;
	public Text strText;
	public Text intText;
	public Text wisText;
	public Text agiText;
	public Text dexText;
	public Text luckText;


	public Text classTypeText;

	public Text statPointsText;

	//plus and minus buttons arrays
	public Button[] plusButtons;
	public Button[] minusButtons;

	private Text[] statTexts = new Text[7];




	//Stats for labels on class menu
	public float playerMaxHealth = 100f;
	
	public float playerMaxMana = 50f;

	public float playerVitality = 5f;
	public float playerStrength = 5f;
	public float playerIntelligence = 5f;
	public float playerWisdom = 5f;
	public float playerAgility = 5f;
	public float playerDexterity = 5f;
	public float playerLuck = 5f;

	public int statPoints = 0;




	// Use this for initialization
	void Start () {
		advToggle = GameObject.Find("AdventurerToggle").GetComponent<Toggle>();
		mageToggle = GameObject.Find("MageToggle").GetComponent<Toggle>();
		rogueToggle = GameObject.Find("RogueToggle").GetComponent<Toggle>();
		customToggle = GameObject.Find("CustomClassToggle").GetComponent<Toggle>();


		classTypeText = GameObject.Find("classTypeText").GetComponent<Text>();
		healthText = GameObject.Find("healthText").GetComponent<Text>();
		manaText = GameObject.Find("manaText").GetComponent<Text>();
		vitText = GameObject.Find("vitText").GetComponent<Text>();
		strText = GameObject.Find("strText").GetComponent<Text>();
		intText = GameObject.Find("intText").GetComponent<Text>();
		wisText = GameObject.Find("wisText").GetComponent<Text>();
		agiText = GameObject.Find("agiText").GetComponent<Text>();
		dexText = GameObject.Find("dexText").GetComponent<Text>();
		luckText = GameObject.Find("luckText").GetComponent<Text>();

		statPointsText = GameObject.Find("statPointsText").GetComponent<Text>();



		statTexts[0] = vitText;
		statTexts[1] = strText;
		statTexts[2] = intText;
		statTexts[3] = wisText;
		statTexts[4] = agiText;
		statTexts[5] = dexText;
		statTexts[6] = luckText;

        startingInv.EquippedWeapon = new Weapon("Dagger", 1, 0);

		classMenuCanvas.SetActive(false);

		examplePlayer = new Stats(classType);

		//playerMaxHealth = examplePlayer.CalculateMaxHealth(playerVitality);
		//playerMaxMana = examplePlayer.CalculateMaxMana(playerIntelligence);

		healthText.text = examplePlayer.BaseStats["MaxHP"].ToString();
		manaText.text = examplePlayer.BaseStats["MaxMana"].ToString();


	}




	public void LoadFirstLevel()
	{

		if (statPoints == 0)
		{
			SceneManager.LoadScene("Main");

			StatsAndItems.PlayerInventory = startingInv;

			//set player's initial stats...
			if (classType == ClassType.Custom)
			{

				StatsAndItems.PlayerStats = new Stats(playerVitality, playerStrength, playerIntelligence, playerWisdom, playerAgility, playerDexterity, playerLuck);

			}
			else
			{
				StatsAndItems.PlayerStats = new Stats(classType);
			}

			
			//GameManager.InitPlayerStats();
			GameManager.floor = 0;

		} else
		{
			UnityEditor.EditorUtility.DisplayDialog("Stats", "Must use all stat points", "OK");
		}
		
	}



	
	public void ShowChooseClassMenu()
	{
		classMenuCanvas.SetActive(true);
	}



	//used for toggle button changes
	public void UpdateStatValues()
	{
		if (advToggle.isOn)
		{
			classType = ClassType.Adventurer;
			statPoints = 0;
			statPointsText.text = statPoints.ToString();
			startingInv.EquippedWeapon = new Weapon("Dagger", 1, 0);

        } else if (mageToggle.isOn)
		{
			classType = ClassType.Mage;
			statPoints = 0;
			statPointsText.text = statPoints.ToString();
			startingInv.EquippedWeapon = new Weapon("Dagger", 1, 0);

        }
		else if (rogueToggle.isOn)
		{
			classType = ClassType.Rogue;
			statPoints = 0;
			statPointsText.text = statPoints.ToString();
			startingInv.EquippedWeapon = new Weapon("Dagger", 1, 0);

        }
		else if (customToggle.isOn)
		{
			classType = ClassType.Custom;
			startingInv.EquippedWeapon = new Weapon("Fists", 1, 0);

        }

		//if classType is not custom, then make an example player and pull it's stats from StatsClass
		if (classType != ClassType.Custom)
		{
			examplePlayer = new Stats(classType);


			
			playerVitality = examplePlayer.BaseStats["Vit"];
			playerStrength = examplePlayer.BaseStats["Str"];
			playerIntelligence = examplePlayer.BaseStats["Int"];
			playerWisdom = examplePlayer.BaseStats["Wis"];
			playerAgility = examplePlayer.BaseStats["Agi"];
			playerDexterity = examplePlayer.BaseStats["Dex"];
			playerLuck = examplePlayer.BaseStats["Luck"];

			playerMaxHealth = examplePlayer.CalculateMaxHealth(playerVitality);
			playerMaxMana = examplePlayer.CalculateMaxMana(playerIntelligence);


			classTypeText.text = classType.ToString();

			healthText.text = playerMaxHealth.ToString();
			manaText.text = playerMaxMana.ToString();
			vitText.text = playerVitality.ToString();
			strText.text = playerStrength.ToString();
			intText.text = playerIntelligence.ToString();
			wisText.text = playerWisdom.ToString();
			agiText.text = playerAgility.ToString();
			dexText.text = playerDexterity.ToString();
			luckText.text = playerLuck.ToString();
		}
		else            //if it is custom, then set base hp and mana, and set all stats to 0, and give 35 stat points to spend.
		{
			playerMaxHealth = 80.0f;
			playerMaxMana = 20.0f;
			playerVitality = 0f;
			playerStrength = 0f;
			playerIntelligence = 0f;
			playerWisdom = 0f;
			playerAgility = 0f;
			playerDexterity = 0f;
			playerLuck = 0f;

			statPoints = 35;
			statPointsText.text = statPoints.ToString();

			classTypeText.text = classType.ToString();

			healthText.text = playerMaxHealth.ToString();
			manaText.text = playerMaxMana.ToString();
			vitText.text = playerVitality.ToString();
			strText.text = playerStrength.ToString();
			intText.text = playerIntelligence.ToString();
			wisText.text = playerWisdom.ToString();
			agiText.text = playerAgility.ToString();
			dexText.text = playerDexterity.ToString();
			luckText.text = playerLuck.ToString();
		}





	}



	#region    //Add and Subtract Stat methods for plus and minus buttons

	public void AddVit()
	{
		if (statPoints > 0)
		{
			playerVitality += 1;
			vitText.text = playerVitality.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();

			playerMaxHealth = examplePlayer.CalculateMaxHealth(playerVitality);
			healthText.text = playerMaxHealth.ToString();

		}
	}

	public void SubVit()
	{
		if (playerVitality > 0)
		{
			playerVitality -= 1;
			vitText.text = playerVitality.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();

			playerMaxHealth = examplePlayer.CalculateMaxHealth(playerVitality);
			healthText.text = playerMaxHealth.ToString();

		}
	}

	public void AddStr()
	{
		if (statPoints > 0)
		{
			playerStrength += 1;
			strText.text = playerStrength.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void SubStr()
	{
		if (playerStrength > 0)
		{
			playerStrength -= 1;
			strText.text = playerStrength.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void AddInt()
	{
		if (statPoints > 0)
		{
			playerIntelligence += 1;
			intText.text = playerIntelligence.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();

			playerMaxMana = examplePlayer.CalculateMaxMana(playerIntelligence);
			manaText.text = playerMaxMana.ToString();

		}
	}

	public void SubInt()
	{
		if (playerIntelligence > 0)
		{
			playerIntelligence -= 1;
			intText.text = playerIntelligence.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();

			playerMaxMana = examplePlayer.CalculateMaxMana(playerIntelligence);
			manaText.text = playerMaxMana.ToString();

		}
	}

	public void AddWis()
	{
		if (statPoints > 0)
		{
			playerWisdom += 1;
			wisText.text = playerWisdom.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void SubWis()
	{
		if (playerWisdom > 0)
		{
			playerWisdom -= 1;
			wisText.text = playerWisdom.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void AddAgi()
	{
		if (statPoints > 0)
		{
			playerAgility += 1;
			agiText.text = playerAgility.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void SubAgi()
	{
		if (playerStrength > 0)
		{
			playerAgility -= 1;
			agiText.text = playerAgility.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void AddDex()
	{
		if (statPoints > 0)
		{
			playerDexterity += 1;
			dexText.text = playerDexterity.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void SubDex()
	{
		if (playerDexterity > 0)
		{
			playerDexterity -= 1;
			dexText.text = playerDexterity.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void AddLuck()
	{
		if (statPoints > 0)
		{
			playerLuck += 1;
			luckText.text = playerLuck.ToString();
			statPoints -= 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}

	public void SubLuck()
	{
		if (playerLuck > 0)
		{
			playerLuck -= 1;
			luckText.text = playerLuck.ToString();
			statPoints += 1;
			statPointsText.text = statPoints.ToString();
			
		}
	}
	#endregion




	public void ExitGame()
	{
		Application.Quit();
		
	}




	
	// Update is called once per frame
	void Update () {


		//Enable and disable plus and minus buttons when needed
		if (statPoints == 0)
		{
			foreach (Button b in plusButtons)
			{
				b.interactable = false;
			}
		} else
		{
			foreach (Button b in plusButtons)
			{
				b.interactable = true;
			}
		}

		int i = 0;
		foreach (Text t in statTexts)
		{
			if (int.Parse(statTexts[i].text) == 0)
			{
				minusButtons[i].interactable = false;
			} else
			{
				minusButtons[i].interactable = true;
			}
			i++;
		}
		
		
	}
}
