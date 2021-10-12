using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2 : MonoBehaviour
{
    public GameObject player;
    public float attackRange = 20;
    public float closeRange = 10;
    public float maxHeight = 5;
    public float speed = 100;
    public Rigidbody rb;
    public int HP = 100;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);

        //WIP. Basically what it does is it tries to keep the enemy within a certain range of the player
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist > attackRange)
        {
            rb.AddRelativeForce(Vector3.forward * speed);
        }
        else if(dist < closeRange)
        {
            rb.AddRelativeForce(Vector3.forward * -speed);
        }
        else if(attackRange > dist && dist > closeRange)
        {
            rb.velocity = rb.velocity * 0.9f;
        }

        if(transform.position.y > maxHeight)
        {
            rb.AddRelativeForce(Vector3.down * speed);
        }
        
        
        
    }
    public void doDamage(){
        HP -= 20;
        if(HP <= 0){
            Destroy(gameObject);
        }
    }
}
