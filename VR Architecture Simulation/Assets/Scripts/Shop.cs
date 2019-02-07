using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Placer placingSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnObject(GameObject objectToPlace)
    {
        placingSystem.trackingObj = Instantiate(objectToPlace, Vector3.zero, Quaternion.identity);
    }
}
