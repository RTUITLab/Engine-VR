using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrentHint : MonoBehaviour
{
    [SerializeField] private Text output;

    private void Start()
    {
        StopDisplaying();
    }

    public void DispayHint(string text)
    {
        output.text = text;
    }

    public void StopDisplaying()
    {
        output.text = "";
    }
}