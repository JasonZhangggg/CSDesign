using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public GameObject player;
    public float speed = 100;
    public Rigidbody rb;
    public int HP = 100;
    public GameObject gameController;

    bool traveling = false;
    public float maxZ;
    public float minZ;
    public float maxX;
    public float minX;
    float targetZ;
    float targetX;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        if(!traveling)
        {
            targetX = Random.Range(minX, maxX);
            targetZ = Random.Range(minZ, maxZ);
            transform.position += new Vector3((targetX - transform.position.x) * Time.deltaTime * 0.5f, 0, (targetZ - transform.position.z) * Time.deltaTime * 0.5f);
            traveling = true;
            
        }
        else
        {
            rb.AddForce(new Vector3((targetX - transform.position.x) * Time.deltaTime * speed, 0, (targetZ - transform.position.z) * Time.deltaTime * speed));
        }
    }
        
    
    public void doDamage(){
        HP -= 20;
        if(HP <= 0){
            gameController.GetComponent<GameController>().addKill();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            gameController.GetComponent<GameController>().playAudio(GetComponent<AudioSource>(), "Explosion"); 
            Destroy(gameObject, 1);
        }
    }
}