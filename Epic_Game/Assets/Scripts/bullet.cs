using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        Debug.Log(collision.collider.name);
        Destroy(gameObject);
        if(collision.gameObject.tag == "Enemy"){
            collision.gameObject.GetComponent<Enemy1>().doDamage();
        }
    }
}
