using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildProgress : MonoBehaviour
{
    [SerializeField] private Text progressOutput;
    [SerializeField] private Slider progressVisual;
    [SerializeField] private Text currentStageOutput;

    public void ChangeData(int partsCount, // Общее кол-во необходимых деталей
        int placedPartsCount, // Общее кол-во поставленных деталей
        int currentStage, // Номер текущей стадии (этапа)
        int currentPartsCount, // Кол-во необходимых деталей на текущем этапе
        int currentPlacedPartsCount) // Кол-во поставленных деталей на текущем этапе
    {
        progressOutput.text = $"ВСЕГО: {placedPartsCount} ДЕТАЛ{FormatEnding(placedPartsCount)} ИЗ {partsCount}";

        progressVisual.value = ((float)placedPartsCount / partsCount);

        currentStageOutput.text = $"ЭТАП: {currentStage} — {currentPlacedPartsCount} ДЕТАЛ{FormatEnding(currentPlacedPartsCount)} ИЗ {currentPartsCount}";
    }

    private string FormatEnding(int number)
    {
        // ДЕТАЛ + ЕЙ/Ь/И
        switch (number)
        {
            case 0:
            case int n when (n >= 5) && (n <= 20):
                return "ЕЙ";
            case 1:
                return "Ь";
            case 2:
            case 3:
            case 4:
                return "И";
            default:
                // Оставляем только последнюю цифру
                return (FormatEnding(number % 10));
        }
    }
}
