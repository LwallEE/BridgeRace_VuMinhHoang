using System;

[Serializable]
public class ShopRequest
{
    public int itemId;

    public ShopRequest(int id)
    {
        itemId = id;
    }
}
