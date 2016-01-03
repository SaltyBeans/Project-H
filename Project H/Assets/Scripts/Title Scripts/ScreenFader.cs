﻿using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GUITexture))]
public class ScreenFader : MonoBehaviour
{
    public float fadeSpeed = 1.5f;         


    private bool sceneStarting = true;      


    void Awake()
    {
        
        GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }


    void Update()
    {

        //if (sceneStarting)
        //    StartScene();

        if (Input.GetKeyDown(KeyCode.F))
        {
            FadeToBlack();
            Debug.Log("F key found");
            Debug.Log(GetComponent<GUITexture>().name);
        }
    }


    void FadeToClear()
    {
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        FadeToClear();

        if (GetComponent<GUITexture>().color.a <= 0.05f)
        {
            GetComponent<GUITexture>().color = Color.clear;
            GetComponent<GUITexture>().enabled = false;

            sceneStarting = false;
        }
    }


    public void EndScene()
    {
        GetComponent<GUITexture>().enabled = true;

        FadeToBlack();

        if (GetComponent<GUITexture>().color.a >= 0.95f)
            Application.LoadLevel(0);
    }
}