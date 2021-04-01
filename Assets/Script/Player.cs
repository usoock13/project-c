using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int currenCombo = 0;
    bool comboable = false;

    IEnumerator attackCoroutine;

    Rigidbody playerRigidbody;
    Animator playerAnimator;
    public GameObject playerAvatar;

    float moveSpeed = 7f;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = playerAvatar.GetComponent<Animator>();
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
        if(
            playerState == PlayerState.Attack
        ) { return; }
        attackCoroutine = AttackCoroutine(targetPosition);
        StartCoroutine(attackCoroutine);
    }
    IEnumerator AttackCoroutine(Vector3 targetPosition) {
        Vector3 target = targetPosition;
        target.y = playerAvatar.transform.position.y;
        playerAvatar.transform.LookAt(target);
        playerState = PlayerState.Attack;
        playerAnimator.SetBool("Attack", true);
        currenCombo = currenCombo + 1;

        switch(currenCombo) {
            case 1:
                playerAnimator.SetInteger("Combo", currenCombo);
                playerAnimator.speed = 2.2f;
                yield return new WaitForSeconds(.22f);

                playerAnimator.speed = .5f;
                playerState = PlayerState.AfterAttack;
                yield return new WaitForSeconds(.5f);

                if(currenCombo == 1) {
                    playerAnimator.speed = 1f;
                    playerState = PlayerState.Idle;
                    playerAnimator.SetBool("Attack", false);
                    currenCombo = 0;
                }
                break;
            case 2:
                playerAnimator.SetInteger("Combo", currenCombo);
                playerAnimator.speed = 1.7f;
                yield return new WaitForSeconds(.22f);

                playerAnimator.speed = .5f;
                playerState = PlayerState.AfterAttack;
                yield return new WaitForSeconds(.5f);

                if(currenCombo == 2) {
                    playerAnimator.speed = 1f;
                    playerState = PlayerState.Idle;
                    playerAnimator.SetBool("Attack", false);
                    currenCombo = 0;
                }
                break;
            case 3:
                playerAnimator.SetInteger("Combo", currenCombo);
                playerAnimator.speed = 2.2f;
                yield return new WaitForSeconds(.30f);

                playerAnimator.speed = .5f;
                yield return new WaitForSeconds(.5f);

                if(currenCombo == 3) {
                    playerAnimator.speed = 1f;
                    playerState = PlayerState.Idle;
                    playerAnimator.SetBool("Attack", false);
                    currenCombo = 0;
                }
                break;
            default : 
                break;
        }
    }
    
    void AnimationController() {
        
    }
}
