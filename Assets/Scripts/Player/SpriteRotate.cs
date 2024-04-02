using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool facingRight = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}