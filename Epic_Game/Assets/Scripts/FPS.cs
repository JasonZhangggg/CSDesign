﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text fpsText;
    public float deltaTime;
    void Start() {
        fpsText = GameObject.Find("/HUD/FPS").GetComponent<Text>();
    }
    void Update()
    {
        if(Time.timeScale != 0)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString();
        }
    }
}
