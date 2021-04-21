using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private float alertShowTime = 5f;
    [SerializeField] private GameObject successAlert;

    private NicknameChoose nickname;

    private void Start()
    {
        nickname = FindObjectOfType<NicknameChoose>();
    }

    public void SetInputText(string text)
    {
        inputField.text = text;
    }

    public void AddChar(char input)
    {
        inputField.text += input;

        nickname.SaveNickname(false);
    }

    public void RemoveChar()
    {
        inputField.text = inputField.text.Remove(inputField.text.Length-1);

        nickname.SaveNickname(false);
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
