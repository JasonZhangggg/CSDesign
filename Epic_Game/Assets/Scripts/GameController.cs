using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Types of win conditions. If you update this make sure to also update the UIObjective code and the checkwin functio below
    private const int KILL_ENEMIES = 1;
    private const int SURVIVE = 2;
    
    //Player stats
    public int enemiesKilled = 0;
    public int totalKills = 0;
    public float timeElapsed = 0;

    //Variables holding information about each level
    public int[] winConditions = {SURVIVE, KILL_ENEMIES};
    public int[] winValues = {10, 60};
    public string[] levelNames = {"Level 1", "Level 2"};
    public int level = 0;

    //Sound management
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    
    // Start is called before the first frame update
    void Awake()
    {
        //Makes it so the game controller presists through levels
        DontDestroyOnLoad (transform.gameObject);

        //makes sure there are no other game controllers
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("GameController");
        foreach(GameObject controller in controllers)
        {
            if (controller != gameObject)
            {
                //if it finds a game controller isn't this one it will destroy itself
                //since this only runs when the controller is first created only the original gamecontroller will stick around
                Destroy(gameObject);
            }
        }

        //Add new audio clips here
        audioClips.Add("Gun Shot", Resources.Load<AudioClip>("Audio/mixkit-game-gun-shot-1662"));
        audioClips.Add("Reload1", Resources.Load<AudioClip>("Audio/mixkit-handgun-release-1664"));
        audioClips.Add("Reload2", Resources.Load<AudioClip>("Audio/mixkit-handgun-click-1660"));
        audioClips.Add("Explosion", Resources.Load<AudioClip>("Audio/mixkit-short-explosion-1694"));

        //Sounds obtained from https://mixkit.co/free-sound-effects/

    }

    public void playAudio(AudioSource audioSource, string audioClip)
    {
        audioSource.PlayOneShot(audioClips[audioClip]);
    }


    // Update is called once per frame
    void Update()
    {
        //updates time elapsed
        timeElapsed += Time.deltaTime;
        checkWin();
    }

    //adds 1 to enemies killed and total kills
    public void addKill()
    {
        enemiesKilled++;
        totalKills++;
    }

    //checks if the current win condition has been met
    public void checkWin()
    {
        switch(winConditions[level])
        {
            case KILL_ENEMIES:
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

    //loads next level and resets certain variables
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
