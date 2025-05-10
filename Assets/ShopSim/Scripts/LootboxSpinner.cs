using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class LootboxSpinner : MonoBehaviour
{
    [System.Serializable]
    public class CaseItem
    {
        public RectTransform rect;
        public string itemName; // Название предмета, можно расширить
    }

    public List<CaseItem> caseItems; // 5 предметов
    public float spinSpeed = 300f; // Пикселей в секунду
    public float minSpinDistance = 2000f;
    public float maxSpinDistance = 4000f;

    public RectTransform panelCenter; // Центральная точка панели
    public float spacing = 200f; // Расстояние между предметами
    public float wrapOffset = 600f; // Граница, за которую "телепортируем"

    private float totalDistanceToSpin;
    private float distanceSpun;
    private bool isSpinning;

    private CaseItem selectedItem;

    public void StartSpin()
    {
        if (isSpinning) return;

        totalDistanceToSpin = Random.Range(minSpinDistance, maxSpinDistance);
        distanceSpun = 0f;
        isSpinning = true;
    }

    void Update()
    {
        if (!isSpinning) return;

        float move = spinSpeed * Time.deltaTime;
        distanceSpun += move;

        foreach (var item in caseItems)
        {
            item.rect.anchoredPosition -= new Vector2(move, 0);

            // Проверка границ и "перемотка"
            if (item.rect.anchoredPosition.x < -wrapOffset)
            {
                float rightMostX = caseItems.Max(i => i.rect.anchoredPosition.x);
                item.rect.anchoredPosition = new Vector2(rightMostX + spacing, item.rect.anchoredPosition.y);
            }
        }

        if (distanceSpun >= totalDistanceToSpin)
        {
            isSpinning = false;
            SnapToNearestCenterItem();
        }
    }

    private void SnapToNearestCenterItem()
    {
        CaseItem closest = null;
        float closestDist = float.MaxValue;

        foreach (var item in caseItems)
        {
            float dist = Mathf.Abs(item.rect.position.x - panelCenter.position.x);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = item;
            }
        }

        selectedItem = closest;
        Debug.Log("Выпал предмет: " + selectedItem.itemName);
        // Можешь вызвать тут анимацию, эффекты и т.д.
    }
}
