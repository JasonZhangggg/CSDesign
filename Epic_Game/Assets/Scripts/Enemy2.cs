﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2 : MonoBehaviour
{
    public GameObject player;
    public float attackRange = 20;
    public float closeRange = 10;
    public float maxHeight = 5;
    public float minHeight = 0;
    public float speed = 100;
    public Rigidbody rb;
    public int HP = 100;
    private int direction = 4;
    public Component Enemy_Shoot_Bullet;

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
            rb.velocity -= rb.velocity * (0.99f * Time.deltaTime);
        }

        if(transform.position.y > maxHeight)
        {
            rb.AddRelativeForce(Vector3.down * speed);
        }
        if(transform.position.y < minHeight)
        {
            rb.AddRelativeForce(Vector3.up * speed);
        }
        rb.AddRelativeForce((1 * Time.deltaTime * speed * direction),0,0);
        rb.velocity += Vector3.up * rb.velocity.y * 0.01f;
        
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.blue);
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            if(hit.collider.tag == "Player")
            {
                GetComponent<Enemy_Shoot_Bullet>().enabled = true;
            }
            else
            {
                GetComponent<Enemy_Shoot_Bullet>().enabled = false;
            }
        }

    }

    void OnCollisionEnter(Collision col)
    {
        direction *= -1;
    }


    public void doDamage(){
        HP -= 20;
        if(HP <= 0){
            Destroy(gameObject);
        }
    }
}