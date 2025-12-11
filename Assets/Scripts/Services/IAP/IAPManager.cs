using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    [SerializeField] private string wizardBundleProductId = "wizard_bundle";
    [SerializeField] private string masteryBundleProductId = "mastery_bundle";
    [SerializeField] private string kingBundleProductId = "king_bundle";
    [SerializeField] private string soulsDoublerProductId = "souls_doubler";
    [SerializeField] private string xpDoublerProductId = "xp_doubler";
    [SerializeField] private string diamonds100ProductId = "diamonds_100";
    [SerializeField] private string diamonds600ProductId = "diamonds_600";
    [SerializeField] private string diamonds1300ProductId = "diamonds_1300";
    [SerializeField] private string diamonds2800ProductId = "diamonds_2800";

    public static bool IsInitialized { get; private set; } = false;
    private static StoreController storeController;

    [SerializeField] private ShopMenu shopMenu;
    [SerializeField] private AudioSource purchaseSound;

    private async void Awake()
    {
        await InitIAP();
    }

    private async Task InitIAP()
    {
        try
        {
            var option = new InitializationOptions().SetEnvironmentName("production");
            await UnityServices.InitializeAsync(option);

            storeController = UnityIAPServices.StoreController();

            storeController.OnStoreDisconnected += OnStoreDisconnected;
            storeController.OnProductsFetched += OnProductsFetched;
            storeController.OnProductsFetchFailed += OnProductsFetchFailed;
            storeController.OnPurchasesFetched += OnPurchasesFetched;
            storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
            storeController.OnPurchasePending += OnPurchasePending;
            storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            storeController.OnPurchaseFailed += OnPurchaseFailed;
            storeController.OnPurchaseDeferred += OnPurchaseDeferred;

            RegisterEntitlementCallback();

            await storeController.Connect();

            var initialProductToFetch = BuildProductDefinitions();
            storeController.FetchProducts(initialProductToFetch);
        }
        catch (Exception e)
        {
            Debug.LogError($"IAP Initialization failed: {e.Message}");
        }
    }

    private void RegisterEntitlementCallback()
    {
        storeController.OnCheckEntitlement += (result) =>
        {
            Product product = result.Product;
            var status = result.Status;

            Debug.Log($"Entitlement check for product {product}: {status}");

            // Only for non-consumable products or subscriptions
            bool isEntitled = status == EntitlementStatus.FullyEntitled;

            if (isEntitled)
            {
                if (product.definition.id == soulsDoublerProductId)
                {
                    shopMenu.PurchaseDoubleSoulsBuff();
                }

                if (product.definition.id == xpDoublerProductId)
                {
                    shopMenu.PurchaseDoubleXPBuff();
                }
            }
        };
    }

    private List<ProductDefinition> BuildProductDefinitions()
    {
        var initialProductToFetch = new List<ProductDefinition>();
        initialProductToFetch.Add(new ProductDefinition(wizardBundleProductId, ProductType.Consumable));
        initialProductToFetch.Add(new ProductDefinition(masteryBundleProductId, ProductType.Consumable));
        initialProductToFetch.Add(new ProductDefinition(kingBundleProductId, ProductType.Consumable));
        initialProductToFetch.Add(new ProductDefinition(soulsDoublerProductId, ProductType.NonConsumable));
        initialProductToFetch.Add(new ProductDefinition(xpDoublerProductId, ProductType.NonConsumable));
        initialProductToFetch.Add(new ProductDefinition(diamonds100ProductId, ProductType.Consumable));
        initialProductToFetch.Add(new ProductDefinition(diamonds600ProductId, ProductType.Consumable));
        initialProductToFetch.Add(new ProductDefinition(diamonds1300ProductId, ProductType.Consumable));
        initialProductToFetch.Add(new ProductDefinition(diamonds2800ProductId, ProductType.Consumable));

        return initialProductToFetch;
    }

    private void OnProductsFetched(List<Product> products)
    {
        storeController.FetchPurchases();

        foreach (var product in products)
        {
            string price = product.metadata.localizedPriceString + " " + product.metadata.isoCurrencyCode;
            shopMenu.UpdateButtonPrice(product.definition.id, price);
        }
    }

    private void OnProductsFetchFailed(ProductFetchFailed reason)
    {
        Debug.LogError($"Product fetch failed: {reason}");
    }

    private void OnPurchasesFetched(Orders orders)
    {
        IsInitialized = true;

        foreach (var product in storeController.GetProducts())
        {
            storeController.CheckEntitlement(product);
        }
    }
    
    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription reason)
    {
        Debug.LogError($"Purchases fetch failed: {reason}");
    }

    private void OnStoreDisconnected(StoreConnectionFailureDescription description)
    {
        Debug.LogWarning($"Store disconnected: {description.message}");
    }

    public void BuyProduct(IAPProductKey productKey)
    {
        if (!IsInitialized)
        {
            Debug.LogWarning("IAP Manager is not initialized.");
            return;
        }

        string productId = productKey switch
        {
            IAPProductKey.WizardBundle => wizardBundleProductId,
            IAPProductKey.MasteryBundle => masteryBundleProductId,
            IAPProductKey.KingBundle => kingBundleProductId,
            IAPProductKey.SoulsDoubler => soulsDoublerProductId,
            IAPProductKey.XpDoubler => xpDoublerProductId,
            IAPProductKey.Diamonds100 => diamonds100ProductId,
            IAPProductKey.Diamonds600 => diamonds600ProductId,
            IAPProductKey.Diamonds1300 => diamonds1300ProductId,
            IAPProductKey.Diamonds2800 => diamonds2800ProductId,
            _ => throw new NotImplementedException()
        };

        storeController.PurchaseProduct(productId);
    }

    private void OnPurchasePending(PendingOrder order)
    {
        Debug.Log($"Pending order: {order}");
        storeController.ConfirmPurchase(order);
    }

    private void OnPurchaseDeferred(DeferredOrder order)
    {
        Debug.Log($"Purchase deferred for product: {order?.Info}");
        // UI "Purchase pending approval" can be shown here
    }

    private void OnPurchaseConfirmed(Order order)
    {
        Debug.Log($"Purchase confirmed: {order}");
        
        if (order?.Info?.PurchasedProductInfo != null && order.Info.PurchasedProductInfo.Count > 0)
        {
            int quantity = GetPurchaseQuantity(order);
            string productId = order.Info.PurchasedProductInfo[0].productId;

            switch (productId)
            {
                case var id when id == wizardBundleProductId:
                    shopMenu.PurchaseWizardBundle(quantity);
                    break;
                case var id when id == masteryBundleProductId:
                    shopMenu.PurchaseMasteryBundle(quantity);
                    break;
                case var id when id == kingBundleProductId:
                    shopMenu.PurchaseKingBundle(quantity);
                    break;
                case var id when id == soulsDoublerProductId:
                    shopMenu.PurchaseDoubleSoulsBuff();
                    break;
                case var id when id == xpDoublerProductId:
                    shopMenu.PurchaseDoubleXPBuff();
                    break;
                case var id when id == diamonds100ProductId:
                    shopMenu.PurchaseBagOfDiamonds(quantity);
                    break;
                case var id when id == diamonds600ProductId:
                    shopMenu.PurchaseBucketOfDiamonds(quantity);
                    break;
                case var id when id == diamonds1300ProductId:
                    shopMenu.PurchaseBarrelOfDiamonds(quantity);
                    break;
                case var id when id == diamonds2800ProductId:
                    shopMenu.PurchaseChestOfDiamonds(quantity);
                    break;
                default:
                    Debug.LogWarning($"Unrecognized product ID: {productId}");
                    break;
            }

            purchaseSound.Play();
        }
    }

    private int GetPurchaseQuantity(Order order)
    {
        int quantity = 1;
        string receipt = order.Info.Receipt;

        if (!string.IsNullOrEmpty(receipt))
        {
            var payData = JsonUtility.FromJson<IAPPayData>(receipt);

            if (payData.Store != "fake")
            {
                IAPPayload payload = JsonUtility.FromJson<IAPPayload>(payData.Payload);
                IAPPayloadData payloadData = JsonUtility.FromJson<IAPPayloadData>(payload.json);
                quantity = payloadData.quantity;
            }
        }

        return quantity;
    }

    private void OnPurchaseFailed(FailedOrder order)
    {
        if (order?.Info?.PurchasedProductInfo == null || order.Info.PurchasedProductInfo.Count == 0)
        {
            Debug.LogError("Purchase failed: No product info available.");
            return;
        }

        string productId = order.Info.PurchasedProductInfo[0].productId;
        var reason = order.FailureReason;
        var message = order.Details;
        Debug.LogError($"Purchase failed for product {productId}: {reason} - {message}");
    }

    public void RestoreAllPurchases()
    {
        storeController.RestoreTransactions((success, error) =>
        {
            if (success)
            {
                Debug.Log("Restore purchases succeeded.");
            }
            else
            {
                Debug.LogError($"Restore purchases failed: {error}");
            }
        });
    }

    private void OnDisable()
    {
        storeController.OnStoreDisconnected -= OnStoreDisconnected;
        storeController.OnProductsFetched -= OnProductsFetched;
        storeController.OnProductsFetchFailed -= OnProductsFetchFailed;
        storeController.OnPurchasesFetched -= OnPurchasesFetched;
        storeController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;
        storeController.OnPurchasePending -= OnPurchasePending;
        storeController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
        storeController.OnPurchaseFailed -= OnPurchaseFailed;
        storeController.OnPurchaseDeferred -= OnPurchaseDeferred;
    }
}