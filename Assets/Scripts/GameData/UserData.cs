using System;

[Serializable]
public struct UserData
{
    // basic info
    public string id;
    public string email;
    public string nickName;

    // item info
    public ItemData itemData;
}

[Serializable]
public struct ItemData
{
    public long gold;
    public long ticket;
    public long extra_ticket;
    public string ticket_time;
}