using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot_Bullet : MonoBehaviour
{
    //creates variables to store player and bullet gameobjects
    public GameObject bullet;
    public GameObject player;
    private GameObject currentClone;
    public Text ammoText;
    public float counter = 12;
    //creates variables relating to bullet mechanics
    public float bulletSpeed = 20f;
    public float bulletLifeSpan = 10f;
    public float distInFrontOfCamera = 3f;
    public float reloadTime = 0f;
    public bool isReloading = false;
    // Update is called once per frame
    void Update()
    {
        ammoText.text = counter.ToString()+"/12";
        //checks if "fire" button was pressed.
        if(Input.GetButtonDown("Fire1") && !isReloading)
        {
            //Instantiates a bullet slightly in from of the player and adds a force on it's z axis
            currentClone = Instantiate(bullet, player.transform.position, player.transform.rotation);
            currentClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed);
            currentClone.transform.position = currentClone.transform.position + (transform.forward * distInFrontOfCamera);
            //reduce ammo counter
            counter--;
            //destroys the bullet after the given lifespan
            Destroy(currentClone, bulletLifeSpan);
        
        }
        if(Input.GetButtonDown("Reload") && !isReloading)
        {
            //manually reloads
            counter = 0;
            isReloading = true;
        }
        if(counter == 0)isReloading = true;
        if(isReloading){
            reloadTime+=Time.deltaTime;
        }
        if(reloadTime >= 1){
            reloadTime = 0;
            isReloading = false;
            counter = 12;
        }
    }
}
