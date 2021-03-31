using System.Collections;
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
            playerState == PlayerState.Attack
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
        StartCoroutine(AttackCoroutine(targetPosition));
    }
    IEnumerator AttackCoroutine(Vector3 targetPosition) {
        Vector3 target = targetPosition;
        target.y = playerAvatar.transform.position.y;
        playerAvatar.transform.LookAt(target);
        playerState = PlayerState.Attack;
        playerAnimator.SetBool("Attack", true);

        playerAnimator.speed = 3.5f;
        yield return new WaitForSeconds(.4f);

        //playerAnimator.speed = 1f;
        // yield return new WaitForSeconds(.25f);

        // playerAnimator.speed = .25f;
        // yield return new WaitForSeconds(.3f);

        playerAnimator.speed = 1f;
        yield return new WaitForSeconds(.05f);

        playerState = PlayerState.Idle;
        playerAnimator.SetBool("Attack", false);
    }
    
    void AnimationController() {
        
    }
}
