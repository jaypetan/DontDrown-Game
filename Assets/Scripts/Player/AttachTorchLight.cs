using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTorchLight : MonoBehaviour
{

    public Transform attachedObject; // Drag the object you want to attach in the Inspector.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  Attach the object to the sprite
        if (attachedObject != null)
        {
            attachedObject.parent = transform;
            attachedObject.localPosition = new Vector3(0.54f, -0.156f, 0f); // Adjust the position as needed.
        }
    }
}
