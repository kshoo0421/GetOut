using UnityEngine;

public class C_ItemManager : MonoBehaviour
{
    #region �̱��� ������
    static C_ItemManager instance;
    C_ItemManager() { }
    public static C_ItemManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<C_ItemManager>();
        }
    }
    #endregion
}