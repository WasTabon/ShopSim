using System.Collections.Generic;
using UnityEngine;

namespace ShopSim.Scripts.Seller
{
    public class SellersManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _movePoint1;
        [SerializeField] private Transform _movePoint2;
        [SerializeField] private Transform _movePoint3;
        [SerializeField] private Transform _movePoint4;

        [SerializeField] private Transform[] queuePoints = new Transform[5];
        
        private Transform _currentQueuePoint;
        
        [SerializeField] private GameObject _seller;

        private Queue<Seller> _sellersQueue;

        private void Start()
        {
            _sellersQueue = new Queue<Seller>();
            
            InvokeRepeating("SpawnSeller", 0f, 1f);
        }

        private void SpawnSeller()
        {
            GameObject spawned = Instantiate(_seller, _spawnPoint.position, Quaternion.identity);
            Seller spawnedSeller = spawned.GetComponent<Seller>();
            
            spawnedSeller.Initialize(_movePoint1, _movePoint2,_movePoint3, _movePoint4, queuePoints[_sellersQueue.Count]);
            
            _sellersQueue.Enqueue(spawnedSeller);
        }
    }
}
