using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public delegate void BreakDelegate(Vector3 attackerPosition);
    public BreakDelegate breakDelegate;
    public void OnDamage(Vector3 attackerPosition) {
        OnBreak(attackerPosition);
    }
    public void OnBreak(Vector3 attackerPosition) {
        breakDelegate(attackerPosition);
        GetComponent<BoxCollider>().enabled = false;
    }
}
