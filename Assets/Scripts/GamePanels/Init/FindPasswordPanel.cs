using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindPasswordPanel : InitPanels
{
    #region Fields
    [SerializeField] private TMP_InputField EmailInputField;
    [SerializeField] private TMP_Text FindPasswordMessage, notif_Title_Text, notif_MessageText;
    #endregion

    #region Monobehaviour


    #endregion

    #region FindPassword
    public void ForgetPass()
    {
        if(string.IsNullOrEmpty(EmailInputField.text))
        {
            showNotificationMessage("Error", "Forget Email Empty");
            return;
        }
    }

    #endregion


}