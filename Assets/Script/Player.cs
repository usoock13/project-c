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

    public Transform cutPlane;
    public LayerMask layerMask;
    public Material crossMaterial;

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
            playerState == PlayerState.SmashAttack
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
            playerState == PlayerState.Attack ||
            playerState == PlayerState.SmashAttack
        ) { return; }
        StartCoroutine(AttackCoroutine(targetPosition));
    }
    IEnumerator AttackCoroutine(Vector3 targetPosition) {
        Vector3 target = targetPosition;
        target.y = playerAvatar.transform.position.y;
        playerAvatar.transform.LookAt(target);
        playerState = PlayerState.Attack;
        playerAnimator.SetBool("Attack", true);
        Slice();

        playerAnimator.speed = 3.5f;
        yield return new WaitForSeconds(.4f);

        //playerAnimator.speed = 1f;
        // yield return new WaitForSeconds(.25f);

        // playerAnimator.speed = .25f;
        // yield return new WaitForSeconds(.3f);

        playerAnimator.speed = .5f;
        yield return new WaitForSeconds(.05f);

        playerAnimator.speed = 1f;
        playerState = PlayerState.Idle;
        playerAnimator.SetBool("Attack", false);
    }
    public void SmashAttack(Vector3 targetPosition) {
        if(
            playerState == PlayerState.Attack ||
            playerState == PlayerState.SmashAttack
        ) { return; }
        StartCoroutine(SmashAttackCoroutine(targetPosition));
    }
    IEnumerator SmashAttackCoroutine(Vector3 targetPosition) {
        Vector3 target = targetPosition;
        target.y = playerAvatar.transform.position.y;
        playerAvatar.transform.LookAt(target);
        playerState = PlayerState.SmashAttack;
        playerAnimator.SetBool("SmashAttack", true);

        playerAnimator.speed = 3.5f;
        yield return new WaitForSeconds(.4f);

        //playerAnimator.speed = 1f;
        // yield return new WaitForSeconds(.25f);

        // playerAnimator.speed = .25f;
        // yield return new WaitForSeconds(.3f);

        playerAnimator.speed = .3f;
        yield return new WaitForSeconds(.05f);
        playerAnimator.speed = 1f;

        playerState = PlayerState.Idle;
        playerAnimator.SetBool("SmashAttack", false);
    }

    public void Slice()
    {
        // Debug.Log("Slice()");
        Collider[] hits = Physics.OverlapBox(cutPlane.position, new Vector3(5, 0.1f, 5), cutPlane.rotation, layerMask);

        if (hits.Length <= 0) {
            // Debug.Log("Has no slice obj");
            return;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            GameObject target = hits[i].gameObject;
            SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
            
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                bottom.layer = 8;
                top.layer = 8;
                Destroy(hits[i].gameObject);
            }
        }
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, go.transform.position, 20);
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(cutPlane.position, cutPlane.up, crossSectionMaterial);
    }
}
