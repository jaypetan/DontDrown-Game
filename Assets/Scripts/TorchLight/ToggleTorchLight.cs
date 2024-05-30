using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTorchLight : MonoBehaviour
{
    public PlayerSanity torchSanity;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (torchSanity.curSanity <= 20)
        {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
    }
}
