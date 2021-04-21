﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKeyboardButton : VRButton
{
    [Header("Параметры клавиши")]
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private Keys key;

    private void Start()
    {
        if (keyboard == null)
        {
            Debug.LogError($"Клавиатура не найдена! {gameObject.name}");
        }

        EventClick.AddListener(KeyClicked);
    }

    public void KeyClicked()
    {
        switch (key)
        {
            case Keys.Clear: // Стерка всего
                keyboard.ClearAll();
                break;
            case Keys.Backspace: // Стерка одной буквы
                keyboard.RemoveChar();
                break;
            case Keys.Space: // Пробел
                keyboard.AddChar(' ');
                break; 
            case Keys.minus: // Минус
                keyboard.AddChar('-');
                break;
            case Keys.equals: // Равно
                keyboard.AddChar('=');
                break;
            case Keys.dot: // Точка
                keyboard.AddChar('.');
                break; 
            default:
                if (key.ToString().Contains("num"))
                { // Число
                    keyboard.AddChar(key.ToString()[key.ToString().Length - 1]);
                    // Берем последнюю букву: num5 -> 5
                } else 
                { // Буква
                    keyboard.AddChar(key.ToString()[0]);
                }       
                break;
        }
    }
}

public enum Keys
{
    Б, В, Г, Д, Ж, З, Й, К, Л, М, Н, П, Р, С, Т, Ф, Х, Ч, Ц, Ш, Щ,
    А, Е, Ё, И, О, У, Ы, Э, Ю, Я, Ь, Ъ,
    Clear, Backspace, Space, minus, equals, dot,
    num1, num2, num3, num4, num5, num6, num7, num8, num9, num0
}
