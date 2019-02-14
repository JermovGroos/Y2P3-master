using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Placer : MonoBehaviour
{
    public static Placer placer;
    public GameObject trackingObj;
    public LayerMask snapMask, nonSnapMask;
    public Transform hand;
    public bool snappingPosition;
    public bool snappingRotation;
    public SteamVR_Action_Boolean positionSnapButton, placeButton;
    public SteamVR_Input_Sources inputSource;
    public SteamVR_Action_Vector2 rotateButton;
    public SteamVR_Action_Boolean rotatePress, rotationSnapButton;
    public int rotateTurnAmount;

    [Range(1, 10)]
    public int divisionAmount;
    public GameObject tile;
    public Vector2 tileSize;
    public Vector2 gridTileSize;
    public List<GameObject> allTiles = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        placer = this;
    }
    private void Start()
    {
        CalculateTilePositions(GameObject.FindGameObjectsWithTag("Ground"));
    }

    // Update is called once per frame
    void Update()
    {
        if (trackingObj)
        {
            if (placeButton.GetStateDown(inputSource))
            {
                StartCoroutine(ClearTrackingObject());
            }
            else
            {
                ToggleGridSnap();
                ToggleRotationSnap();
                RaycastHit hit;
                Ray ray = new Ray(hand.position, hand.forward);
                Vector3 placePos = hand.transform.forward * 5;
                if (snappingPosition)
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
                float rotateAmount = rotateButton.GetAxis(inputSource).x;
                if (snappingRotation)
                {
                    if (rotatePress.GetStateDown(inputSource))
                    {
                        rotateAmount = Mathf.RoundToInt(rotateAmount);
                        rotateAmount *= rotateTurnAmount;
                        trackingObj.transform.Rotate(new Vector3(0, rotateAmount, 0));
                    }
                }
                else
                {
                    if (rotatePress.GetState(inputSource))
                    {
                        trackingObj.transform.Rotate(new Vector3(0, rotateAmount, 0));
                    }
                }
            }
        }
    }
    public void SetTrackingObject(GameObject thisObject)
    {
        if(trackingObj == null)
        {
            trackingObj = thisObject;
            trackingObj.GetComponent<Collider>().enabled = false;
        }
    }
    public IEnumerator ClearTrackingObject()
    {
        GameObject oldTracker = trackingObj;
        trackingObj = null;
        yield return new WaitForSeconds(0.1f);
        oldTracker.GetComponent<Collider>().enabled = true;
    }
    void ToggleGridSnap()
    {
        if (positionSnapButton.GetStateDown(inputSource))
        {
            snappingPosition = true;
            ToggleGrid(true);
        }
        else
        {
            if (positionSnapButton.GetStateUp(inputSource))
            {
                snappingPosition = false;
                ToggleGrid(false);
            }
        }
    }
    void ToggleRotationSnap()
    {
        if (rotationSnapButton.GetStateDown(inputSource))
        {
            snappingRotation = snappingRotation.ToggleBool();
            if (snappingRotation)
            {
                Vector3 snappedRotation = trackingObj.transform.eulerAngles;
                snappedRotation.y = Mathf.RoundToInt(snappedRotation.y / rotateTurnAmount) * rotateTurnAmount;
                trackingObj.transform.eulerAngles = snappedRotation;
            }
        }
    }
    void CalculateTilePositions(GameObject[] groundTiles)
    {
        //UIManager.uiManager.settings.GetComponent<Options>().UpdateGridDivision(divisionAmount);
        DeleteCurrentTiles();
        foreach (GameObject groundTile in groundTiles)
        {
            tileSize.x = Mathf.Abs(groundTile.GetComponent<Collider>().bounds.max.x - groundTile.GetComponent<Collider>().bounds.min.x);
            tileSize.y = Mathf.Abs(groundTile.GetComponent<Collider>().bounds.max.z - groundTile.GetComponent<Collider>().bounds.min.z);
            gridTileSize.x = tileSize.x / divisionAmount;
            gridTileSize.y = tileSize.y / divisionAmount;


            for (int hor = 0; hor < divisionAmount; hor++)
            {
                for (int ver = 0; ver < divisionAmount; ver++)
                {
                    Vector2 newPos = Vector2.zero;
                    newPos.x += (hor * gridTileSize.x);
                    newPos.y += (ver * gridTileSize.y);
                    newPos += gridTileSize / 2;
                    newPos.x += groundTile.GetComponent<Collider>().bounds.min.x;
                    newPos.y += groundTile.GetComponent<Collider>().bounds.min.z;

                    Vector3 newTilePos = new Vector3(newPos.x, groundTile.transform.position.y, newPos.y);
                    GameObject newTile = Instantiate(tile, newTilePos, Quaternion.identity);

                    Vector3 tileSize = new Vector3(gridTileSize.x, newTile.transform.localScale.y, gridTileSize.y);
                    tileSize.x -= 0.05f;
                    tileSize.z -= 0.05f;

                    newTile.transform.localScale = tileSize;
                    newTile.SetActive(snappingPosition);
                    allTiles.Add(newTile);

                }
            }
        }
    }
    void DeleteCurrentTiles()
    {
        foreach (GameObject tile in allTiles)
        {
            Destroy(tile);
        }
        allTiles = new List<GameObject>();
    }
    public void ToggleGrid(bool show)
    {
        foreach (GameObject tile in allTiles)
        {
            tile.SetActive(show);
        }
    }
    public void ChangeTileDivision(int changeAmount)
    {
        divisionAmount += changeAmount;
        divisionAmount = Mathf.Clamp(divisionAmount, 1, 10);
        CalculateTilePositions(GameObject.FindGameObjectsWithTag("Ground"));
    }
}
