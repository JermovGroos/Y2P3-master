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
    public int selectedHorIndex;
    public int selectedVerIndex;
    float verTileDistance;
    public GameObject[] selectionTabs;
    public Animation[] indicatorHolders;
    public List<GameObject> shopButtons = new List<GameObject>();
    public Transform sectionHolder;
    public Transform itemHolder;
    public float tickDelay;
    public int ticks;
    public GameObject shopItem;
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> listedSelecTabs = new List<GameObject>();
        foreach(Transform child in sectionHolder)
        {
            listedSelecTabs.Add(child.gameObject);
        }
        selectionTabs = listedSelecTabs.ToArray();
        StartCoroutine(Open());
    }

    // Update is called once per frame
    void Update()
    {
        //VR
        if (canMove)
        {
            if (changeTabButton.GetState(InputMan.rightHand))
            {
                int x = Mathf.RoundToInt(changeTabTrackpad.axis.x);
                if (x != 0)
                {
                    if (x == 1)
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
                    print(y);
                    if (y != 0)
                    {
                        if (y == 1)
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

            //PC
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    StartCoroutine(ChangeHorIndex(1));
                }
                else
                {
                    StartCoroutine(ChangeHorIndex(-1));
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    StartCoroutine(ChangeVerIndex(1));
                }
                else
                {
                    StartCoroutine(ChangeVerIndex(-1));
                }
            }
            if (selectButton.GetStateDown(InputMan.rightHand) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                shopButtons[selectedVerIndex].GetComponent<ItemButton>().Select();
            }
        }


    }
    public void SpawnObject(GameObject objectToPlace)
    {
        placingSystem.SetTrackingObject(Instantiate(objectToPlace, Vector3.zero, Quaternion.identity));
    }
    public IEnumerator ChangeHorIndex(int changeAmount)
    {
        canMove = false;
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
        float moveAmount = selectionTabs[previousHorIndex].transform.localPosition.x - selectionTabs[selectedHorIndex].transform.localPosition.x;
        moveAmount /= ticks;
        StartCoroutine(ClearShopItems());
        for (int i = 0; i < ticks; i++)
        {
            sectionHolder.localPosition += (new Vector3(moveAmount, 0));
            yield return new WaitForSeconds(tickDelay);
        }
        StartCoroutine(UpdateShopItems());
        FixVerIndex();
    }

    public IEnumerator ChangeVerIndex(int changeAmount)
    {
        canMove = false;
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
        float moveAmount = shopButtons[previousVerIndex].transform.localPosition.y - shopButtons[selectedVerIndex].transform.localPosition.y;
        moveAmount /= ticks;

        for (int i = 0; i < ticks; i++)
        {
            itemHolder.localPosition += (new Vector3(0, moveAmount));
            yield return new WaitForSeconds(tickDelay);
        }
        canMove = true;
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
    IEnumerator UpdateShopItems()
    {
        GameObject newItem = null;
        foreach (Item item in selectionTabs[selectedHorIndex].GetComponent<ShopTabData>().tabItems)
        {
            newItem = Instantiate(shopItem, itemHolder);
            shopButtons.Add(newItem);
            newItem.GetComponent<ItemButton>().itemData = item;
            newItem.GetComponent<Animation>().Play("ShopItemAppear");
            yield return new WaitForSeconds(newItem.GetComponent<Animation>().clip.length / 4);
        }
        if(newItem != null)
        {
            yield return new WaitForSeconds(newItem.GetComponent<Animation>().clip.length / 4 * 3);
        }
        canMove = true;
    }
    public IEnumerator Open()
    {
        canMove = false;
        for(int i = 0; i < indicatorHolders.Length; i++)
        {
            indicatorHolders[i].Play();
        }
        for(int i = 0; i < selectionTabs.Length; i++)
        {
            selectionTabs[i].GetComponent<Animation>().Play();
            yield return new WaitForSeconds(selectionTabs[i].GetComponent<Animation>().clip.length / 4);
        }
        StartCoroutine(UpdateShopItems());
    }
    public IEnumerator ClearShopItems()
    {
        for (int i = shopButtons.Count - 1; i >= 0; i--)
        {
            GameObject button = shopButtons[i];
            button.GetComponent<Animation>().Play("ShopItemDisappear");
            yield return new WaitForSeconds(button.GetComponent<Animation>().GetClip("ShopItemDisappear").length);
            Destroy(button);
        }
        shopButtons = new List<GameObject>();
    }
}
