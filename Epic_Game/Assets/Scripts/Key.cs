using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float verticalRange = 0.5f;
    public float turnSpeed = 0.2f;
    public Rigidbody rb;
    
    GameController gameController;
    float direction = 1;
    float maxHeight;
    float minHeight;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        maxHeight = transform.position.y + verticalRange;
        minHeight = transform.position.y - verticalRange;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(new Vector3(0, verticalRange * Time.deltaTime * direction * speed, 0));
        transform.position += new Vector3(0, verticalRange * Time.deltaTime * direction, 0);
        if((transform.position.y > maxHeight && direction == 1) || (transform.position.y < minHeight && direction ==-1))
        {
            direction *= -1;
        }

        transform.Rotate(0f, 360f * Time.deltaTime * turnSpeed, 0f, Space.Self);
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            gameController.playAudio(col.gameObject.GetComponent<AudioSource>(), "Key Collect"); 
            gameController.keyCollected();
            Destroy(gameObject);
        }
        
    }
}
