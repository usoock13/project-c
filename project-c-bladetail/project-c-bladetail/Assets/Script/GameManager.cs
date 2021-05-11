using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManagerInstant;
    public static GameManager instant {
        get {
            if(gameManagerInstant == null) {
                gameManagerInstant = FindObjectOfType<GameManager>();
            }
            return gameManagerInstant;
        }
    }

    public float gravityAcceleration = 9.8f;

    void Start() {
        
    }
}