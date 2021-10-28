using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameController gamecontroller;

    void start()
    {
        //gets game controller
        gamecontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    //either makes the setting menu active or not depending on if it is already active
    public void toggleActive()
    {
        bool state = settingsMenu.activeSelf;
        gamecontroller.settingsOpen = !gamecontroller.settingsOpen;
        settingsMenu.SetActive(!state);
    }
}
