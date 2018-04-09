using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameMenuController : MonoBehaviour
{

    public GameObject loader;

    public GameObject pausePanel;
    public GameObject statsPanel;



    public void Awake()
    {
        if (GameManager.instance == null)
        {
            //Instantiate Loader prefab if gamemanager(which loader creates) is null
            Instantiate(loader);
        }
    }


    public void ShowPickupItemMenu()
    {
        if (Player.overItem == true)
        {
            GameUI.instance.ShowPickUpItemMenu();
        }        
    }

    public void ShowStatsMenu()
    {
        GameUI.instance.ShowStatsMenu();
        
    }

    public void ExitGame()
    {
        Application.Quit();

    }

    public void CallGameOver()
    {
        GameManager.instance.GameOver();
    }

    public void RestartGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        SceneManager.LoadScene("StartMenu");
    }
    

    public void PauseGame()
    {
        Time.timeScale = 0;

        pausePanel.SetActive(true);


    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        pausePanel.SetActive(false);
    }


    private int StatPointsFromStats()
    {
        return StatsAndItems.PlayerStats.StatPointsToUse;
    }


    #region    //Add Stat methods for plus and minus buttons

    public void AddVit()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Vit"] += 1;
            GameUI.instance.vitText.text = StatsAndItems.PlayerStats.BaseStats["Vit"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

            GameUI.instance.playerStatMenuHealthText.text = StatsAndItems.PlayerStats.CalculateMaxHealth().ToString();


        }
    }

    public void AddStr()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Str"] += 1;
            GameUI.instance.strText.text = StatsAndItems.PlayerStats.BaseStats["Str"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

        }
    }

    public void AddInt()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Int"] += 1;
            GameUI.instance.intText.text = StatsAndItems.PlayerStats.BaseStats["Int"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

            GameUI.instance.playerStatMenuManaText.text = StatsAndItems.PlayerStats.CalculateMaxMana().ToString();
        }
    }

    public void AddWis()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Wis"] += 1;
            GameUI.instance.wisText.text = StatsAndItems.PlayerStats.BaseStats["Wis"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

        }
    }

    public void AddAgi()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Agi"] += 1;
            GameUI.instance.agiText.text = StatsAndItems.PlayerStats.BaseStats["Agi"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

        }
    }

    public void AddDex()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Dex"] += 1;
            GameUI.instance.dexText.text = StatsAndItems.PlayerStats.BaseStats["Dex"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

        }
    }

    public void AddLuck()
    {
        if (StatPointsFromStats() > 0)
        {
            StatsAndItems.PlayerStats.BaseStats["Luck"] += 1;
            GameUI.instance.luckText.text = StatsAndItems.PlayerStats.BaseStats["Luck"].ToString();
            StatsAndItems.PlayerStats.StatPointsToUse -= 1;
            GameUI.instance.statPointsText.text = StatPointsFromStats().ToString();

        }
    }

    #endregion

}
