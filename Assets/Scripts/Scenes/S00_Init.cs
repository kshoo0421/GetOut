using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S00_Init : Scenes
{
    #region Field
    /* Initialize */
    private bool isInitialized = false;
    [SerializeField] private GameObject BeforeLoad;
    [SerializeField] private GameObject AfterLoad;

    /* Sign In */
    [SerializeField] private GameObject SignInPanel;
    [SerializeField] private GameObject ErrorPanel;

    [SerializeField] private GameObject OptionPanel;
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
    public void OpenOptionPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        OptionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        OptionPanel.SetActive(false);
    }

    public void OpenSignInPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        SignInPanel.SetActive(true);
    }
    public void CloseSignInPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        SignInPanel.SetActive(false);
    }

    public void OpenErrorPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        ErrorPanel.SetActive(true);
    }

    public void CloseErrorPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        ErrorPanel.SetActive(false);
    }
    #endregion
}