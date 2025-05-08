using ShopSim.Scripts.Sellers;
using UnityEngine;

public class RemoveSellers : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out Seller seller))
        {
            seller.gameObject.SetActive(false);
        }
    }
}
