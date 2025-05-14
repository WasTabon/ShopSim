using UnityEngine;
using UnityEngine.Purchasing;

public class IAPInitializer : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    [SerializeField] private LootboxSpinner _lootboxSpinner;
    
    public string productId = "com.coidea.forestMatch.full";

    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(productId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error) { }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) { }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == productId)
        {
            _lootboxSpinner.StartSpin();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void BuyProduct()
    {
        if (storeController != null)
        {
            storeController.InitiatePurchase(productId);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }
}