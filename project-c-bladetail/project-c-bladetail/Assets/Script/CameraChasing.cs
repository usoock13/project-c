using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChasing : MonoBehaviour
{
    public GameObject player;
    public float chaseSpeed = 7.0f;
    Vector3 nextPosition;
    void FixedUpdate() {
        nextPosition = player.transform.position + new Vector3(-5.3f, 9f, -5.3f);
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * chaseSpeed);
        // transform.position = nextPosition;
    }
}
