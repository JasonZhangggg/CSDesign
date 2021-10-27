using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot_Bullet : MonoBehaviour
{
    //gamecontroller related variables.
    public GameController gameController;
    public AudioSource playerAudioSource;
    bool soundPlayed = false;

    //creates variables to store player and bullet gameobjects
    public GameObject bullet;
    public GameObject player;
    private GameObject currentClone;
    public Text ammoText;
    public float counter = 12;
    public mouseLook cameraScript;
    //creates variables relating to bullet mechanics
    public float bulletSpeed = 20f;
    public float bulletLifeSpan = 10f;
    public float distInFrontOfCamera = 3f;
    public float reloadTime = 0f;
    public bool isReloading = false;
    public float recoil = 5;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = counter.ToString()+"/12";
        //checks if "fire" button was pressed.
        if(Input.GetButtonDown("Fire1") && !isReloading && Time.timeScale != 0)
        {
            //Instantiates a bullet slightly in from of the player and adds a force on it's z axis
            currentClone = Instantiate(bullet, player.transform.position, player.transform.rotation);
            currentClone.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed);
            currentClone.transform.position = currentClone.transform.position + (transform.forward * distInFrontOfCamera);
            //reduce ammo counter
            counter--;
            //destroys the bullet after the given lifespan
            Destroy(currentClone, bulletLifeSpan);

            //applies recoil to camera
            cameraScript.addRecoil(recoil);

            //plays gunshot
            gameController.playAudio(playerAudioSource, "Gun Shot");
        
        }
        if(Input.GetButtonDown("Reload") && !isReloading)
        {
            //manually reloads
            counter = 0;
            isReloading = true;
        }
        if(counter == 0)isReloading = true;
        if(isReloading){
            if(!soundPlayed)
            {
                gameController.playAudio(playerAudioSource, "Reload1");
                soundPlayed = true;
            }
            reloadTime+=Time.deltaTime;
        }
        if(reloadTime >= 1){
            gameController.playAudio(playerAudioSource, "Reload2");
            reloadTime = 0;
            isReloading = false;
            soundPlayed = false;
            counter = 12;
        }
    }
}
