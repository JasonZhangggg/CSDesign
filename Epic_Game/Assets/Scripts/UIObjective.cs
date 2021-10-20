using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjective : MonoBehaviour
{
    public GameController gameController;
    private string winCondition;
    // Start is called before the first frame update
    void Start()
    {
        //gets game controller script
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        //Sets the win condition text 
        if(gameController.winConditions[gameController.level] == 1)
        {
            winCondition = "Kill " + gameController.winValues[gameController.level] + " Enemies";
        }
        else if(gameController.winConditions[gameController.level] == 2)
        {
            winCondition = "Survive " + gameController.winValues[gameController.level] + " Seconds";
        }

        //Applies the win condition text to the UI
        GetComponent<UnityEngine.UI.Text>().text = "Objective: " + winCondition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
