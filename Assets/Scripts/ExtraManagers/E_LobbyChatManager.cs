using UnityEngine;

public class E_LobbyChatManager : MonoBehaviour
{
    #region Field
    /* Singleton*/
    static E_LobbyChatManager _instance;
    #endregion

    #region Singleton
    public static E_LobbyChatManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new E_LobbyChatManager();
            }
            return _instance;
        }
    }
    #endregion

}
