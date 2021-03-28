﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState {
    Idle,
    Run,
    Slide,
    Jump,
    Attack
}
public class Player : MonoBehaviour
{
    PlayerState playerState = PlayerState.Idle;

    Rigidbody playerRigidbody;
    Animator playerAnimator;
    public GameObject playerAvatar;

    float moveSpeed = 5f;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = playerAvatar.GetComponent<Animator>();
    }
    void Update()
    {
        float xx = Input.GetAxisRaw("Horizontal");
        float yy = Input.GetAxisRaw("Vertical");

        if(xx==0 && yy==0) {
            playerAnimator.SetBool("Running", false);
        }
    }

    public void Idle(){

    }

    public void Move(Vector3 direction) {
        // Position
        Vector3 addPosition = transform.TransformDirection(direction);
        playerRigidbody.MovePosition(transform.position + (addPosition * Time.deltaTime * moveSpeed));
        
        // Rotate
        Vector3 rotateTarget = transform.TransformDirection(direction);
        playerAvatar.transform.rotation = Quaternion.Slerp(playerAvatar.transform.rotation, Quaternion.LookRotation(rotateTarget), Time.deltaTime * 10f);

        // Animation
        playerAnimator.SetBool("Running", true);
    }
    
    void AnimationController() {
        
    }
}
