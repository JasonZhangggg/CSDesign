using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const int KILL_ENEMIES = 1;
    private const int SURVIVE = 2;
    public int enemiesKilled = 0;
    public int totalKills = 0;
    public float timeElapsed = 0;
    public int[] winConditions = {SURVIVE, KILL_ENEMIES};
    public int[] winValues = {10, 60};
    public string[] levelNames = {"Level 1", "Level 2"};
    public int level = 0;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad (transform.gameObject);

        //makes sure there are no other game controllers
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("GameController");
        foreach(GameObject controller in controllers)
        {
            if (controller != gameObject)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        checkWin();
    }

    public void addKill()
    {
        enemiesKilled++;
        totalKills++;
    }

    public void checkWin()
    {
        Debug.Log(winConditions[level]);
        switch(winConditions[level])
        {
            case KILL_ENEMIES:
                Debug.Log("test");
                if(enemiesKilled >= winValues[level])
                {
                    nextLevel();
                }
                break;
            case SURVIVE:
                if(timeElapsed >= winValues[level])
                {
                    nextLevel();
                }
                
                break;
            default:
                Debug.Log("Invalid Win Condition");
                break;
        }
    }

    public void nextLevel()
    {
        enemiesKilled = 0;
        timeElapsed = 0;
        level++;
        SceneManager.LoadScene(levelNames[level]);
    }

    public void resetLevel()
    {
        //Resets the scene
        Debug.Log("You Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
