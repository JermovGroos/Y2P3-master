using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [Range(1, 10)]
    public int divisionAmount;
    public GameObject tile;
    public Vector2 tileSize;
    public Vector2 gridTileSize;
    public List<GameObject> allTiles = new List<GameObject>();
    // Start is called before the first frame update
    private void Start()
    {
        CalculateTilePositions(GameObject.FindGameObjectsWithTag("Ground"));
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CalculateTilePositions(GameObject.FindGameObjectsWithTag("Ground"));
        }
        if (Input.GetButtonDown("Sprint"))
        {
            ToggleGrid(true);
        }
        else
        {
            if (Input.GetButtonUp("Sprint"))
            {
                ToggleGrid(false);
            }
        }
    }

    // Update is called once per frame
    void CalculateTilePositions(GameObject[] groundTiles)
    {
        DeleteCurrentTiles();
        foreach(GameObject groundTile in groundTiles)
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
                    newTile.SetActive(false);
                    allTiles.Add(newTile);

                }
            }
        }
    }
    void DeleteCurrentTiles()
    {
        foreach(GameObject tile in allTiles)
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
}
