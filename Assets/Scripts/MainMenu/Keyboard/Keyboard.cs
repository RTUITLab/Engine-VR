using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private float alertShowTime = 5f;
    [SerializeField] private GameObject successAlert;

    public void SetInputText(string text)
    {
        inputField.text = text;
    }

    public void AddChar(char input)
    {
        inputField.text += input;
    }

    public void RemoveChar()
    {
        inputField.text = inputField.text.Remove(inputField.text.Length-1);
    }

    public void ClearAll()
    {
        inputField.text = "";
    }

    public IEnumerator ShowSuccess()
    {
        successAlert.SetActive(true);

        yield return new WaitForSecondsRealtime(alertShowTime);

        successAlert.SetActive(false);
    }
}
