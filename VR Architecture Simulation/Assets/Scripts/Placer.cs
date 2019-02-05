using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    public GameObject trackingObj;
    public LayerMask snapMask, nonSnapMask;
    public bool snapping;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trackingObj)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                snapping = true;
            }
            else
            {
                if (Input.GetButtonUp("Sprint"))
                {
                    snapping = false;
                }
            }
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
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
        }
    }
    public void SetTrackingObject(GameObject thisObject)
    {
        trackingObj = thisObject;
    }
}
