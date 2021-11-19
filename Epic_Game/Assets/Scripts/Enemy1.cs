using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1 : MonoBehaviour
{
    public GameObject player;
    public bool charging = false;
    public float windupTime = 5;
    public float timer = 0;
    public float speed = 100;
    public Rigidbody rb;
    public int HP = 100;
     public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 50) { 
            if (!charging)
            {
                rb.velocity = Vector3.zero;
                transform.LookAt(player.transform);
                timer += Time.deltaTime;
                if (timer >= windupTime)
                {
                    timer = 0;
                    charging = true;
                }
            }
            else
            {
                //charges at player
                rb.AddRelativeForce(Vector3.forward * speed);
                timer += Time.deltaTime;
                if (timer >= windupTime / 2)
                {
                    timer = 0;
                    charging = false;
                }
            }
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
            Destroy(gameObject, 1);
        }
    }
}
