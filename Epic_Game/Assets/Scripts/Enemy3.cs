using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy3 : MonoBehaviour
{
    public GameObject player;
    public float speed = 0.5f;
    public Rigidbody rb;
    public int HP = 100;
    public float range = 50f;
    public GameController gameController;

    //movement variables
    bool traveling = false;
    float travelTimer = 0;
    float maxTravelDuration = 5;
    bool resting = false;
    float restTimer = 0;
    float maxRestDuration = 2;

    //parameters for area of movement
    public float maxZ;
    public float minZ;
    public float maxX;
    public float minX;
    public float targetZ;
    public float targetX;
    float moveX;
    float moveZ;
    double moveAngle;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)HP / 100;

        if (Vector3.Distance(transform.position, player.transform.position) > range)
        {
            GetComponent<Enemy_Shoot_Bullet>().enabled = false;
        }
        else
        {
            GetComponent<Enemy_Shoot_Bullet>().enabled = true;
        }
        if(Time.timeScale == 1)
        {
            transform.LookAt(player.transform);

            //if the enemy is not traveling or doing something else it will choose a new location to travel to
            if(!traveling && !resting)
            {
                targetX = UnityEngine.Random.Range(minX, maxX);
                targetZ = UnityEngine.Random.Range(minZ, maxZ);

                //randomly picks the maximum amount of time the enemy will spend traveling
                maxTravelDuration = UnityEngine.Random.Range(2, 5);
                
                traveling = true;
                
            }
            else if(Vector3.Distance(player.transform.position, transform.position) < 10)
            {
                //if the enemy is a certain distance from the player they will attack the player
                traveling = false;
                rb.AddRelativeForce(Vector3.forward * speed * 50);
            }
            else if(resting)
            {
                restTimer += Time.deltaTime;
                if (restTimer > maxRestDuration)
                {
                    restTimer = 0;
                    maxRestDuration = UnityEngine.Random.Range(1,3);
                    resting = false;
                }
            }
            else
            {
                travelTimer += Time.deltaTime;

                //the enemy will travel towards the previously selected point
                transform.position += new Vector3(moveX * Time.deltaTime * speed, 0, moveZ * Time.deltaTime * speed);
                if((Math.Abs(transform.position.x - targetX) < 5 && Math.Abs(transform.position.z - targetZ) < 5) || travelTimer >= maxTravelDuration)
                {
                    resting = true;
                    traveling = false;
                    travelTimer = 0;
                }
            }
            moveAngle = Math.Atan2((double)(targetZ-transform.position.z), (double)(targetX-transform.position.x));
            moveZ = (float)Math.Sin(moveAngle);
            moveX = (float)Math.Cos(moveAngle);
        }
    }
        
    
    public void doDamage(){
        HP -= 20;
        
        if(gameController.betterAudio)
        {
            gameController.playAudio(GetComponent<AudioSource>(), "Better Enemy Hit"); 
        }
        else
        {
            gameController.playAudio(GetComponent<AudioSource>(), "Enemy Hit"); 
        } 
        if(HP <= 0){
            gameController.addKill();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            gameController.playAudio(GetComponent<AudioSource>(), "Explosion"); 
            Destroy(gameObject);
        }
    }
}