using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertSnapTest : MonoBehaviour
{
    public Vector3 selectedVertInfo;
    public Vector3 hitPoint;
    public GameObject hodler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Vector3 vert in hodler.GetComponent<MeshFilter>().mesh.vertices)
        {
            print(vert);
        }
    }
}
