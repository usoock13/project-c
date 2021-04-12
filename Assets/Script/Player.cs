using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum PlayerState {
    Idle,
    Run,
    Slide,
    Jump,
    Attack,
    AfterAttack
}

public class Player : MonoBehaviour
{
    PlayerState playerState = PlayerState.Idle;
    int maxComboNumber = 3;
    bool comboable = false;

    IEnumerator attackCoroutine;

    Rigidbody playerRigidbody;
    Animator playerAnimator;
    public GameObject playerAvatar;
    Text countText;

    float moveSpeed = 7f;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = playerAvatar.GetComponent<Animator>();
        countText = GetComponentInChildren<Text>();
    }
    void Update()
    {
        float xx = Input.GetAxisRaw("Horizontal");
        float yy = Input.GetAxisRaw("Vertical");

        if(xx==0 && yy==0) {
            playerAnimator.SetBool("Run", false);
        }
    }

    public void Move(Vector3 direction) {
        if(
            playerState == PlayerState.Attack || 
            playerState == PlayerState.AfterAttack
        ) { return; }
        // Position
        Vector3 addPosition = transform.TransformDirection(direction);
        // playerRigidbody.MovePosition(transform.position + (addPosition * Time.deltaTime * moveSpeed));
        transform.Translate(direction * Time.deltaTime * moveSpeed);
        
        // Rotate
        Vector3 rotateTarget = transform.TransformDirection(direction);
        playerAvatar.transform.rotation = Quaternion.Slerp(playerAvatar.transform.rotation, Quaternion.LookRotation(rotateTarget), Time.deltaTime * 10f);

        // Animation
        playerAnimator.SetBool("Run", true);
        playerState = PlayerState.Run;
    }

    public void Attack(Vector3 targetPosition) {
        if(playerState == PlayerState.Attack) return;
        attackCoroutine = AttackCoroutine(targetPosition);
        StartCoroutine(attackCoroutine);
    }
    IEnumerator AttackCoroutine(Vector3 targetPosition) {
        Vector3 target = targetPosition;
        target.y = playerAvatar.transform.position.y;
        playerAvatar.transform.LookAt(target);
        playerState = PlayerState.Attack;
        playerAnimator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        playerState = PlayerState.Idle;
        playerAnimator.SetBool("Attack", false);
    }

    int eventCount = 0;
    public void AnimationEvent() {
        print(++eventCount);
        countText.text = "count : " + eventCount;
    }
    
    void AnimationController() {
        
    }
}
