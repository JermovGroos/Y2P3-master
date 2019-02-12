using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    public SteamVR_Action_Boolean interactButton;
    public SteamVR_Input_Sources leftHandSource;
    public SteamVR_Action_Boolean teleportButton;
    public SteamVR_Input_Sources rightHandSource;
    LineRenderer pointer;
    bool teleporting;
    // Start is called before the first frame update
    void Start()
    {
        pointer = rightHand.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitPoint;
        if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hitPoint))
        {
            pointer.SetPosition(0, rightHand.transform.position);
            pointer.SetPosition(1, hitPoint.point);
            if (interactButton.GetStateDown(rightHandSource))
            {
                if (hitPoint.transform.tag == "Interactable")
                {
                    hitPoint.transform.GetComponent<Interactable>().Interact();
                }
            }
            if (teleportButton.GetStateUp(leftHandSource))
            {
                if(hitPoint.transform.tag == "Ground")
                {
                    if (!teleporting)
                    {
                        StartCoroutine(Teleport(hitPoint.point));
                    }
                }
            }
        }
        else
        {
            pointer.SetPosition(0, rightHand.transform.position);
            pointer.SetPosition(1, rightHand.transform.forward);
        }
    }
    IEnumerator Teleport(Vector3 newPosition)
    {
        teleporting = true;
        UIManager.uiManager.ScreenFade(true);
        yield return new WaitForSeconds(UIManager.uiManager.fadeAnimation.clip.length);
        transform.position = newPosition;
        UIManager.uiManager.ScreenFade(false);
        yield return new WaitForSeconds(UIManager.uiManager.fadeAnimation.clip.length);
        teleporting = false;
    }
}
