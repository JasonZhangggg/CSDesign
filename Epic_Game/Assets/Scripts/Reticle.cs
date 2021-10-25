using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    private GameController gameController;
    private Text reticle;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Text reticle = GetComponent<Text>();
        reticle.fontSize = gameController.reticalSize;
        reticle.color = gameController.reticalColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
