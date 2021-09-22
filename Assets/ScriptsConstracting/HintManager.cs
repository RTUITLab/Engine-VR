using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] private Text hintOutput;

    public void DisplayHint(string hint)
    {
        hintOutput.text = hint;
    }

    public void StopDisplayingHint()
    {
        // hintOutput.text = "";
    }
}
