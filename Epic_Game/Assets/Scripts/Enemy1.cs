using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class Enemy1 : MonoBehaviour
{
    public GameObject player;
    public bool charging = false;
    public float windupTime;
    public float timer = 0;
    private float speed = 30;
    public Rigidbody rb;
    public int HP = 100;
    public GameController gameController;
    Vector3 targetPos;
    private float windUpMax=5f;
    private float windUpMin=2f;
    public bool startAttacking = false;
    public Slider slider;

    public float inRange;
    public Animator animationController;

    private bool dead = false;

    // Start is called before the first frame update
    void Start()    
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        windupTime = Random.Range(windUpMin, windUpMax);
}

// Update is called once per frame
void Update()
    {
        slider.value = (float) HP / 100;
        if(animationController.GetCurrentAnimatorStateInfo(0).IsName("Take Damage"))
        if (HP <= 50) {
            speed = 50;
            windUpMin = 1;
            windUpMax = 2.5f;
        }
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= inRange) startAttacking = true;
        if (startAttacking && !dead) { 
            if (!charging)
            {
                rb.velocity = Vector3.zero;
                transform.LookAt(player.transform);
                timer += Time.deltaTime;
                if (timer >= windupTime)
                {
                    timer = 0;
                    targetPos = player.transform.position;
                    animationController.SetBool("Run Forward", true);
                    charging = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                //charges at player
                if (targetPos == transform.position)
                {
                    animationController.SetBool("Run Forward", false);
                    windupTime = Random.Range(windUpMin, windUpMax);
                    charging = false;
                }
            }
        }
        
    }
    public void doDamage(){
        HP -= 20;

        if (gameController.betterAudio)
        {
            gameController.playAudio(GetComponent<AudioSource>(), "Better Enemy Hit"); 
        }
        else
        {
            gameController.playAudio(GetComponent<AudioSource>(), "Enemy Hit"); 
        }
        if (HP <= 0)
        {
            animationController.SetBool("Run Forward", false);
            animationController.SetBool("Die", true);

            gameController.addKill();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            gameController.playAudio(GetComponent<AudioSource>(), "Explosion");
            dead = true;
            Destroy(transform.GetChild(0).gameObject);
            Destroy(gameObject, 1.8f);

        }
        else
        {
            animationController.SetTrigger("Take Damage");
        }

    }
}
