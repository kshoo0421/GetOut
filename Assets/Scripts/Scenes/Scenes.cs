using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenes : MonoBehaviour
{
    #region Field
    /* Managers */
    protected DatabaseManager databaseManager;
    protected GoogleAdMobManager googleAdMobManager;
    protected OptionManager optionManager;
    protected PhotonManager photonManager;
    protected PaymentManager paymentManager;

    /* Quit */
    [SerializeField] private GameObject QuitPanel;    
    #endregion

    #region  For Monobehaviour
    protected void ForUpdate()
    {
        if (!databaseManager.isFullTicket())
        {
            databaseManager.AutoFillTicket();
        }
    }
    #endregion

    #region Change Scene
    public void ChangeToScene(string sceneName) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    #endregion

    #region InitialSet
    protected void InitialSet()
    {
        SetManagers();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            googleAdMobManager.ToggleBannerAd(false);
        }
        else
        {
            googleAdMobManager.ToggleBannerAd(PrefsBundle.isBannerOpen == 1);
        }
    }
    #endregion

    #region Set Managers
    protected void SetManagers()
    {
        databaseManager = DatabaseManager.Instance;
        googleAdMobManager = GoogleAdMobManager.Instance;
        optionManager = OptionManager.Instance;
        photonManager = PhotonManager.Instance;
        paymentManager = PaymentManager.Instance;
    }
    #endregion

    #region Open Keyboard
    private TouchScreenKeyboard keyboard;
    public void OpenDefaultKeyboard() => keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

    public void OpenNumberKeyboard() => keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);

    public void CloseKeyboard()
    {
        if (keyboard != null)
        {
            keyboard.active = false;
            keyboard = null;
        }
    }
    #endregion

    #region Quit Game
    public void ToggleQuitPanel()
    {
        if (QuitPanel.activeSelf)
        {
            QuitPanel.SetActive(false);
        }
        else
        {
            QuitPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}