using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Types of win conditions. If you update this make sure to also update the UIObjective code and the checkwin functio below
    private const int KILL_ENEMIES = 1;
    private const int SURVIVE = 2;
    private const int LOCATION = 3;
    private const int ACTION = 4;

    //Player stats
    public int enemiesKilled = 0;
    public int totalKills = 0;
    public float timeElapsed = 0;

    //Variables holding information about each level
    public int[][] winConditions = new int[][]{ new int[]{LOCATION, LOCATION, ACTION, KILL_ENEMIES, KILL_ENEMIES}, new int[]{ KILL_ENEMIES } };
    public int[][] winValues = new int[][] {new int[]{ 9, 23, 1, 3, 1 }, new int[]{ 60 } };
    public string[][] objText = new string[][] { new string[]{"Look around with your mouse and WASD to move", "Press space to jump over the obstacle", "Use the shift key to dash around", "Right click to shoot the 3 targets", "Kill" }, new string[]{ "Kill" } };

    public string[] levelNames = {"Level 1", "Level 2"};
    public int level = 0;
    public int winPart = 0;


    Vector3 loc;
    //Sound management
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public float volume = 1f;
    public Slider volumeSlider;
    public Text volumeLabel;

    //player settings
    public float mouseSensitivity = 75f;

    //System variables
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public bool settingsOpen = false;
    public bool isPaused = false;
    
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

    //Changes volume to value selected by user
    public void updateVolume()
    {
        volume = volumeSlider.value;
        volumeLabel.text = "Volume: " + Math.Round(volumeSlider.value*100, 1);
    }

    //plays given audio clip at the given audio source
    public void playAudio(AudioSource audioSource, string audioClip)
    {
        audioSource.PlayOneShot(audioClips[audioClip], volume);
    }


    // Update is called once per frame
    void Update()
    {
        //updates time elapsed
        loc = GameObject.Find("Player").transform.position;
        timeElapsed += Time.deltaTime;
        checkWin();

        //checks if user presses the pause button
        if(Input.GetButtonDown("Pause"))
        {
            //if the user presses paus again it will exit the current menu
            if(settingsOpen)
            {
                settingsMenu.SetActive(false);
                settingsOpen = false;
            }
            else
            {
                pause();
            }
        }

        //stops time if game is paused, resumes time otherwise
        if(isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //toggles the games pause status
    public void pause()
    {
        isPaused = !isPaused;
    }

    //ends program
    public void quit()
    {
        //Note that this does nothing in editor mode
        Application.Quit();
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
        switch (winConditions[level][winPart])
        {
            case KILL_ENEMIES:
                if (enemiesKilled >= winValues[level][winPart]){
                    resetKills();
                    winPart++;
                }
                break;
            case SURVIVE:
                if (timeElapsed >= winValues[level][winPart]) {
                    resetKills();
                    winPart++;
                }
                break;
            case LOCATION:
                if (loc.z >= winValues[level][winPart] && loc.x <= -17) winPart++;
                break;
            case ACTION:
                if (playerMovement.hasDashed == 1){
                    Destroy(GameObject.Find("Barrier1"));
                    winPart++;
                }
                break;
            default:
                Debug.Log("Invalid Win Condition");
                break;
        }
        if (winPart == winConditions[level].Length) {
            Debug.Log("Next Level");
            nextLevel();
        }
    }

    //loads next level and resets certain variables
    public void nextLevel()
    {
        enemiesKilled = 0;
        timeElapsed = 0;
        level++;
        winPart = 0;
        SceneManager.LoadScene(levelNames[level]);
    }

    //resets kills
    public void resetKills() {
        enemiesKilled = 0;
        timeElapsed = 0;
    }
    public void resetLevel()
    {
        //Resets the scene
        enemiesKilled = 0;
        timeElapsed = 0;
        winPart = 0;
        Debug.Log("You Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //updates the mouse sensitivity to match what is in the settings input field
    public void setMouseSensitivity()
    {
        float sensitivity = float.Parse(GameObject.FindGameObjectWithTag("Sensitivity Field").GetComponent<Text>().text);
        mouseSensitivity = sensitivity;
        GameObject.Find("Player Camera").GetComponent<mouseLook>().updateMouseSensitivity();
    }
}
