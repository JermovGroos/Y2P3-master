using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class UISelection : MonoBehaviour
{

    public List<TwoDemensionalGOList> selectableOptions = new List<TwoDemensionalGOList>();
    int currentHorIndex;
    int currentVerIndex;
    UIButton currentSelected;
    public Vector2 outlineScale;
    public SteamVR_Action_Boolean acceptButton, selectButton;
    public SteamVR_Action_Vector2 trackpadPos;

    // Start is called before the first frame update
    void Start()
    {
        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButton>();
        Outline outline = currentSelected.gameObject.AddComponent<Outline>();
        outline.effectDistance = outlineScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 changeAmount = new Vector2();
        if (selectButton.GetLastStateDown(InputMan.rightHand))
        {
            int rawAxisX = Mathf.RoundToInt(trackpadPos.axis.x);
            int rawAxisY = Mathf.RoundToInt(trackpadPos.axis.y);
            if (rawAxisX != 0)
            {
                changeAmount.x = rawAxisX;
            }
            if (rawAxisY != 0)
            {
                changeAmount.y = rawAxisY;
            }
            ChangeSelectPos(changeAmount);
        }

        if (acceptButton.GetStateDown(InputMan.rightHand))
        {
            currentSelected.Interact();
        }
    }
    public void ChangeSelectPos(Vector2 changeAmount)
    {
        currentVerIndex -= (int)changeAmount.y;
        Destroy(currentSelected.gameObject.GetComponent<Outline>());
        if(currentVerIndex < 0)
        {
            currentVerIndex = selectableOptions.Count - 1;
        }
        else
        {
            if(currentVerIndex >= selectableOptions.Count)
            {
                currentVerIndex = 0;
            }
        }

        currentHorIndex += (int)changeAmount.x;
        if (currentHorIndex < 0)
        {
            currentHorIndex = selectableOptions[currentVerIndex].xIndexes.Count;
        }
        else
        {
            if (currentHorIndex >= selectableOptions[currentVerIndex].xIndexes.Count)
            {
                currentHorIndex = 0;
            }
        }

        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButton>();
        Outline outline = currentSelected.gameObject.AddComponent<Outline>();
        outline.effectDistance = outlineScale;
        print(currentSelected.gameObject.name);
    }
    [System.Serializable]
    public struct TwoDemensionalGOList
    {
        public List<GameObject> xIndexes;
    }
}
