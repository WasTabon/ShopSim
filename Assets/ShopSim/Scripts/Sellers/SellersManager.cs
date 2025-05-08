using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShopSim.Scripts.Sellers
{
    public class SellersManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _movePoint1;
        [SerializeField] private Transform _movePoint2;
        [SerializeField] private Transform _movePoint3;
        [SerializeField] private Transform _movePoint4;

        [SerializeField] private Transform[] queuePoints = new Transform[5];
        [SerializeField] private Transform[] wayBack;

        [SerializeField] private Sprite[] _itemIcons;
        
        private Transform _currentQueuePoint;
        
        [FormerlySerializedAs("_seller")] [SerializeField] private GameObject _sellerPrefab;

        private Queue<Seller> _sellersQueue;

        private void Start()
        {
            _sellersQueue = new Queue<Seller>();
            
            InvokeRepeating("SpawnSeller", 0f, 15f);
        }
        
        private void MoveSellerQueue()
        {
            Seller seller = _sellersQueue.Dequeue();
            //seller.gameObject.SetActive(false);
            
            Seller[] array = _sellersQueue.ToArray();
            
            for (int i = 0; i < array.Length; i++)
            {
                array[i].SetMoveToTarget(queuePoints[i]);
            }
        }

        private void SpawnSeller()
        {
            if (_sellersQueue.Count < 5)
            {
                Item item = CreateItem();
                GameObject seller = CreateSeller(item);
            }
        }

        private GameObject CreateSeller(Item item)
        {
            GameObject spawned = Instantiate(_sellerPrefab, _spawnPoint.position, Quaternion.identity);
            Seller spawnedSeller = spawned.GetComponent<Seller>();
            
            spawnedSeller.Initialize(_movePoint1, _movePoint2,_movePoint3, _movePoint4, queuePoints[_sellersQueue.Count], item, wayBack);
            
            _sellersQueue.Enqueue(spawnedSeller);
            spawnedSeller.OnItemComplete += MoveSellerQueue;
            return spawned;
        }

        private Item CreateItem()
        {
            int randomNumber = Random.Range(0, _itemIcons.Length);
            Sprite randomIcon = _itemIcons[randomNumber];
            ItemRarity randomRarity = GetRandomRarity();
            int randomPrice = GetRandomPrice();
            int randomFake = Random.Range(1, 100);
            bool isFake = randomFake <= 25;
            int randomDirty = Random.Range(1, 100);
            bool isDirty = randomDirty <= 25;
            
            return new Item(randomIcon, randomRarity, randomPrice, isFake, isDirty);
        }
        
        public static int GetRandomPrice()
        {
            (int min, int max)[] ranges = {
                (10, 60),    // 50%
                (60, 160),   // 30%
                (160, 350),  // 15%
                (350, 500)   // 5%
            };
            
            float[] weights = { 50f, 30f, 15f, 5f };

            float total = 0f;
            foreach (var w in weights)
                total += w;

            float randomPoint = Random.Range(0f, total);

            float cumulative = 0f;
            for (int i = 0; i < weights.Length; i++)
            {
                cumulative += weights[i];
                if (randomPoint < cumulative)
                {
                    return Random.Range(ranges[i].min, ranges[i].max + 1); 
                }
            }

            return Random.Range(10, 500);
        }
        public static ItemRarity GetRandomRarity()
        {
            ItemRarity[] rarities = (ItemRarity[])System.Enum.GetValues(typeof(ItemRarity));
            float[] chances = { 60f, 25f, 10f, 5f };

            float total = 0f;
            foreach (var chance in chances)
                total += chance;

            float randomPoint = Random.Range(0f, total);

            float cumulative = 0f;
            for (int i = 0; i < chances.Length; i++)
            {
                cumulative += chances[i];
                if (randomPoint < cumulative)
                    return rarities[i];
            }
            
            return rarities[0];
        }
    }
}
