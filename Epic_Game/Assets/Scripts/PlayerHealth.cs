using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //variables
    public GameObject player;
    public int maxHealth = 100;
    public GameObject healthBar;
    public bool invincible = false;
    public float invincibilityLength = 1;
    private float invinicbilityTimer = 0;
    public int health;
    private float healthBarSizeX;

    // Start is called before the first frame update
    void Start()
    {
        //Sets up health and healthbar size variables
        health = maxHealth;
        healthBarSizeX = healthBar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {   
        //checks if player is invincible and turns off invincibility after a given time
        if(invincible)
        {
           invinicbilityTimer += Time.deltaTime;
           if(invinicbilityTimer >= invincibilityLength) 
           {
               invincible = false;
               invinicbilityTimer = 0;
           }
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy1")
        {
            takeDamage(20);
        }
        
    }
    void OnTriggerEnter(Collider trig)
    {
        Debug.Log("trigger");
        if(trig.gameObject.tag == "Fatal")
        {
            
            //Instantly kills player
            takeDamage(1000);
        }
    }

    //deals removes health points and updates health bar
    void takeDamage(int damage)
    {
        
        if(!invincible || damage > 100)
        {
            health -= damage;
            //changes the healthbar's scale based on percent health. Used this like to figure out how to change scale
            //https://answers.unity.com/questions/805594/how-do-i-change-an-objects-scale-in-code.html
            float percentHealth = (float)damage/maxHealth;
            healthBar.transform.localScale -= new Vector3(healthBarSizeX*(percentHealth), 0, 0);

            //makes player temporarily invincible
            invincible = true;
        }
        if(health <= 0)
        {
            //Resets the scene
            Debug.Log("You Died");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}