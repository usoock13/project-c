using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EzySlice;

enum PlayerState {
    Idle,
    Run,
    Slide,
    Jump,
    Attack,
    SmashAttack
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

    public Transform attackRange;

    float moveSpeed = 7f;
    Vector3 range;

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
            playerState == PlayerState.SmashAttack
        ) { return; }
        // Position
        Vector3 addPosition = transform.TransformDirection(direction);
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
            playerState == PlayerState.Attack ||
            playerState == PlayerState.SmashAttack
        ) { return; }

        playerAvatar.transform.LookAt(new Vector3(targetPosition.x, 0, targetPosition.z));

        playerState = PlayerState.Attack;
        playerAnimator.SetBool("Attack", true);
    }

    public void EndMelee() {
        playerState = PlayerState.Idle;
        playerAnimator.SetBool("Attack", false);
    }
    public void HitMelee() {
        range = playerAvatar.transform.position + playerAvatar.transform.forward;
        Vector3 center = playerAvatar.transform.position;
        Collider[] colliders = Physics.OverlapBox(range, new Vector3(.2f, 1f, .3f), playerAvatar.transform.rotation, LayerMask.GetMask("Damageable"));

        for(int i=0; i<colliders.Length; i++) {
            colliders[i].GetComponent<DestroyableObject>().OnDamage(playerAvatar.transform.position);
        }
    }
}
