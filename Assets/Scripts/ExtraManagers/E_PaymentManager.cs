using UnityEngine;

public class E_PaymentManager : MonoBehaviour
{
    #region Field
    /* Singleton*/
    static E_PaymentManager instance;
    #endregion

    #region Singleton
    E_PaymentManager() { }
    public static E_PaymentManager Instance { get { return instance; } }

    private void SetSingleton()
    {
        if (instance == null)
        {
            instance = GetComponent<E_PaymentManager>();
        }
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
    #endregion
}