using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject GoToEndScene;
    public GameObject CheckPoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SharkMonsterHealth sharkState = GetComponent<SharkMonsterHealth>();
        if(sharkState.monsterDied)
        {
            GoToEndScene.SetActive(true);
            CheckPoint.SetActive(false);
        }
    }
}
