using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        Placer.placer.SetTrackingObject(gameObject);
    }
}
