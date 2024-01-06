using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASD : MonoBehaviour
{
    public float controlSpeed = 5;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3( h, v, 0);
        transform.position += move * controlSpeed * Time.deltaTime;
        Vector3 direction = move.normalized;

        if (move != Vector3.zero)
        {
            transform.up = direction;
        }

    }
}
