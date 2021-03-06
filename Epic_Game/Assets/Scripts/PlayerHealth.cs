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
    public float invincibilityLength = 0.5f;
    private float invinicbilityTimer = 0;
    public bool permaInvincible = false;
    public int health;
    private float healthBarSizeX;
    public GameObject gameController;
    public Color32 healthColor;
    public float N_power;
    public float N_root;

    

    // Start is called before the first frame update
    void Start()
    {
        //Sets up health and healthbar size variables
        health = maxHealth;
        healthBarSizeX = healthBar.transform.localScale.x;
        healthColor = healthBar.GetComponent<Image>().color;
        gameController = GameObject.Find("Game Controller");
    }

    // Update is called once per frame
    void Update()
    {   
        //checks if player is invincible and turns off invincibility after a given time
        if(invincible && !permaInvincible)
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
        //when you hit an enemy or a bullet you take damage
        if(col.gameObject.tag == "Enemy1" || col.gameObject.tag == "Enemy3")
        {
            takeDamage(20);
            Vector3 force = transform.position -  col.transform.position;
            GameObject.Find("Player").GetComponent<playerMovement>().AddImpact(force, 200);
        }
        if(col.gameObject.tag == "Enemy_Bullet")
        {
            takeDamage(10);
        }
        
    }
    void OnTriggerEnter(Collider trig)
    {
        if(trig.gameObject.tag == "Fatal")
        {
            
            //Instantly kills player
            takeDamage(1000);
        }
        if(trig.gameObject.tag == "Key")
        {
            if(health < maxHealth/2)
            {
                //heals player to half health
                takeDamage(health - (maxHealth/2));
            }

            
        }
    }

    //deals removes health points and updates health bar
    public void takeDamage(int damage)
    {
        if (!invincible || damage > 100)
        {
            health -= damage;
            //makes player temporarily invincible
            invincible = true;
            healthUpdate(damage);
        }
        
    }

    public void takeHealth(int healing)
    {
        if (health + healing > maxHealth)
        {
            healing = (maxHealth - health);
        }
        health += healing;
        healthUpdate(healing * -1);
    }

    void healthUpdate(int delta)
    {
        N_root = (float)Mathf.Pow(((float)health / 100f), (1f / 1.5f));
        N_power = (float)Mathf.Pow(((float)health / 100f), 4);

        //changes the healthbar's scale based on percent health. Used this like to figure out how to change scale
        //https://answers.unity.com/questions/805594/how-do-i-change-an-objects-scale-in-code.html
        float percentHealth = (float)delta / maxHealth;
        healthBar.transform.localScale -= new Vector3(healthBarSizeX * (percentHealth), 0, 0);

        if (health <= 40)
        {
            //swag
        }
        if (health <= 0)
        {
            gameController.GetComponent<GameController>().playerDied();
        }

        if (health < 50)
        {
            healthBar.GetComponent<Image>().color = Color.Lerp(Color.red, Color.yellow, (float)N_root);
        }
        else if (health >= 50)
        {
            healthBar.GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.green, (float)N_power);
        }
    }
}
