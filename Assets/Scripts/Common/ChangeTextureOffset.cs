using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureOffset : MonoBehaviour
{
    public float offsetX = 1;
    Material material;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        var offset = material.mainTextureOffset;
        offset.x += offsetX * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
