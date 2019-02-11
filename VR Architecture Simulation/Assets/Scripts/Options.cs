using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Text gridDivText;
    public GameObject decreaseDiv, increaseDiv;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateGridDivision(int newDivValue)
    {
        gridDivText.text = newDivValue.ToString();
        switch (newDivValue)
        {
            case 10:
                increaseDiv.SetActive(false);
                break;
            case 9:
                increaseDiv.SetActive(true);
                break;
            case 1:
                decreaseDiv.SetActive(false);
                break;
            case 2:
                decreaseDiv.SetActive(true);
                break;
        }
    }
}
