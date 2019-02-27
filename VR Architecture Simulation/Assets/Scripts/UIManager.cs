using System.Collections;
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
    public GameObject properties;
    public SteamVR_Action_Boolean shopToggleButton;
    public SteamVR_Action_Boolean settingsToggleButton;
    bool toggledShop;
    bool toggledSettings;
    bool toggledProperties;
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
        if (settingsToggleButton.GetStateDown(InputMan.rightHand))
        {
            ToggleSettings();
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
            StartCoroutine(shop.GetComponent<Shop>().Open());
            toggledShop = true;
        }
    }
    public void ToggleSettings()
    {
        if (toggledSettings)
        {
            settings.SetActive(false);
            toggledSettings = false;
        }
        else
        {
            settings.SetActive(true);
            toggledSettings = true;
        }
    }
    public void ToggleProperties(GameObject item)
    {
        if (toggledProperties)
        {
            properties.SetActive(false);
        }
        else
        {
            properties.SetActive(true);
        }
        toggledProperties = toggledProperties.ToggleBool();
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
