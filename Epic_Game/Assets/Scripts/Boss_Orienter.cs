using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Orienter : MonoBehaviour
{
    GameObject Boss;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
