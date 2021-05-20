using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Player playerScript;

    void Start() {
        playerScript = transform.parent.gameObject.GetComponent<Player>();
    }
    public void HitMelee() {
        playerScript.HitMelee();
    }
    public void EndMelee() {
        playerScript.EndMelee();
    }
}
