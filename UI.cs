using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public Image Filler;
    public Text healthValue, amount;

    public float counter, maxCounter;

    public GameObject UIKey;
    

    // Start is called before the first frame update
    void Start()
    {
        GameStatus.status.previousHealth = GameStatus.status.health;
        UIKey.gameObject.SetActive(false);
        GameStatus.status.gotKey = false;
        GameStatus.status.questDone = false;

    }

    // Update is called once per frame
    void Update()
    {

        //UIavaimen aktivointi
        if(GameStatus.status.gotKey == false)
        {
            UIKey.gameObject.SetActive(false);
        } else
        {
            UIKey.gameObject.SetActive(true);
        }

        //counter
        if (counter > maxCounter)
        {
            GameStatus.status.previousHealth = GameStatus.status.health;
            counter = 0;
        }
        else
        {
            counter += Time.deltaTime;
        }

        amount.text = "x " + GameStatus.status.lives;

        //Hitaasti liikkuva healthbar
        Filler.fillAmount = Mathf.Lerp(GameStatus.status.previousHealth / GameStatus.status.maxHealth, GameStatus.status.health / GameStatus.status.maxHealth, counter / maxCounter);
        healthValue.text = Mathf.RoundToInt(GameStatus.status.health) + " / " + GameStatus.status.maxHealth;
    }

}

