using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot_Bullet : MonoBehaviour
{
    //creates variables to store player and bullet gameobjects
    public GameObject bullet;
    public GameObject player;
    private GameObject currentClone;

    public float fireRate = 2;
    private float timer = 0;

    //creates variables relating to bullet mechanics
    public float bulletSpeed = 20f;
    public float bulletLifeSpan = 10f;
    public float distInFrontOfCamera = 3f;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Checks if its time to shoot
        if(timer >= fireRate)
        {
            //Instantiates a bullet slightly in from of the player and adds a force on it's x axis
            currentClone = Instantiate(bullet, transform.position, transform.rotation);
            currentClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed);
            currentClone.transform.position = currentClone.transform.position + (transform.forward * distInFrontOfCamera);
            currentClone.tag = "Enemy_Bullet";

            //destroys the bullet after the given lifespan
            Destroy(currentClone, bulletLifeSpan);
            timer = 0;
        }
    }
}
