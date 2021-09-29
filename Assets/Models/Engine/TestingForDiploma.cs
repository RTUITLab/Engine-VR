using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingForDiploma : MonoBehaviour
{
    // Start is called before the first frame update
    public int Count = 0;
    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            Count++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
