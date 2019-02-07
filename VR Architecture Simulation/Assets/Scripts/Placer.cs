using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Placer : MonoBehaviour
{
    public SystemManager sysMan;
    public GameObject trackingObj;
    public LayerMask snapMask, nonSnapMask;
    public Transform hand;
    public bool snapping;
    public SteamVR_Action_Boolean snapButton, placeButton;
    public SteamVR_Input_Sources inputSource;
    public SteamVR_Action_Vector2 rotateButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trackingObj)
        {
            if (placeButton.GetStateDown(inputSource))
            {
                trackingObj = null;
            }
            else
            {
                if (snapButton.GetStateDown(inputSource))
                {
                    snapping = true;
                    sysMan.ToggleGrid(true);
                }
                else
                {
                    if (snapButton.GetStateUp(inputSource))
                    {
                        snapping = false;
                        sysMan.ToggleGrid(false);
                    }
                }
                RaycastHit hit;
                Ray ray = new Ray(hand.position, hand.forward);
                Vector3 placePos = Vector3.zero;
                if (snapping)
                {
                    if (Physics.Raycast(ray, out hit, 1000, snapMask))
                    {
                        placePos = hit.transform.position;
                        trackingObj.transform.position = placePos;
                    }
                }
                else
                {

                    if (Physics.Raycast(ray, out hit, 1000, nonSnapMask))
                    {
                        placePos = hit.point;
                        trackingObj.transform.position = placePos;
                    }
                }
                trackingObj.transform.Rotate(new Vector3(0, -rotateButton.GetAxis(inputSource).x, 0));
            }
        }
    }
    public void SetTrackingObject(GameObject thisObject)
    {
        trackingObj = thisObject;
    }
}
