using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Orienter : MonoBehaviour
{
    //the job of this is just to look at the player so the boss knows where to look
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //looks at player
        transform.LookAt(player.transform);
    }
}
