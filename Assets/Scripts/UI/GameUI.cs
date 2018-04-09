using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public static GameUI instance = null;

    public Text levelText;
    public GameObject gameOverImage;

    public GameObject restartButton;
    public GameObject exitButton;

    public GameObject pausePanel;
    public GameObject statsPanel;
    public bool statsMenuOpen = false;

    public GameObject pickUpItemPanel;
    public GameObject itemList;
    public VerticalLayoutGroup itemListLayoutGroup;
    public GameObject listItem;
    public bool itemMenuOpen = false;

    public Button closePickUpButton;

    public List<Item> currentListItems;
    public List<GameObject> currentListObjects;

    public float levelStartDelay = 1f;

    public Text playerExpText;
    public Text playerLvlText;

    public Text playerStatMenuHealthText;
    public Text playerStatMenuManaText;
    public Text vitText;
    public Text strText;
    public Text intText;
    public Text wisText;
    public Text agiText;
    public Text dexText;
    public Text luckText;

    public Text statPointsText;


    //plus and minus buttons arrays
    public Button[] plusButtons;
    public Button[] minusButtons;

    private Text[] statTexts = new Text[7];


    private void Awake()
    {
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

    }

    // Use this for initialization
    void Start () {

        //FindAndInitUI();

    }

    private void Update()
    {

    }

    public void FindAndInitUI()
    {
        gameOverImage = GameObject.Find("GameOverImage");
        levelText = GameObject.Find("GameOverText").GetComponent<Text>();

        playerExpText = GameObject.Find("PlayerExpText").GetComponent<Text>();
        playerLvlText = GameObject.Find("PlayerLvlText").GetComponent<Text>();

        restartButton = GameObject.Find("RestartGameButton");
        exitButton = GameObject.Find("ExitGameButton");

        pausePanel = GameObject.Find("PauseMenuPanel");
        pausePanel.SetActive(false);

        pickUpItemPanel = GameObject.Find("PickUpPanel");

        itemList = GameObject.Find("ItemList");
        itemListLayoutGroup = itemList.GetComponent<VerticalLayoutGroup>();

        closePickUpButton = GameObject.Find("ClosePickUpButton").GetComponent<Button>();

        pickUpItemPanel.SetActive(false);

        //Set the text of levelText current level number.
        levelText.text = "Floor " + GameManager.floor;

        //Set levelImage to active blocking player's view of the game board during setup.
        gameOverImage.SetActive(true);

        restartButton.SetActive(false);
        exitButton.SetActive(false);

        statsPanel = GameObject.Find("StatsMenuPanel");
        

        playerStatMenuHealthText = GameObject.Find("playerStatMenuHealthText").GetComponent<Text>();
        playerStatMenuManaText = GameObject.Find("playerStatMenuManaText").GetComponent<Text>();
        vitText = GameObject.Find("vitText").GetComponent<Text>();
        strText = GameObject.Find("strText").GetComponent<Text>();
        intText = GameObject.Find("intText").GetComponent<Text>();
        wisText = GameObject.Find("wisText").GetComponent<Text>();
        agiText = GameObject.Find("agiText").GetComponent<Text>();
        dexText = GameObject.Find("dexText").GetComponent<Text>();
        luckText = GameObject.Find("luckText").GetComponent<Text>();

        statPointsText = GameObject.Find("statPointsText").GetComponent<Text>();

        statsPanel.SetActive(false);

        statTexts[0] = vitText;
        statTexts[1] = strText;
        statTexts[2] = intText;
        statTexts[3] = wisText;
        statTexts[4] = agiText;
        statTexts[5] = dexText;
        statTexts[6] = luckText;

        UpdateUIWithCurrentValues();

        //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
        Invoke("HideLevelImage", levelStartDelay);

    }

    //Hides black image used between levels
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        gameOverImage.SetActive(false);

        //Set doingSetup to false allowing player to move again.
        GameManager.instance.doingSetup = false;
    }


    //When player is over an item and presses pickup button
    public void ShowPickUpItemMenu()
    {
        //Time.timeScale = 0;

        foreach (Item item in currentListItems)
        {
            GameObject toInstantiate = listItem;
            Text listItemText = listItem.GetComponentInChildren<Text>();

            toInstantiate.GetComponent<ListItem>().ItemInList = item;
            listItemText.text = listItem.GetComponent<ListItem>().ItemInList.Name;

            
            GameObject itemObject = Instantiate(toInstantiate, itemList.transform);

            itemObject.GetComponent<ListItem>().ItemInList = item;

            currentListObjects.Add(itemObject);

            itemObject.SetActive(true);
            
        }

        pickUpItemPanel.SetActive(true);

        closePickUpButton.onClick.AddListener(ClosePickUpItemMenu);

        itemMenuOpen = true;        
    }

    

    public void ClosePickUpItemMenu()
    {
        //Time.timeScale = 1;

        pickUpItemPanel.SetActive(false);

        //currentListItems.Clear();

        foreach (GameObject obj in currentListObjects)
        {
            obj.SetActive(false);
            Destroy(obj);
        }

        currentListObjects.Clear();

        itemMenuOpen = false;

    }


    public void UpdateUIWithCurrentValues()
    {
        playerExpText.text = "Player Exp: " + StatsAndItems.PlayerStats.CurrentExp.ToString() + "/" + StatsAndItems.PlayerStats.NeededExp.ToString();
        playerLvlText.text = "Player Lvl: " + StatsAndItems.PlayerStats.Level;

        playerStatMenuHealthText.text = StatsAndItems.PlayerStats.CalculateMaxHealth().ToString();
        playerStatMenuManaText.text = StatsAndItems.PlayerStats.CalculateMaxMana().ToString();

        vitText.text = StatsAndItems.PlayerStats.BaseStats["Vit"].ToString();
        strText.text = StatsAndItems.PlayerStats.BaseStats["Str"].ToString();
        intText.text = StatsAndItems.PlayerStats.BaseStats["Int"].ToString();
        wisText.text = StatsAndItems.PlayerStats.BaseStats["Wis"].ToString();
        agiText.text = StatsAndItems.PlayerStats.BaseStats["Agi"].ToString();
        dexText.text = StatsAndItems.PlayerStats.BaseStats["Dex"].ToString();
        luckText.text = StatsAndItems.PlayerStats.BaseStats["Luck"].ToString();

        statPointsText.text = StatsAndItems.PlayerStats.StatPointsToUse.ToString();
        
        //strText.text = StatsAndItems.PlayerStats.BaseStats["Str"].ToString();

    }


    public void GameOver()
    {
        pausePanel.SetActive(false);

        gameOverImage.SetActive(true);
        levelText.text = "Game Over..";

        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }








    public void ShowStatsMenu()
    {
        UpdateUIWithCurrentValues();
        statsPanel.SetActive(true);
        //closeStatsButton.onClick.AddListener(CloseStatsMenu);
        statsMenuOpen = true;
    }

    public void CloseStatsMenu()
    {
        statsPanel.SetActive(false);
        statsMenuOpen = false;
    }


    


}
