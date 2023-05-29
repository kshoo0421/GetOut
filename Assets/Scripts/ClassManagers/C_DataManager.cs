using UnityEngine;

public class C_DataManager : MonoBehaviour
{
    #region �̱��� ������
    static C_DataManager instance;
    C_DataManager() { }
    public static C_DataManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<C_DataManager>();
        }
    }
    #endregion
}