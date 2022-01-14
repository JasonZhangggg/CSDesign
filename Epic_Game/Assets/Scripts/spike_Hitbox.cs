using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike_Hitbox : MonoBehaviour
{
    float timer = 0;
    float hitboxDuration = 0.1f;

    // Update is called once per frame
    void Update()
    {
        //destroys the hitbox after 1 frame
        if(Time.timeScale != 0 && timer > hitboxDuration)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }
}
