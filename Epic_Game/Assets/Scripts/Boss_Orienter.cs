using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Orienter : MonoBehaviour
{
    public GameObject Boss;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
