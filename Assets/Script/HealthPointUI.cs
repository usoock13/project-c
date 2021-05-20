using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointUI : MonoBehaviour
{
    public float hp = 0;
    public Material material;

    void Start() {
        material.shader = Shader.Find("Shader Graphs/UI Shader Graph");
    }

    void Update() {
        material.SetFloat("_HeightPercent", hp);
    }
}
