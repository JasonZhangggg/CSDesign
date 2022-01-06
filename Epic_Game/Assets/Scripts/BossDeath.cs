using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    public GameObject winMenu;
    public GameController gameController;
    public AudioSource audioSource;
    float timer = 0;
    bool menuActive = false;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameController.playAudio(audioSource, "Explosion");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 2 && !menuActive)
        {
            //makes it so the player can't pause
            gameController.isDead = true;
            menuActive = true;

            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            winMenu.SetActive(true);
        }
    }

    public void exit()
    {
        gameController.quit();
    }

    public void restart()
    {
        Time.timeScale = 1;
        gameController.isDead = false;
        gameController.loadLevel(0);
    }
}
