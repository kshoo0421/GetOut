using TMPro;
using UnityEngine;

public class PlusTicketPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject fullTicketPanel;
    [SerializeField] private GameObject notFullTicketPanel;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        if (DatabaseManager.userData.itemData.ticket == 5)
        {
            fullTicketPanel.SetActive(true);
            notFullTicketPanel.SetActive(false);
        }
        else
        {
            fullTicketPanel.SetActive(false);
            notFullTicketPanel.SetActive(true);
        }
    }
    #endregion
}
