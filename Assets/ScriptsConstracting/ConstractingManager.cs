using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstractingManager : MonoBehaviour
{
    [SerializeField] List<FixedPart> fixedParts;
    int currentFixedPartIndex;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartFunc", 1);
    }

    void StartFunc()
    {
        currentFixedPartIndex = 0;
        NextFixedPart();
    }

    public void NextFixedPart()
    {
        if (fixedParts.Count > currentFixedPartIndex)
        {
            fixedParts[currentFixedPartIndex].currentStage = FixedPart.Stage.Highlighted;
            currentFixedPartIndex++;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
