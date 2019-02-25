﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
    [Header("Shop")]
    public GameObject shop;
    public GameObject settings;
    public SteamVR_Action_Boolean shopToggleButton;
    bool toggledShop;
    [Header("ScreenFade")]
    public Animation fadeAnimation;
    public AnimationClip fadeAppear;
    public AnimationClip fadeRemove;
    // Start is called before the first frame update
    private void Awake()
    {
        uiManager = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (shopToggleButton.GetStateDown(InputMan.leftHand))
        {
            ToggleShop();
        }
    }
    public void ScreenFade(bool appear)
    {
        if (appear)
        {
            fadeAnimation.clip = fadeAppear;
        }
        else
        {
            fadeAnimation.clip = fadeRemove;
        }
        fadeAnimation.Play();
    }
    public void ToggleShop()
    {
        if (toggledShop)
        {
            shop.SetActive(false);
            toggledShop = false;
        }
        else
        {
            shop.SetActive(true);
            toggledShop = true;
        }
    }
    public void DisableUI(GameObject toDisable)
    {
        toDisable.SetActive(false);
    }
    public void EnableUI(GameObject toEnable)
    {
        toEnable.SetActive(true);
    }
}
