using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTargets : MonoBehaviour
{
    public int targetsLeft = 3;

    public GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    public void hit()
    {
        targetsLeft -= 1;
        gameController.playAudio(GetComponent<AudioSource>(), "Explosion");
        if (targetsLeft == 0) {
            Debug.Log("all dead");
            Destroy(GameObject.Find("Barrier2"));

        }
    }
}
