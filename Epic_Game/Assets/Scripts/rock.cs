using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    //effect when rock breaks
    public GameObject breakFX;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag != "Boss")
        {
            if(col.gameObject.tag == "Player")
            {
                col.transform.GetChild(0).gameObject.GetComponent<PlayerHealth>().takeDamage(20);
            }
            GameObject FX = Instantiate(breakFX, transform.position, Quaternion.identity);
            Destroy(FX, 2);
            Destroy(gameObject);

        }

    }
}
