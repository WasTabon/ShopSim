using System;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class RouletteBuy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private GameObject _loadingButton;
    [SerializeField] private LootboxSpinner _lootboxSpinner;

    private void Awake()
    {
        _loadingButton.SetActive(false);
    }

    public void ShowLoadingButton()
    {
        _loadingButton.SetActive(true);
    }
    
    public void OnPurchaseComlete(Product product)
    {
        if (product.definition.id == "com.coidea.forestMatch.full")
        {
            Debug.Log("Complete");
            _loadingButton.SetActive(false);
            _lootboxSpinner.StartSpin();
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription description)
    {
        if (product.definition.id == "com.coidea.forestMatch.full")
        {
            _loadingButton.SetActive(false);
            Debug.Log($"Failed: {description.message}");
        }
    }
    
    public void OnProductFetched(Product product)
    {
        Debug.Log("Fetched");
        _buttonText.text = product.metadata.localizedPriceString;
    }
}
