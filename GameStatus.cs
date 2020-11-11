using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameStatus : MonoBehaviour
{

    public static GameStatus status;

    public string currentLevel;

    public float health;
    public float maxHealth;
    public float previousHealth;
    public float attackDamage;
    public float throwDamage;
    public int lives;
    public bool gotKey;
    public bool questDone;

    public bool Story;
    public bool Level1;
    public bool Level2;
    public bool Level3;
    // Start is called before the first frame update
    void Awake()
    {
        if (status == null)
        {
            DontDestroyOnLoad(gameObject);  //tätä objektia ei tuhota scenen vaihtojen välillä
            status = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
    
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        data.health = health;
        data.maxHealth = maxHealth;
        data.previousHealth = previousHealth;
        data.lives = lives;
        data.gotKey = gotKey;
        data.questDone = questDone;
        data.currentLevel = currentLevel;
        data.Story = Story;
        data.Level1 = Level1;
        data.Level2 = Level2;
        data.Level3 = Level3;
        bf.Serialize(file, data);
        file.Close();

    }
    
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            maxHealth = data.maxHealth;
            previousHealth = data.maxHealth;
            lives = data.lives;
            gotKey = data.gotKey;
            questDone = data.questDone;
            currentLevel = data.currentLevel;
            Story = data.Story;
            Level1 = data.Level1;
            Level2 = data.Level2;
            Level3 = data.Level3;
        }

    }
}

[Serializable]

class PlayerData
{
    public float health;
    public float maxHealth;
    public float previousHealth;
    public int lives;
    public bool gotKey;
    public bool questDone;
    public string currentLevel;

    public bool Story;
    public bool Level1;
    public bool Level2;
    public bool Level3;
}
