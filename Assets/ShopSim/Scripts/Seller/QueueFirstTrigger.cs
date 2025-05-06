using ShopSim.Scripts.Seller;
using UnityEngine;

public class QueueFirstTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.TryGetComponent(out Seller seller))
        {
            seller.ShowDialogue();
        }
    }
}
