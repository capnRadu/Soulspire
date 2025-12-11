using System;

[Serializable]
public enum IAPProductKey
{
    WizardBundle,
    MasteryBundle,
    KingBundle,
    SoulsDoubler,
    XpDoubler,
    Diamonds100,
    Diamonds600,
    Diamonds1300,
    Diamonds2800
}

[Serializable]
public class IAPPayData
{
    public string Payload;
    public string Store;
    public string TransactionID;
}

[Serializable]
public class IAPPayload
{
    public string json;
    public string signature;
    public IAPPayloadData payloadData;
}

[Serializable]
public class IAPPayloadData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity;
    public bool aknowledged;
}