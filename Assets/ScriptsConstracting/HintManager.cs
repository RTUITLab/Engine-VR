using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] private Text hintOutput;

    private void Start()
    {
        StopDisplaying();
    }

    public void DisplayHint(string hint)
    {
        hintOutput.text = hint;
    }

    public void StopDisplaying()
    {
        hintOutput.text = "";
    }
}
