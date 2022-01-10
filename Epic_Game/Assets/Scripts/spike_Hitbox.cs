using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike_Hitbox : MonoBehaviour
{
    int frames = 0;

    // Update is called once per frame
    void Update()
    {
        //destroys the hitbox after 1 frame
        if(Time.timeScale != 0 && frames !=0)//timer > hitboxDuration)
        {
            Destroy(gameObject);
        }
        frames++;
    }
}
