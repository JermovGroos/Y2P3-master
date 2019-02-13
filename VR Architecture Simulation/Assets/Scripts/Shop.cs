using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Placer placingSystem;
    public SteamVR_Action_Vector2 changeTabTrackpad;
    public SteamVR_Action_Boolean changeTabButton;
    public SteamVR_Action_Boolean selectButton;
    public SteamVR_Input_Sources changeTabSource;
    public int selectedHorIndex;
    public int selectedVerIndex;
    float verTileDistance;
    public GameObject[] selectionTabs;
    public List<GameObject> shopButtons = new List<GameObject>();
    public Transform sectionHolder;
    public Transform itemHolder;
    public float tickDelay;
    public int ticks;
    public GameObject shopItem;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> listedSelecTabs = new List<GameObject>();
        foreach(Transform child in sectionHolder)
        {
            listedSelecTabs.Add(child.gameObject);
        }
        selectionTabs = listedSelecTabs.ToArray();
        UpdateShopItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeTabButton.GetStateDown(changeTabSource))
        {
            int x = Mathf.RoundToInt(changeTabTrackpad.axis.x);
            if (x != 0)
            {
                if(x == 1)
                {
                    StartCoroutine(ChangeHorIndex(1));
                }
                else
                {
                    StartCoroutine(ChangeHorIndex(-1));
                }
            }
            else
            {
                int y = Mathf.RoundToInt(changeTabTrackpad.axis.y);
                if (y != 0)
                {
                    if(y == 1)
                    {
                        StartCoroutine(ChangeVerIndex(-1));
                    }
                    else
                    {
                        StartCoroutine(ChangeVerIndex(1));
                    }
                }
            }
        }
        if (selectButton.GetStateDown(changeTabSource))
        {
            shopButtons[selectedVerIndex].GetComponent<ItemButton>().Select();
        }
    }
    public void SpawnObject(GameObject objectToPlace)
    {
        placingSystem.SetTrackingObject(Instantiate(objectToPlace, Vector3.zero, Quaternion.identity));
    }
    public IEnumerator ChangeHorIndex(int changeAmount)
    {
        int previousHorIndex = selectedHorIndex;
        selectedHorIndex += changeAmount;
        if(selectedHorIndex < 0)
        {
            selectedHorIndex = selectionTabs.Length - 1;
        }
        else
        {
            if(selectedHorIndex >= selectionTabs.Length)
            {
                selectedHorIndex = 0;
            }
        }
        float moveAmount = selectionTabs[previousHorIndex].transform.position.x - selectionTabs[selectedHorIndex].transform.position.x;
        moveAmount /= ticks;

        for(int i = 0; i < ticks; i++)
        {
            sectionHolder.Translate(new Vector2(moveAmount, 0));
            yield return new WaitForSeconds(tickDelay);
        }
        UpdateShopItems();
        FixVerIndex();
    }

    public IEnumerator ChangeVerIndex(int changeAmount)
    {
        int previousVerIndex = selectedVerIndex;
        selectedVerIndex += changeAmount;
        if (selectedVerIndex < 0)
        {
            selectedVerIndex = shopButtons.Count - 1;
        }
        else
        {
            if (selectedVerIndex >= shopButtons.Count)
            {
                selectedVerIndex = 0;
            }
        }
        float moveAmount = shopButtons[previousVerIndex].transform.position.y - shopButtons[selectedVerIndex].transform.position.y;
        moveAmount /= ticks;

        for (int i = 0; i < ticks; i++)
        {
            itemHolder.Translate(new Vector2(0, moveAmount));
            yield return new WaitForSeconds(tickDelay);
        }
    }
    void FixVerIndex()
    {
        if(selectedVerIndex >= shopButtons.Count)
        {
            float newVal = CalcVerDistance();
            newVal *= selectedVerIndex - (shopButtons.Count - 1);
            itemHolder.localPosition += new Vector3(0, newVal);
            selectedVerIndex = shopButtons.Count - 1;
            print(newVal);
        }
    }
    float CalcVerDistance()
    {
        verTileDistance = -(shopItem.GetComponent<RectTransform>().rect.height + itemHolder.GetComponent<VerticalLayoutGroup>().spacing);
        return (verTileDistance);
    }
    void UpdateShopItems()
    {
        foreach(GameObject button in shopButtons)
        {
            Destroy(button);
        }
        shopButtons = new List<GameObject>();
        foreach(Item item in selectionTabs[selectedHorIndex].GetComponent<ShopTabData>().tabItems)
        {
            GameObject newItem = Instantiate(shopItem, itemHolder);
            shopButtons.Add(newItem);
            newItem.GetComponent<ItemButton>().itemData = item;
        }
    }
}
