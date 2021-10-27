using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    public void toggleActive()
    {
        bool state = settingsMenu.activeSelf;
        settingsMenu.SetActive(!state);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
