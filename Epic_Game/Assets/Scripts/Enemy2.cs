using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour
{
    private GameObject player;
    public float attackRange = 60;
    public float closeRange = 40;
    public float maxHeight = 20;
    public float minHeight = 20;
    public float speed = 100;
    public Rigidbody rb;
    public int HP = 100;
    private int direction = 4;
    private GameController gameController;
    public Slider slider;
    public GameObject deathFX;

    public Animator animationController;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }



    // Update is called once per frame
    void Update()
    {
        slider.value = (float)HP / 100;
        if (!dead)
        {
            transform.LookAt(player.transform);

            //tries to keep the enemy within a certain range of the player
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist > attackRange)
            {
                Enemy_Shoot_Bullet.shooting = false;
                rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime *5);
                animationController.SetBool("Fly Forward", true);

            }
            else if (dist < closeRange)
            {
                Enemy_Shoot_Bullet.shooting = false;
                rb.AddRelativeForce(Vector3.forward * -speed * Time.deltaTime *5);
                animationController.SetBool("Fly Forward", true);

            }
            else if (attackRange > dist && dist > closeRange)
            {
                //If enemy is in attack range they will shoot the player
                Enemy_Shoot_Bullet.shooting = true;
                rb.velocity -= rb.velocity * (0.9f * Time.deltaTime);
                animationController.SetBool("Fly Forward", false);

            }

            if (transform.position.y > maxHeight)
            {
                rb.AddRelativeForce(Vector3.down * 800 * Time.deltaTime);
            }
            else if (transform.position.y < minHeight)
            {
                rb.AddRelativeForce(Vector3.up * 800 * Time.deltaTime);
            }

            //move sideways
            rb.AddRelativeForce((1 * Time.deltaTime * speed * direction), 0, 0);
            //rb.velocity += Vector3.up * rb.velocity.y * 0.01f * Time.deltaTime;

            /* There's an issue with this. It only works if the enemy 2 object ignores raycast, butit need to be hit by the players raycasts.
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
            */

        }
    }

    //if the enemy hits a wall or something it will reverse direction
    void OnCollisionEnter(Collision col)
    {
        direction *= -1;
    }

    //does damage to enemy
    public void doDamage(){
        HP -= 20;

        if (HP <= 0)
        {
            //Plays death animation and destroys gameobject
            animationController.SetBool("Fly Forward", false);
            animationController.SetBool("Die", true);

            gameController.addKill();
            gameController.playAudio(GetComponent<AudioSource>(), "Explosion");
            GameObject deathFxClone = Instantiate(deathFX, transform.position, transform.rotation);
            Destroy(deathFxClone, 3.8f);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Enemy_Shoot_Bullet.shooting = false;
            dead = true;
            Destroy(transform.GetChild(0).gameObject);
            Destroy(gameObject, 1.6f);

        }
        else {
            animationController.Play("TakeDamage");

        }

        //plays hit noise
        if (gameController.betterAudio)
        {
            gameController.playAudio(GetComponent<AudioSource>(), "Better Enemy Hit"); 
        }
        else
        {
            gameController.playAudio(GetComponent<AudioSource>(), "Enemy Hit"); 
        }
        
    }
}
