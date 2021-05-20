using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player playerScript;

    void Awake() {
        playerScript = GetComponent<Player>();
    }
    void FixedUpdate() {
        MoveInput();
        AttackInput();
        SmashAttackInput();
    }
    void MoveInput() {
        float xx = Input.GetAxisRaw("Horizontal");
        float yy = Input.GetAxisRaw("Vertical");

        if(xx!=0 || yy!=0) {
            Vector3 direction = new Vector3(xx, 0, yy).normalized;
            playerScript.Move(direction);
        } else {
            
        }
    }
    void AttackInput() {
        if(Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                playerScript.Attack(hit.point);
            }
        }
    }
    void SmashAttackInput() {
        if(Input.GetMouseButton(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                playerScript.SmashAttack(hit.point);
            }
        }
    }
}