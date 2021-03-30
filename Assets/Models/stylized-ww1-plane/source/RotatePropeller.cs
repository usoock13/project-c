using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    public GameObject pCube1;
    public GameObject pCube4;
    public GameObject polySurface171;

    void Update()
    {
        pCube1.transform.Rotate(new Vector3(0, 0, 500*Time.deltaTime));
        pCube4.transform.Rotate(new Vector3(0, 0, 500*Time.deltaTime));
        polySurface171.transform.Rotate(new Vector3(0, 0, 500*Time.deltaTime));
    }
}
