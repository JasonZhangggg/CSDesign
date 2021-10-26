using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTargets : MonoBehaviour
{
    public int targetsLeft = 3;

    // Update is called once per frame
    public void hit()
    {
        targetsLeft -= 1;
        if (targetsLeft == 0) {
            Debug.Log("all dead");
            Destroy(GameObject.Find("Barrier2"));

        }
    }
}
