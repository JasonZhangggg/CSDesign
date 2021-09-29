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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!charging)
        {
            rb.velocity = Vector3.zero;
            transform.LookAt(player.transform);
            timer += Time.deltaTime;
            if(timer >= windupTime)
            {
                timer = 0;
                charging = true;
            }
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * speed);
            timer += Time.deltaTime;
            if(timer >= windupTime/2)
            {
                timer = 0;
                charging = false;
            }
        }
        
    }
    public void doDamage(){
        HP -= 20;
        if(HP <= 0){
            Destroy(gameObject);
        }
    }
}
