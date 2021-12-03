using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class medHealth : MonoBehaviour
{
    public GameObject player;
    public float verticalRange = 0.5f;
    public float turnSpeed = 0.2f;
    
    GameController gameController;
    float direction = 1;
    float maxHeight;
    float minHeight;

    // Start is called before the first frame update
    void Start()
    {

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
    
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player") 
        {
            Debug.Log(col.gameObject.tag);
            GameObject.Find("/Player/Cylinder").GetComponent<PlayerHealth>().takeHealth(50);
            Destroy(gameObject);
        }
    }
}
