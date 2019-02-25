using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    public GameObject leftHandGO, rightHandGO;
    public SteamVR_Action_Boolean interactButton;
    public SteamVR_Action_Boolean teleportButton;
    LineRenderer pointer;
    bool teleporting;
    // Start is called before the first frame update
    void Start()
    {
        pointer = rightHandGO.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitPoint;
        if (Physics.Raycast(rightHandGO.transform.position, rightHandGO.transform.forward, out hitPoint))
        {
            pointer.SetPosition(0, rightHandGO.transform.position);
            pointer.SetPosition(1, hitPoint.point);
            if (interactButton.GetStateDown(InputMan.rightHand))
            {
                if (hitPoint.transform.tag == "Interactable")
                {
                    hitPoint.transform.GetComponent<Interactable>().Interact();
                }
            }
            if (teleportButton.GetStateUp(InputMan.leftHand))
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
            pointer.SetPosition(0, rightHandGO.transform.position);
            pointer.SetPosition(1, rightHandGO.transform.forward);
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
