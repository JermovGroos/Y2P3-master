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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(ChangeHorIndex(1));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(ChangeHorIndex(-1));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(ChangeVerIndex(-1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(ChangeVerIndex(1));
        }
        if (Input.GetKeyDown(KeyCode.Space))
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
            newVal = -200;
            newVal *= selectedVerIndex - (shopButtons.Count - 1);
            itemHolder.localPosition += new Vector3(0, newVal);
            selectedVerIndex = shopButtons.Count - 1;
            print(newVal);
        }
    }
    float CalcVerDistance()
    {
        GameObject firstButton = Instantiate(shopItem, itemHolder);
        GameObject secondButton = Instantiate(shopItem, itemHolder);
        verTileDistance = secondButton.transform.position.y - firstButton.transform.position.y;
        Destroy(firstButton);
        Destroy(secondButton);
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
