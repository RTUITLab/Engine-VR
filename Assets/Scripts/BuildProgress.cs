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
        progressOutput.text = $"СОБРАНО {placedPartsCount} ДЕТАЛЕЙ ИЗ {partsCount}";

        progressVisual.value = ((float)placedPartsCount / partsCount);

        currentStageOutput.text = $"ЭТАП: {currentStage} — {currentPlacedPartsCount} ДЕТАЛЕЙ ИЗ {currentPartsCount}";
    }
}
