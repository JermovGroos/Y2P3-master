using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Options : MonoBehaviour
{
    public Text gridDivText;
    public Text rotSnapText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateGridDivision(Placer.placer.divisionAmount);
        //UpdateRotationSnap(Placer.placer.rotateTurnAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGridDivision(int newDivValue)
    {
        gridDivText.text = newDivValue.ToString();
    }

    public void UpdateRotationSnap(int newSnapValue)
    {
        rotSnapText.text = newSnapValue.ToString();
    }
    public void ChangePrimaryHand(bool isRightHanded)
    {
        InputMan.ChangePrimaryHand(isRightHanded);
    }

}
