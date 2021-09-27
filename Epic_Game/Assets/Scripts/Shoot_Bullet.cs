using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Bullet : MonoBehaviour
{
    //creates variables to store player and bullet gameobjects
    public GameObject bullet;
    public GameObject player;
    private GameObject currentClone;

    //creates variables relating to bullet mechanics
    public float bulletSpeed = 20f;
    public float bulletLifeSpan = 10f;
    public float distInFrontOfCamera = 3f;

    // Update is called once per frame
    void Update()
    {
        //checks if "fire" button was pressed.
        if(Input.GetButtonDown("Fire1"))
        {
            //Instantiates a bullet slightly in from of the player and adds a force on it's x axis
            currentClone = Instantiate(bullet, player.transform.position, player.transform.rotation);
            currentClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed);
            currentClone.transform.position = currentClone.transform.position + (transform.forward * distInFrontOfCamera);

            //destroys the bullet after the given lifespan
            Destroy(currentClone, bulletLifeSpan);
        }
    }
}
