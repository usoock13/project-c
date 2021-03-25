using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player playerScript;
    void Awake() {
        playerScript = GetComponent<Player>();
    }
    void Update()
    {
        MoveInput();
    }
    void MoveInput(){
        float xx = Input.GetAxisRaw("Horizontal");
        float yy = Input.GetAxisRaw("Vertical");

        if(xx!=0 || yy!=0) {
            Vector3 direction = new Vector3(xx, 0, yy).normalized;
            playerScript.Move(direction);
        }
    }
}