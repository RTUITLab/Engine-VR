using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameChoose : MonoBehaviour
{
    [Tooltip("Позволяет сбрасывать никнейм каждый раз при входе в главное меню.\n" +
        "Полезно на демонстрационных стендах, чтобы не сохранять чужие ники.")]
    [SerializeField] private bool resetNicknameAfterRestart = true;
    private string nickname;

    [SerializeField] private Keyboard keyboard;

    private void Start()
    {
        string savedNickname = PlayerPrefs.GetString("Nickname");
        LoadNickname();
    }

    private void LoadNickname()
    {
        if (resetNicknameAfterRestart)
        { // См. описание флага
            nickname = RandomNickname();
        }
        else if (PlayerPrefs.HasKey("Nickname"))
        { // Загружаем сохраненный никнейм
            nickname = PlayerPrefs.GetString("Nickname");
        }
        else // Ни один никнейм пока не был сохранен
        {
            nickname = RandomNickname();
        }
        keyboard.SetInputText(nickname.ToUpper());

        SaveNickname(false);
    }

    /// <summary>
    // Вызывается при нажатии кнопки с предложенным именем, например: «инженер» или «помощник»
    /// </summary>
    public void UseNickname(string name) 
    {
        nickname = name;
        keyboard.SetInputText(nickname.ToUpper());

        SaveNickname(false);
    }

    private string RandomNickname()
    {
        string[] nicknameList = { "Инженер", "Конструктор", "Механик", "Помощник" };
        return nicknameList[Random.Range(0, nicknameList.Length)];
    }

    public void SaveNickname(bool showSuccess)
    {
        nickname = keyboard.inputField.text;
        PlayerPrefs.SetString("Nickname", nickname);

        if (showSuccess)
            keyboard.StartCoroutine(keyboard.ShowSuccess());
    }
}
