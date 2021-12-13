using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    //different states the boss can be in
    const int CHARGING = 1;
    const int SLAMMING = 2;

    //boss' current state
    int State = CHARGING;

    GameObject player;
    GameController gameController;
    public GameObject orienter;

    public float turnSpeed = 1;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(State == CHARGING)
        {
            //Makes the boss smoothly rotate to look at player
            Quaternion target = new Quaternion(0, orienter.transform.rotation.y, 0, orienter.transform.rotation.w);
            transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * turnSpeed);

            //add force to boss until a certain velocity is reached and don't go above that velocity

        }
        //when the boss is close enough to player while charging, does a slam of some kind and damages player if they are too close to the blast

        //idle state where it waits a bit, then randomly chooses its next state

        //ranged attack

        //ability that removes or blocks off part of the arena

    }
}
