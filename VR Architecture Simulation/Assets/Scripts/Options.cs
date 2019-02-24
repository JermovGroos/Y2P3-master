using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Text gridDivText;
    public List<TwoDemensionalGOList> selectableOptions = new List<TwoDemensionalGOList>();
    int currentHorIndex;
    int currentVerIndex;
    UIButton currentSelected;
    // Start is called before the first frame update
    void Start()
    {
        UpdateGridDivision(Placer.placer.divisionAmount);
        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButton>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 changeAmount = new Vector2();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeAmount.x = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            changeAmount.y = -1;
        }
        if(changeAmount.x != 0 || changeAmount.y != 0)
        {
            ChangeSelectPos(changeAmount);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSelected.Interact();
        }
    }
    public void ChangeSelectPos(Vector2 changeAmount)
    {
        currentVerIndex += (int)changeAmount.y;
        currentHorIndex += (int)changeAmount.x;

        if(currentVerIndex == selectableOptions.Count)
        {
            currentVerIndex = 0;
        }
        else
        {
            if(currentVerIndex < 0)
            {
                currentVerIndex = selectableOptions.Count - 1;
            }
        }
        if(currentHorIndex == selectableOptions[currentVerIndex].xIndexes.Count)
        {
            currentHorIndex = 0;
        }
        else
        {
            if(currentHorIndex < 0)
            {
                currentHorIndex = selectableOptions[currentVerIndex].xIndexes.Count - 1;
            }
        }
        currentSelected = selectableOptions[currentVerIndex].xIndexes[currentHorIndex].GetComponent<UIButton>();
        print(currentSelected.gameObject.name);
    }
    public void UpdateGridDivision(int newDivValue)
    {
        gridDivText.text = newDivValue.ToString();
    }

    public void ChangePrimaryHand(bool isRightHanded)
    {
        InputMan.ChangePrimaryHand(isRightHanded);
    }

    [System.Serializable]
    public struct TwoDemensionalGOList
    {
        public List<GameObject> xIndexes;
    }
}
