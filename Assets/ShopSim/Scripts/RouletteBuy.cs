using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class RouletteBuy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private LootboxSpinner _lootboxSpinner;
    
    public void OnPurchaseComlete(Product product)
    {
        if (product.definition.id == "com.coidea.forestMatch.full")
        {
            Debug.Log("Complete");
            _lootboxSpinner.StartSpin();
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription description)
    {
        if (product.definition.id == "com.coidea.forestMatch.full")
        {
            Debug.Log($"Failed: {description.message}");
        }
    }
    
    public void OnProductFetched(Product product)
    {
        Debug.Log("Fetched");
        _buttonText.text = product.metadata.localizedPriceString;
    }
}
