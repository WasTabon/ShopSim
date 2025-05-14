using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ShopSim.Scripts;

public class LootboxSpinner : MonoBehaviour
{
    [System.Serializable]
    public class CaseItem
    {
        public string itemName;
        public RectTransform prefab;
    }

    private class SpawnedItem
    {
        public RectTransform rect;
        public string itemName;
        public bool hasPlayedSound = false;
        public Sprite sprite;
    }

    public GameObject button;

    public BuyController buyController;
    public List<CaseItem> caseItems; // шаблоны предметов (prefab + name)
    public RectTransform content; // Родитель для предметов
    public float spacing = 200f;
    public float spinSpeed = 300f;
    public float minSpinDistance = 2000f;
    public float maxSpinDistance = 4000f;
    public RectTransform panelCenter;
    public float wrapOffset = 600f;

    public GameObject _winParticles;
    
    public AudioClip[] tickSounds;
    public AudioClip winSound;
    public AudioSource audioSource;
    public float soundTriggerThreshold = 10f;   

    private List<SpawnedItem> spawnedItems = new List<SpawnedItem>();
    private float totalDistanceToSpin;
    private float distanceSpun;
    private bool isSpinning;

    private SpawnedItem selectedItem;

    void Start()
    {
        GenerateItems();
    }

    void GenerateItems()
    {
        spawnedItems.Clear();

        float startX = -content.rect.width / 2f + spacing / 2f;

        for (int i = 0; i < caseItems.Count; i++)
        {
            var caseItem = caseItems[i];
            RectTransform itemInstance = Instantiate(caseItem.prefab, content);
            itemInstance.anchoredPosition = new Vector2(startX + i * spacing, 0);

            var spawned = new SpawnedItem
            {
                rect = itemInstance,
                itemName = caseItem.itemName,
                sprite = caseItem.prefab.gameObject.GetComponent<SpriteRenderer>().sprite
            };

            spawnedItems.Add(spawned);
        }
    }

    public void StartSpin()
    {
        if (buyController.GetMoneyCount() - 300 >= 0)
        {
            if (isSpinning) return;
            
            button.SetActive(false);

            buyController.RemoveMoney(300);
            
            totalDistanceToSpin = Random.Range(minSpinDistance, maxSpinDistance);
            distanceSpun = 0f;
            isSpinning = true;
        }
    }

    void Update()
    {
        if (!isSpinning) return;

        float move = spinSpeed * Time.deltaTime;
        distanceSpun += move;

        foreach (var item in spawnedItems)
        {
            item.rect.anchoredPosition -= new Vector2(move, 0);

            float rightEdge = item.rect.anchoredPosition.x + (item.rect.rect.width / 2f);
            if (rightEdge < -wrapOffset)
            {
                float rightMostX = spawnedItems.Max(i => i.rect.anchoredPosition.x);
                item.rect.anchoredPosition = new Vector2(rightMostX + spacing, item.rect.anchoredPosition.y);
            }

            float centerDist = Mathf.Abs(item.rect.position.x - panelCenter.position.x);

            if (centerDist < soundTriggerThreshold && !item.hasPlayedSound)
            {
                item.hasPlayedSound = true;
                int randomSound = Random.Range(0, tickSounds.Length);
                audioSource.PlayOneShot(tickSounds[randomSound]);
            }
            else if (centerDist >= soundTriggerThreshold)
            {
                item.hasPlayedSound = false;
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
        SpawnedItem closest = null;
        float closestDist = float.MaxValue;

        foreach (var item in spawnedItems)
        {
            float dist = Mathf.Abs(item.rect.position.x - panelCenter.position.x);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = item;
            }
        }

        selectedItem = closest;
        audioSource.PlayOneShot(winSound);
        button.SetActive(true);
        buyController.AddItemToInventory(selectedItem.sprite);
        Debug.Log("Выпал предмет: " + selectedItem.itemName);
    }
}
