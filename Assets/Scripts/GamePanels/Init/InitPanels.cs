using TMPro;
using UnityEngine;

public class InitPanels : MonoBehaviour
{
    [SerializeField] private GameObject ErrorPanel;
    [SerializeField] private TMP_Text notif_Title_Text, notif_MessageText;

    #region Panel Control
    public void OpenErrorPanel() => ErrorPanel.SetActive(true);

    public void CloseErrorPanel() => ErrorPanel.SetActive(false);
    #endregion
    protected void showNotificationMessage(string title, string message)
    {
        OpenErrorPanel();
        notif_Title_Text.text = "" + title;
        notif_MessageText.text = "" + message;
    }
}
