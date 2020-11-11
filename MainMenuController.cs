using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{

    public GameObject saveSound;
   
    public void NewGame()
    {
        SceneManager.LoadScene("Story");
    }

    public void Save()
    {
        GameStatus.status.Save();
        Destroy(Instantiate(saveSound), saveSound.GetComponent<AudioSource>().clip.length);
    }

    public void Load()
    {
        GameStatus.status.Load();
        if (GameStatus.status.Story == true)
        {
            SceneManager.LoadScene("Level1");
        } else
        {
            SceneManager.LoadScene("Story");
        }


    }

    public void Exit()
    {
        Application.Quit();

    }


}
