using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour // This script handles all the UI functionalities that have something to do with the game logic
{

    public TMP_Text timeText, SpeedButtonText; // timer text object and two different info texts that appear in a panel
    public TMP_Text waveInfo;

    public GameObject startAndSpeedButton, levelFailedPanel, levelFailedRetryPanel, levelClearedPanel, demoClearedPanel, waveClearText, tutorialPanel;
    public GameObject spawner;
    private Spawner spawnerSc;
    public GameObject[] waveIndicators;
    public Image leaderGuardianHealth;

    private void Start()
    {
        spawnerSc = spawner.GetComponent<Spawner>();
        waveInfo.text = spawnerSc.nextWave.ToString() + "/" + spawnerSc.waves.Length.ToString();
        levelClearedPanel.SetActive(false);
        levelFailedPanel.SetActive(false);

        levelFailedRetryPanel = GameObject.FindGameObjectWithTag("RetryPanel"); // sorry, too hard to assign this individually in EVERY LEVEL
        levelFailedRetryPanel.SetActive(false);

        SaveManager.Instance.data.currency = 300;
        SaveManager.Instance.Save();
    }

    public void GameUpdate()
    {
        if(tutorialPanel != null)
        {
            if (tutorialPanel.activeSelf)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f; // Game speed set to zero while player purchases and places the guardians.
            }
        } else
        {
            Time.timeScale = 0f;
        }
      
       

        // The music while player purchases and places the guardians
        if (!AudioManager.IsPlaying("TD_Demo"))
        {
            AudioManager.StopMusic("TFG_Battle_2");
            AudioManager.StopMusic("TFG_Battle_1");
            AudioManager.PlayOnLoop("TD_Demo");
        }

        if(startAndSpeedButton.activeSelf == true)startAndSpeedButton.GetComponent<Animator>().Play("StartSpeedButtonPulse");

        // You lose. Level failed
        if (leaderGuardianHealth.fillAmount <= 0 && levelFailedRetryPanel.activeSelf == false && levelFailedPanel.activeSelf == false)
        {
            // if over half of the waves cleared and hasn't appeared yet
            if (spawnerSc.waves.Length / 2 < spawnerSc.nextWave && PlayerPrefs.GetInt("Retried", 0) != 1)
            {
                levelFailedRetryPanel.SetActive(true);
                PlayerPrefs.SetInt("Retried", 1);
            }
            else levelFailedPanel.SetActive(true);

            startAndSpeedButton.SetActive(false);
        }
        else if (spawnerSc.nextWave + 1 > spawnerSc.waves.Length) // If the last wave has been cleared the text is changed and instead of start button, there will be a continue button enabled
        {

            if((SaveManager.Instance.data.levelProgress + 4) == SceneManager.GetActiveScene().buildIndex) // The first level is scene index 4
            {
                SaveManager.Instance.data.talentPoints += 3;
                SaveManager.Instance.data.levelProgress += 1;
                SaveManager.Instance.Save();

                levelClearedPanel.SetActive(true);
                startAndSpeedButton.SetActive(false);
                gameObject.SetActive(false);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 13) //Soft launch last level is level 10 which is scene index 13
            {
                SaveManager.Instance.data.talentPoints += 3;
                //SaveManager.Instance.data.levelProgress += 1;
                //We don't increase the level progress since there's no more levels at this time, and increasing it would cause an error to the scene selector
                SaveManager.Instance.Save();

                levelClearedPanel.SetActive(true);
                startAndSpeedButton.SetActive(false);
                gameObject.SetActive(false);
            }
            else //if the level is replayed, we don't want to increase the talents nor levelprogress
            {
                levelClearedPanel.SetActive(true);
                startAndSpeedButton.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void WaveTextUpdate() // Function called in spawner once the wave number needs to be updated
    {
        waveInfo.text = spawnerSc.nextWave.ToString() + "/" + spawnerSc.waves.Length.ToString();
    }

    public void WaveClearUI() // Called in Spawner once a wave is cleared
    {
        if (leaderGuardianHealth.fillAmount > 0)
        {
            waveClearText.GetComponent<Animator>().Play("WaveClear");
            startAndSpeedButton.GetComponent<GameStartAndSpeed>().gameON = false;
            SpeedButtonText.text = "START";
        }
    }
}
