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
    public GameObject[] selectionTabs;
    public Transform sectionHolder;
    public float tickDelay;
    public int ticks;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> listedSelecTabs = new List<GameObject>();
        foreach(Transform child in sectionHolder)
        {
            listedSelecTabs.Add(child.gameObject);
        }
        selectionTabs = listedSelecTabs.ToArray();
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
            if(selectedHorIndex == selectionTabs.Length)
            {
                selectedHorIndex = 0;
            }
        }
        float moveAmount = selectionTabs[previousHorIndex].transform.position.x - selectionTabs[selectedHorIndex].transform.position.x;
        moveAmount /= ticks;

        for(int i = 0; i < ticks; i++)
        {
            sectionHolder.transform.Translate(new Vector2(moveAmount, 0));
            yield return new WaitForSeconds(tickDelay);
        }
    }
}
