using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour 
    /* This script controls the enemy wave system of the level. 
     * It can be placed into an empty game object in the level scene. 
     * The object should be fairly close to the first enemy waypoint as
     * the enemy spawns to this objects location and then starts following the waypoints.
     */
{
    [System.Serializable]

    public class Wave
    {
        [Tooltip("Wave name or number")]
        public string waveName;
        [Header("Choose enemies for the wave")]
        public GameObject[] enemy;
        [Tooltip("Spawn delay between enemies in seconds")]
        public float delay;

        [Tooltip("Give specific path between 0 and 3, 4 if random")]
        [Range(0, 4)]
        public int dedicatedPath;

    }

    public static Spawner instance;

    public enum State { SPAWNING, WAITING, EMPTY }; // SPAWNING = enemies are being spawned, WAITING = enemies still alive, EMPTY = clear from enemies (at the beginning or after clearing a wave)

    private State state = State.EMPTY; // Declaring the state to be EMPTY as the level opens.

    [Header("Amount of waves")]
    public Wave[] waves;

    [Header("Possible paths")]
    public GameObject[] paths;

    public int nextWave = 0; // Zero at first, because when pressing start, it increments by one. Has to be public since it's needed for the UI.
    private float searchTimer = 2f; // Timer keeps track the wave clearing time / searchTimer does the enemy tag search from the hierarchy only once every second, not every frame (to make it less heavy for the system)
    //private float timer; not used atm

    public GameHandler gameHandler;

    public Image leaderGuardianHealth;

    [HideInInspector]
    public GameObject[] spawnedEnemies = new GameObject[30];

    public bool enemiesDead;

    private GameObject shuffledGO; // This is needed for enemy spawn order shuffling

    // Wave clear sound alternatives
    private string[] waveClearSounds = { "Wave_clear", "Wave_clear_var1", "Wave_clear_var2" };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void Update()
    {
        // THESE TIMERS MIGHT BE USED LATER ON TO AWARD PLAYER FOR FAST CLEARING
        //timer += Time.deltaTime; //Timer is added just in case we want to award the player for how fast they cleared the wave
        //timeText.text = timer.ToString("F1"); // Giving the time text object the time as a string

        if (state == State.WAITING) 
            // While the spawner is in waiting state, it calls for EnemiesAlive function. If there are still enemies it doesn't execute the rest of the update function.
            // However, if no enemies were found (enemiesAlive function returned false), the state is changed into EMPTY
        {
            // WAVE CLEARED!
            if (!EnemiesAlive())
            {
                //Debug.Log("No enemies found.");
                GameStateManager.Instance.currentGameState = GameStateManager.GameStates.Inactive;
                // Play random wave clear sound from alternatives
                int i = Random.Range(0, 3); // returns a number between 0-2
                AudioManager.PlaySound(waveClearSounds[i]);

                // Wave clear message etc.
                gameHandler.WaveClearUI();

                state = State.EMPTY;
            }
            else if(leaderGuardianHealth.fillAmount <= 0)
            {
                state = State.EMPTY;
            }
            else
            {
                return;
            }
        }

        if (state == State.EMPTY)
        {
            gameHandler.GameUpdate(); // Calling the function from gameHandler that is attached to the GameManager in scene
            if(nextWave < waves.Length) { 
                if (waves[nextWave].dedicatedPath == 0)
                {
                    gameHandler.waveIndicators[0].SetActive(true);
                    gameHandler.waveIndicators[1].SetActive(false);
                    gameHandler.waveIndicators[2].SetActive(false);
                }
                else if (waves[nextWave].dedicatedPath == 1)
                {
                    gameHandler.waveIndicators[1].SetActive(true);
                    gameHandler.waveIndicators[0].SetActive(false);
                    gameHandler.waveIndicators[2].SetActive(false);
                }
                else if (waves[nextWave].dedicatedPath == 2)
                {
                    gameHandler.waveIndicators[2].SetActive(true);
                    gameHandler.waveIndicators[0].SetActive(false);
                    gameHandler.waveIndicators[1].SetActive(false);
                }
                else if (waves[nextWave].dedicatedPath == 3)
                {
                    gameHandler.waveIndicators[2].SetActive(true); // The 4th possible path in level 6 begins from same place as 3th path, hence the same indicator
                    gameHandler.waveIndicators[0].SetActive(false);
                    gameHandler.waveIndicators[1].SetActive(false);
                }
                else // so if chosen random dedicated path (4th), activate all indicators
                {
                    gameHandler.waveIndicators[0].SetActive(true);
                    gameHandler.waveIndicators[1].SetActive(true);
                    gameHandler.waveIndicators[2].SetActive(true);
                }
            }
        }
    }
    public void Shuffle(Wave _wave) // This shuffles the enemies dedicated to this wave, making the spawn order randomized
    {
        for (int i = 0; i < _wave.enemy.Length - 1; i++)
        {
            int rnd = Random.Range(i, _wave.enemy.Length);
            shuffledGO = _wave.enemy[rnd];
            _wave.enemy[rnd] = _wave.enemy[i];
            _wave.enemy[i] = shuffledGO;
        }
    }

   
    public void StartWave() // This is set to the Start button in the gamePanel. It sets the game speed to 1, deactivated the gamePanel and starts the timer and starts spawning, unless it's already spawning. 
    {
        // Play wave start sound
        AudioManager.PlaySound("Wave_start");
        GameStateManager.Instance.currentGameState = GameStateManager.GameStates.Active;
        Shuffle(waves[nextWave]);

        gameHandler.GetComponent<GameHandler>().startAndSpeedButton.GetComponent<GameStartAndSpeed>().gameON = true;
        Time.timeScale = 1f;
        gameHandler.GetComponent<GameHandler>().startAndSpeedButton.GetComponent<Animator>().Play("Idle");
        //timer = 0f; not in use atm

        if (state != State.SPAWNING)
        {
            StartCoroutine(SpawnWave(waves[nextWave])); // Starting a coroutine "SpawnWave" that spawns the enemies at the set delay from each other. It checks which of the waves it is and sends the information to the coroutine.
        }
        nextWave ++; // After spawning is done, the wave number is incremented, so that next time the StartWave is called, it's the next wave.
        gameHandler.WaveTextUpdate();

        // The music during waves
        if (!AudioManager.IsPlaying("TFG_Battle_2") && !AudioManager.IsPlaying("TFG_Battle_1"))
        {
            AudioManager.StopMusic("TD_Demo");

            int rand = Random.Range(0, 2);
            if(rand == 1) AudioManager.PlayOnLoop("TFG_Battle_2");
            else AudioManager.PlayOnLoop("TFG_Battle_1");
        }
    }

    bool EnemiesAlive() // Function that checks every second (not every frame, hence the timer), if there's enemy tagged objects in the hierarchy.
    {
        //searchTimer -= Time.deltaTime;
        //if (searchTimer <= 0f)
        //{
        //    searchTimer = 1f;
        //    if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("EnemyArmored") == null)
        //    {
        //        enemiesDead = true;
        //        return false;
        //    }
        //}
        for(int i = 0; i < spawnedEnemies.Length; i++)
        {
            if(spawnedEnemies[i] != null)
            {
                return true;
            }
        }
        enemiesDead = true;
        return false;
    }

    IEnumerator SpawnWave(Wave _wave) // Spawning coroutine
    {
        state = State.SPAWNING; // state is changed

        for (int i = 0; i < _wave.enemy.Length; i++) // Goes through the loop as many times as you have set the amount of enemies to be in the wave
        {
            spawnedEnemies[i] = SpawnEnemy(_wave.enemy[i], _wave); // Spawn function called and as parameters it gives a random number from 0 to the amount of different enemies you assigned for this wave
            yield return new WaitForSeconds(_wave.delay); // waits the time set as delay until next spawn function is called
        }

        state = State.WAITING; // when spawning loop is over, the state is changed into waiting 
        yield break; // leaves the coroutine
    }

    GameObject SpawnEnemy(GameObject _enemy, Wave _wave) // Function that instantiates the enemies at the position where the Spawner script is attached to. Enemy starts then moving towards it's first waypoint (0 by default)
    {
        GameObject tempEnemy = _enemy;
        GameObject spawnedEnemy = null;
        if (_wave.dedicatedPath == 4) // Number 4 means the path is chosen to be random between the possible paths
        {
            int randPath = Random.Range(0, paths.Length);
            tempEnemy.GetComponent<Attacker>().path = randPath; // giving the attacker a new path value
            spawnedEnemy = Instantiate(tempEnemy, paths[randPath].transform.position, Quaternion.identity); // spawning it to a position according to the path
            
        }
        else
        {
            tempEnemy.GetComponent<Attacker>().path = _wave.dedicatedPath; // giving the attacker a new path value
           spawnedEnemy = Instantiate(tempEnemy, paths[_wave.dedicatedPath].transform.position, Quaternion.identity); // spawning it to a position according to the path
        }
        return spawnedEnemy;
    }
}
