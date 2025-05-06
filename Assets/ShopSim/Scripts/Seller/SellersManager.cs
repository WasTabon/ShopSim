using System.Collections.Generic;
using UnityEngine;

namespace ShopSim.Scripts.Seller
{
    public class SellersManager : MonoBehaviour
    {
        [SerializeField] private Transform _movePoint1;
        [SerializeField] private Transform _movePoint2;
        
        [SerializeField] private Transform _queuePoint1;
        [SerializeField] private Transform _queuePoint2;
        [SerializeField] private Transform _queuePoint3;
        [SerializeField] private Transform _queuePoint4;
        [SerializeField] private Transform _queuePoint5;
        
        private Transform _currentQueuePoint;
        
        [SerializeField] private Seller _seller;

        private Queue<Seller> _sellersQueue;

        private void Start()
        {
            InvokeRepeating("SpawnSeller", 0f, 10f);
        }

        private void SpawnSeller()
        {
            
        }
    }
}
