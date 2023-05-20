using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    #region �̱��� ������
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
