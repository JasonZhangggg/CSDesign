using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot_Bullet : MonoBehaviour
{
    //creates variables to store bullet gameobjects
    public GameObject fireball;
    private GameObject currentClone;
    public GameController gameController;

    public float fireRate = 2;
    public float timer = 0;

    //creates variables relating to bullet mechanics
    private float bulletSpeed = 2f;
    private float bulletLifeSpan = 10f;
    private float distInFrontOfCamera = 3f;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }   

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Checks if its time to shoot
        if(timer >= fireRate)
        {
            //Instantiates a bullet slightly in from of the enemy and adds a force on it's z axis
            currentClone = Instantiate(fireball, transform.position, transform.rotation);
            currentClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed);
            currentClone.transform.position = currentClone.transform.position + (transform.forward * distInFrontOfCamera);
            currentClone.tag = "Enemy_Bullet";

            //Makes sound
            gameController.playAudio(GetComponent<AudioSource>(), "Enemy Gun Shot");

            //destroys the bullet after the given lifespan
            Destroy(currentClone, bulletLifeSpan);
            timer = 0;
        }
    }
}
