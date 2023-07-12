using Unity.VisualScripting;
using UnityEngine;

public class SignInPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject EmailPanel;
    #endregion

    #region Toggle Panels
    public void ToggleEmailPanel(bool b) => EmailPanel.SetActive(b);
    #endregion

    #region Google Login
    public void GoogleLogin() => DatabaseManager.Instance.GoogleSignInClick();
    #endregion
}