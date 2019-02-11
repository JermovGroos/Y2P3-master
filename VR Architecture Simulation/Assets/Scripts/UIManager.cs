using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
    [Header("Shop")]
    public GameObject menuUI;
    public GameObject settings;
    public SteamVR_Action_Boolean shopToggleButton;
    public SteamVR_Input_Sources shopToggleSource;
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
        if (shopToggleButton.GetStateDown(shopToggleSource))
        {
            if (toggledShop)
            {
                menuUI.SetActive(false);
                toggledShop = false;
            }
            else
            {
                menuUI.SetActive(true);
                toggledShop = true;
            }
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
    public void DisableUI(GameObject toDisable)
    {
        toDisable.SetActive(false);
    }
    public void EnableUI(GameObject toEnable)
    {
        toEnable.SetActive(true);
    }
}
