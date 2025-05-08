using UnityEngine;

namespace ShopSim.Scripts.Sellers
{
    public class QueueFirstTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider coll)
        {
            if (coll.TryGetComponent(out Seller seller))
            {
                seller.HandleOnSeller();
            }
        }
    }
}
