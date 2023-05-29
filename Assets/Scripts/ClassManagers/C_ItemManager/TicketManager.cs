using UnityEngine;

public class TicketManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static TicketManager instance;
    private TicketManager() { }
    public static TicketManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<TicketManager>();
        }
    }
    #endregion
}
