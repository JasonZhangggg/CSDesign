using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjective : MonoBehaviour
{
    public GameController gameController;
    private string winCondition;
    // Start is called before the first frame update
    void Start(){
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    void updateUI() {
        //gets game controller script

        //Sets the win condition text 
        if (gameController.objText[gameController.level][gameController.winPart] == "Kill")
        {
            winCondition = "Kill " + (gameController.winValues[gameController.level][gameController.winPart] - gameController.enemiesKilled) + " Enemies";
        }
        else if (gameController.objText[gameController.level][gameController.winPart] == "Time")
        {
            winCondition = "Survive " + (gameController.winValues[gameController.level][gameController.winPart] - gameController.timeElapsed)+ " Seconds";
        }
        else if (gameController.objText[gameController.level][gameController.winPart] == "Collect")
        {
            winCondition = "Collect " + (gameController.winValues[gameController.level][gameController.winPart] - gameController.keysCollected) + " Keys";
        }
        else
        {
            winCondition = gameController.objText[gameController.level][gameController.winPart];
        }

        //Applies the win condition text to the UI
        GetComponent<UnityEngine.UI.Text>().text = winCondition;
    }
    // Update is called once per frame
    void Update()
    {
        updateUI();
    }
}
