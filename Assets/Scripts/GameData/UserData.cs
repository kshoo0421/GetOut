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

    // pref info
    public PrefsData prefsdata;
}

[Serializable]
public struct ItemData
{
    public long gold;
    public long ticket;
    public long extra_ticket;
    public string ticket_time;
}

[Serializable]
public struct PrefsData
{
    public long isBannerOpen;
    public long LocaleNum;
    public long isVolumeFirst;
    public long isSEMute;
    public long isBGMMute;
    public string SEVolume;
    public string BGMVolume;
}