using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour
{
    DestroyableObject parent;
    public float power = 100f;
    void Start() {
        parent = transform.parent.GetComponent<DestroyableObject>();
        parent.breakDelegate += BreakObject;
    }

    void BreakObject(Vector3 attackerPosition) {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce((transform.position - attackerPosition).normalized * power);
    }
}
