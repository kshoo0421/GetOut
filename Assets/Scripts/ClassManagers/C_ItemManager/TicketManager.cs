using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static TicketManager instance;
    private TicketManager() { }
    public static TicketManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TicketManager();
            }
            return instance;
        }
    }
    #endregion
}
