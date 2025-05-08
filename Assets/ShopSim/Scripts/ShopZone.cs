using ShopSim.Scripts.Sellers;
using UnityEngine;

public class ShopZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out Seller seller))
        {
            seller.isInsideShop = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out Seller seller))
        {
            seller.isInsideShop = false;
        }
    }
}
