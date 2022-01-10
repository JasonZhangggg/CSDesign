using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public float maxHealth;
    public float health;
    public Text healthText;
    private float healthBarSizeX;

    Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        //gets some values for variables it needs
        healthBarSizeX = healthBar.transform.localScale.x;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        health = boss.health;
        maxHealth = boss.health;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the boss health has changed, then updates health bar accordingly
        if(boss.health != health)
        {
            healthUpdate(health - boss.health);
        }

        
    }

    //updates boss healthbar
    public void healthUpdate(float delta)
    {
        healthText.text = "Boss " + health + "/" + maxHealth;
        health = boss.health;
        //yes, I just copied the code from player health
        float percentHealth = (float)delta / maxHealth;
        healthBar.transform.localScale -= new Vector3(healthBarSizeX * (percentHealth), 0, 0);
    }
}
