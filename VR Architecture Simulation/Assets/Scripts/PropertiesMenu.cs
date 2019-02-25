using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesMenu : MonoBehaviour
{
    public GameObject currentPart;
    public Transform materialHolder, tabHolder;
    public GameObject materialButton, tabButton;
    public List<GameObject> materialButtons, tabButtons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(GameObject target)
    {
        for(int i = 0; i < materialButtons.Count; i++)
        {
            materialButtons.RemoveAt(0);
        }
        foreach(Transform child in target.transform)
        {
            materialButtons.Add(Instantiate(materialButton, materialHolder));
        }
    }
}
