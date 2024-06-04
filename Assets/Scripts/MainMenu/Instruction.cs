using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    public GameObject instruction1;
    public GameObject instruction2;

    void Start()
    {
        instruction1.SetActive(false);
        instruction2.SetActive(false);
    }

    public void ShowInstructions()
    {
        instruction1.SetActive(true);
    }

    public void GoToInstruction2()
    {
        instruction2.SetActive(true);
        instruction1.SetActive(false);
    }

    public void GoOutOfInstuction2()
    {
        instruction2.SetActive(false);
    }
}
