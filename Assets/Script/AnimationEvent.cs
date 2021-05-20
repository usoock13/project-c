using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    Player player;
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void Event() {
        // player.AnimationEvent();
    }
}
