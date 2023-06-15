using Photon.Pun;
using UnityEngine;

public class Scenes : MonoBehaviour
{
    #region Field
    /* Managers */
    protected FirebaseManager firebaseManager;
    protected GoogleAdMobManager googleAdMobManager;
    protected ItemManager itemManager;
    protected OptionManager optionManager;
    protected PhotonManager photonManager;
    protected PaymentManager paymentManager;
    /* Options */
    [SerializeField] GameObject OptionPanel;
    [SerializeField] GameObject[] InOptionPanels; // 0 : Sound; 1 : Language; 2 : Resolution

    /* SoundEffect */
    [SerializeField] AudioSource[] soundEffects;
    float sEVolume;

    /* BGM */
    [SerializeField] AudioSource[] bgms;
    float bgmVolume;
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    #endregion

    #region Set Managers
    protected void SetManagers()
    {
        firebaseManager = FirebaseManager.Instance;
        googleAdMobManager = GoogleAdMobManager.Instance;
        itemManager = ItemManager.Instance;
        optionManager = OptionManager.Instance;
        photonManager = PhotonManager.Instance;
        paymentManager = PaymentManager.Instance;
    }
    #endregion

    #region Option - Basement   
    public void ToggleOptionPanel()
    {
        if (OptionPanel.activeSelf)
        {
            OptionPanel.SetActive(false);
        }
        else
        {
            OptionPanel.SetActive(true);
        }
    }

    public void SelectOptionPanel(int panelNum)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == panelNum)
            {
                InOptionPanels[i].SetActive(true);
            }
            else
            {
                InOptionPanels[i].SetActive(false);
            }
        }
    }
    #endregion

    #region Option - Sound
    protected void InitializeVolume() // Initialize
    {
        sEVolume = PlayerPrefs.GetFloat("SEVolume");
        ApplySEVolume();

        bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        ApplyBGMVolume();
    }

    public void SetSEVolume(float value)
    {
        sEVolume = value;
        PlayerPrefs.SetFloat("SEVolume", sEVolume);
        ApplySEVolume();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        ApplyBGMVolume();
    }

    void ApplySEVolume()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].volume = sEVolume;
        }
    }

    void ApplyBGMVolume()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].volume = bgmVolume;
        }
    }
    #endregion

    #region Open Keyboard
    TouchScreenKeyboard keyboard;
    public void OpenKeyboard() => keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

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
    [SerializeField] GameObject QuitPanel;

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
