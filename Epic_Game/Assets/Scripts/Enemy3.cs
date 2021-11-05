using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public GameObject player;
    public float speed = 0.5f;
    public Rigidbody rb;
    public int HP = 100;
    public GameObject gameController;

    bool traveling = false;
    public float maxZ;
    public float minZ;
    public float maxX;
    public float minX;
    public float targetZ;
    public float targetX;
    float moveX;
    float moveZ;
    double moveAngle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1)
        {
            transform.LookAt(player.transform);

            //if the enemy is not traveling or doing something else it will choose a new location to travel to
            if(!traveling)
            {
                targetX = UnityEngine.Random.Range(minX, maxX);
                targetZ = UnityEngine.Random.Range(minZ, maxZ);
                
                traveling = true;
                
            }
            else if(Vector3.Distance(player.transform.position, transform.position) < 10)
            {
                traveling = false;
                rb.AddRelativeForce(Vector3.forward * speed * 50);
            }
            else
            {
                //the enemy will travel towards the previously selected point
                transform.position += new Vector3(moveX * Time.deltaTime * speed, 0, moveZ * Time.deltaTime * speed);
                if(Math.Abs(transform.position.x - targetX) < 5 && Math.Abs(transform.position.z - targetZ) < 5)
                {
                    traveling = false;
                }
            }
            moveAngle = Math.Atan2((double)(targetZ-transform.position.z), (double)(targetX-transform.position.x));
            moveZ = (float)Math.Sin(moveAngle);
            moveX = (float)Math.Cos(moveAngle);
        }
    }
        
    
    public void doDamage(){
        HP -= 20;
        if(HP <= 0){
            gameController.GetComponent<GameController>().addKill();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            gameController.GetComponent<GameController>().playAudio(GetComponent<AudioSource>(), "Explosion"); 
            Destroy(gameObject, 1);
        }
    }
}