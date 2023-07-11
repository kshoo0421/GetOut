using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S00_Initial : Scenes
{
    #region Field
    /* Initialize */
    private bool isInitialized = false;
    [SerializeField] private GameObject BeforeLoad;
    [SerializeField] private GameObject AfterLoad;

    /* Sign In */
    [SerializeField] private GameObject SignInPanel;
    [SerializeField] private GameObject ErrorPanel;

    #endregion

    #region MonoBehaviour
    private void Start()
    {
        InitialSet();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            if (DatabaseManager.IsFirebaseReady && PhotonManager.IsPhotonReady)
            {
                BeforeLoad.SetActive(false);
                AfterLoad.SetActive(true);
                isInitialized = true;
                Debug.Log("초기화 완료");
            }
        }
    }
    #endregion

    #region Panels Control
    public void OpenSignInPanel() => SignInPanel.SetActive(true);
    public void CloseSignInPanel() => SignInPanel.SetActive(false);

    public void OpenErrorPanel() => ErrorPanel.SetActive(true);
    public void CloseErrorPanel() => ErrorPanel.SetActive(false);
    #endregion
}