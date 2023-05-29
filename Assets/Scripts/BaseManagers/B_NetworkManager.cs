using UnityEngine;

public class B_NetworkManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    static B_NetworkManager instance;
    B_NetworkManager() { }
    public static B_NetworkManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null) instance = GetComponent<B_NetworkManager>();
    }
    #endregion
}