using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    GameManager gameManager;
    private float descentSpeed = 0;
    private bool isGrounded = false;
    public bool useGravity = true;

    void Start() {
        gameManager = GameManager.instant;
    }
    void Update() {
        Accelerate();
        print(isGrounded);
    }

    private bool CheckGrounded(Collision collisionInfo) {
        print(collisionInfo.collider.transform.forward.normalized.y);
        if(Mathf.Abs(collisionInfo.collider.transform.up.normalized.y) > .5f){
            return true;
        } else {
            return false;
        }
    }
    private void Accelerate() {
        if(useGravity && isGrounded) {
            descentSpeed = 0;
        } else {
            transform.position = transform.position + (new Vector3(0, descentSpeed, 0) * Time.deltaTime);
            descentSpeed -= gameManager.gravityAcceleration * Time.deltaTime * 1f;
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.collider.tag == "Walkable") isGrounded = CheckGrounded(collisionInfo);
    }
    void OnCollisionExit(Collision collisionInfo) {
        if(collisionInfo.collider.tag == "Walkable") isGrounded = !CheckGrounded(collisionInfo);
    }
}
