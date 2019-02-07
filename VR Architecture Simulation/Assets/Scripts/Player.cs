using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    public SteamVR_Action_Boolean interactButton;
    public SteamVR_Input_Sources interactSource;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer pointer = rightHand.GetComponent<LineRenderer>();
        RaycastHit hitPoint;
        if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hitPoint))
        {
            pointer.SetPosition(0, rightHand.transform.position);
            pointer.SetPosition(1, hitPoint.point);
            if (interactButton.GetStateDown(interactSource))
            {
                if (hitPoint.transform.tag == "Interactable")
                {
                    hitPoint.transform.GetComponent<Interactable>().Interact();
                }
            }
        }
        else
        {
            pointer.SetPosition(0, rightHand.transform.position);
            pointer.SetPosition(1, rightHand.transform.forward);
        }
    }
}
