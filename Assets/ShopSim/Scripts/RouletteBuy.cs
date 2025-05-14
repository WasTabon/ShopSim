using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class RouletteBuy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private LootboxSpinner _lootboxSpinner;
    
    public void OnPurchaseComlete(Product product)
    {
        if (product.definition.id == "com.coidea.forestMatch.full")
        {
            _lootboxSpinner.StartSpin();
        }
    }
    
    public void OnProductFetched(Product product)
    {
        _buttonText.text = product.metadata.localizedPriceString;
    }
}
