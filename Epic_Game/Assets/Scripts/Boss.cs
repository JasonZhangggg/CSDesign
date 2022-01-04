using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    //different states the boss can be in
    const int IDLE = 0;
    const int CHARGING = 1;
    const int THROWING = 2;
    const int SPIKES = 3;
    const int SLAMMING = 10;//this is 10 so the boss doesn't go from idle to slamming
    const int DYING = 11;

    int numberOfStates = 4; 
    

    //boss' current state
    int State = IDLE;

    GameObject player;
    GameController gameController;
    PlayerHealth playerHealth;
    playerMovement PlayerMovement;
    GameObject orienter;
    Rigidbody rb;
    Animator animationController;
    public GameObject slamFX;
    public GameObject rock;
    public GameObject spikes;
    public GameObject deathFX;
    public AudioSource audioSource;

    public float turnSpeed = 1;
    public float speed = 10;
    public float jumpForce = 100;
    public float throwForce = 10;
    public bool hasThrown = false;

    public float health = 1000;
    float timer = 0;
    float idleLength;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        orienter = GameObject.FindGameObjectWithTag("Boss Orienter");
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<Animator>();
        playerHealth = player.transform.GetChild(0).gameObject.GetComponent<PlayerHealth>();
        PlayerMovement =  player.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        switch(State)
        {
            case IDLE:
                animationController.Play("Idle");
                //Boss contemplates life and decides what to do next
                if(timer == 0)
                {
                    //waits a random length of time
                    idleLength = UnityEngine.Random.Range(1, 3);
                    timer += Time.deltaTime;
                }
                else if(timer >= idleLength)
                {
                    //randomly decided next state
                    State = (int)Math.Ceiling((double)UnityEngine.Random.Range(1, numberOfStates));
                    timer = 0;
                    Debug.Log(State);
                }
                else
                {
                    turnTowardsPlayer();
                    timer += Time.deltaTime;
                }
                

                break;
            case CHARGING:
                //Boss charges at player and goes into the slamming state if close enough. Speed increases over time so player can't just run
                turnTowardsPlayer();
                animationController.Play("FlyForward");
                rb.AddRelativeForce(Vector3.forward * speed * Time.timeScale * ((timer/10) + 1));
                timer += Time.deltaTime;
                
                if(Vector3.Distance(player.transform.position, transform.position) < 20)
                {
                    timer = 0;
                    animationController.Play("Idle");
                    State = SLAMMING;
                }

                break;
            case SLAMMING:
                //Boss jumps up, then slams into the ground, dealing AOE damage
                if(timer == 0 && Time.timeScale != 0)
                {
                    //Jumps
                    rb.AddRelativeForce(Vector3.up *jumpForce);
                    rb.AddRelativeForce(Vector3.forward * (speed/2) * Time.timeScale);
                    Debug.Log("Jumping");
                    timer += Time.deltaTime;
                } 
                else if(timer >= 2  && transform.position.y > 10 && Time.timeScale != 0)
                {
                    //Slams back down
                    rb.AddRelativeForce(Vector3.down *jumpForce);
                }
                else if(timer >= 3)
                {
                    //waits a sec before returning to idle state
                    timer = 0;
                    State = IDLE;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                

                break;
            case THROWING:
                timer += Time.deltaTime;
                turnTowardsPlayer();
                //Throws a rock at the player
                animationController.Play("RightPunchAttack");

                if(timer >= 0.7f && !hasThrown)
                {
                    Vector3 rockPos = new Vector3(transform.position.x, transform.position.y + 7.5f, transform.position.z);
                    GameObject projectile = Instantiate(rock, rockPos, transform.rotation);
                    Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
                    projectile.transform.position = projectile.transform.position + (transform.forward * 10.5f);
                    projectile.transform.localPosition += new Vector3(-3, 0, 0);
                    projectileRb.AddRelativeForce((Vector3.forward + (new Vector3(-0.075f, -0.075f, 0))) * throwForce);
                    hasThrown = true;
                    
                } else if(timer >= 1.3f)
                {
                    timer = 0;
                    State = IDLE;
                    hasThrown = false;
                }
                break;

            case SPIKES:
                if(timer == 0 && Time.timeScale != 0)
                {
                    //Jumps
                    rb.AddRelativeForce(Vector3.up *(jumpForce * 2));
                    timer += Time.deltaTime;
                } 
                else if(timer >= 2  && transform.position.y > 10 && Time.timeScale != 0)
                {
                    //Slams back down
                    rb.AddRelativeForce(Vector3.down *jumpForce);
                }
                else if(timer >= 3)
                {
                    //waits a sec before returning to idle state
                    timer = 0;
                    State = IDLE;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                break;
            case DYING:
                animationController.Play("Die");
                timer += Time.deltaTime;
                if(timer > 1)
                {
                    Vector3 FXpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    GameObject slamFXClone = Instantiate(deathFX, FXpos, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            default:
                State = IDLE;
                break;

        }
       

        

       


        //ability that removes or blocks off part of the arena

    }
    
    void turnTowardsPlayer()//turns towards the player slightly
    {
        Quaternion target = new Quaternion(0, orienter.transform.rotation.y, 0, orienter.transform.rotation.w);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * turnSpeed);
    }

    void OnCollisionEnter(Collision col)
    {
        
        if(col.gameObject.tag == "Ground" && State == SLAMMING && timer >= 2) //if boss slams into ground creates "explosion" and pushes the player back if they are too close
        {
            //summons dust and rubble effect when boss slams the ground
            Vector3 FXpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject slamFXClone = Instantiate(slamFX, FXpos, Quaternion.identity);
            gameController.playAudio(audioSource, "Boss Slam");
            Destroy(slamFXClone, 4);

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
        if(col.gameObject.tag == "Ground" && State == SPIKES && timer >= 2) //if boss slams into ground creates "explosion" and pushes the player back if they are too close
        {
            gameController.playAudio(audioSource, "Boss Slam");
            Vector3 spikesPos = new Vector3(transform.position.x, transform.position.y , transform.position.z);
            Instantiate(spikes, spikesPos, transform.rotation);
        }
    }

    public void takeDamage(float damage)
    {
        if(health != 0)
        {
            health -= damage;
        }
        if(health <= 0)
        {
            timer = 0;
            State = DYING;
            health = 0;
        }
    }

}
