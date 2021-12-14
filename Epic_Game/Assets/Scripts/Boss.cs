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
    PlayerHealth playerHealth;
    playerMovement PlayerMovement;
    GameObject orienter;
    Rigidbody rb;
    public GameObject slamFX;

    public float turnSpeed = 1;
    public float speed = 10;
    public float jumpForce = 100;
    
    float timer = 0;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        orienter = GameObject.FindGameObjectWithTag("Boss Orienter");
        rb = GetComponent<Rigidbody>();
        playerHealth = player.transform.GetChild(0).gameObject.GetComponent<PlayerHealth>();
        PlayerMovement =  player.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        switch(State)
        {
            case CHARGING:
                //Makes the boss smoothly rotate to look at player
                Quaternion target = new Quaternion(0, orienter.transform.rotation.y, 0, orienter.transform.rotation.w);
                transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * turnSpeed);
                
                float vX = rb.velocity.x;
                float vZ = rb.velocity.z;

                rb.AddRelativeForce(Vector3.forward * speed * Time.timeScale);
                
                if(Vector3.Distance(player.transform.position, transform.position) < 20)
                {
                    State = SLAMMING;
                }

                break;
            case SLAMMING:
                if(timer == 0 && Time.timeScale != 0)
                {
                    rb.AddRelativeForce(Vector3.up *jumpForce);
                    rb.AddRelativeForce(Vector3.forward * (speed/2) * Time.timeScale);
                    Debug.Log("Jumping");
                    timer += Time.deltaTime;
                } 
                else if(timer >= 2  && transform.position.y > 10 && Time.timeScale != 0)
                {
                    rb.AddRelativeForce(Vector3.down *jumpForce);
                }
                else if(timer >= 3)
                {
                    timer = 0;
                    State = CHARGING;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                

                break;
            default:
                break;

        }
       

        //when the boss is close enough to player while charging, does a slam of some kind and damages player if they are too close to the blast

        //idle state where it waits a bit, then randomly chooses its next state

        //ranged attack

        //ability that removes or blocks off part of the arena

    }
    
    void OnCollisionEnter(Collision col)
    {
        
        if(col.gameObject.tag == "Ground" && State == SLAMMING && timer >= 2) //if boss slams into ground creates "explosion" and pushed the player back if they are too close
        {
            //summons dust effect when boss slams the ground
            Vector3 FXpos = new Vector3(transform.position.x, transform.position.y - 4.5f, transform.position.z);
            GameObject slamFXClone = Instantiate(slamFX, FXpos, Quaternion.identity);
            Destroy(slamFXClone, 2);

            //deals damage and knockback depending on distance to slam
            if(Vector3.Distance(player.transform.position, transform.position) < 30)
            {
                Vector3 force = (player.transform.position -  transform.position);
                PlayerMovement.AddImpact(force, 5000/Vector3.Distance(player.transform.position, transform.position));
                playerHealth.takeDamage((int)Math.Round(300/Vector3.Distance(player.transform.position, transform.position)));
                
            }
        }
        else if(col.gameObject.tag == "Player" && State == SLAMMING && timer >= 2)
        {
            //if boss slams onto the player they die
            playerHealth.takeDamage(10000000);
        }
    }
}
