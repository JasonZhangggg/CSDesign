using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameController gameController;
    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    void OnCollisionEnter(Collision collision){
        Destroy(gameObject);
        if(collision.gameObject.tag == "Enemy1"){
            collision.gameObject.GetComponent<Enemy1>().doDamage();
        }
        if(collision.gameObject.tag == "Enemy2"){
            collision.gameObject.GetComponent<Enemy2>().doDamage();
        }
        if (collision.gameObject.tag == "Target") {
            Destroy(collision.gameObject);
            gameController.addKill();
            GameObject.Find("Targets").GetComponent<PracticeTargets>().hit();
        }
    }
}
