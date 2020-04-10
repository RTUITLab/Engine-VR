using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnline : MonoBehaviour
{
    [SerializeField] private GameObject Body;
    [SerializeField] private GameObject[] Useless;
    void Update()
    {
        
    }
    public void isLocal()
    {
        Body.SetActive(false);
    }
    public void Disable()
    {
        for (int i = 0; i < Useless.Length; ++i)
        {
            Useless[i].SetActive(false);
        }
    }
}
