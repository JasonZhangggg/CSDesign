using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Enemy1 : MonoBehaviour
{
    public GameObject player;
    public bool charging = false;
    public float windupTime;
    public float timer = 0;
    private float speed = 30;
    public Rigidbody rb;
    public int HP = 100;
    public GameController gameController;
    Vector3 targetPos;
    private float windUpMax=5f;
    private float windUpMin=2f;
    public bool startAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        windupTime = Random.Range(windUpMin, windUpMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 50) {
            speed = 50;
            windUpMin = 1;
            windUpMax = 2.5f;
        }
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 60) startAttacking = true;
        if (startAttacking) { 
            if (!charging)
            {
                rb.velocity = Vector3.zero;
                transform.LookAt(player.transform);
                timer += Time.deltaTime;
                if (timer >= windupTime)
                {
                    timer = 0;
                    targetPos = player.transform.position;
                    charging = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                //charges at player
                if (targetPos == transform.position)
                {
                    windupTime = Random.Range(windUpMin, windUpMax);
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
