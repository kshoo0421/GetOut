using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    /* SoundEffect - BGM */
    [SerializeField] AudioSource[] soundEffects;
    public Slider seSlider;
    public Slider bgmSlider;
    float seVolume, bgmVolume;
    bool isSEMute = false, isBGMMute = false;
    [SerializeField] TMP_Text[] Mute_TMP;

    
    /* Quit */
    [SerializeField] GameObject QuitPanel;
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    #endregion

    protected void InitialSet()
    {
        SetManagers();
        InitializeVolume();
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            googleAdMobManager.ToggleBannerAd();
        }
    }

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
    void InitializeVolume() // Initialize
    {
        if (PlayerPrefs.GetInt("isVolumeFirst") == 0)
        {
            seVolume = 1.0f;
            bgmVolume = 1.0f;
            PlayerPrefs.SetInt("isVolumeFirst", 1);
        }
        else
        {
            isSEMute = PlayerPrefs.GetInt("isSEMute") == 1 ? true : false;
            isBGMMute = PlayerPrefs.GetInt("isBGMMute") == 1 ? true : false;

            MakeSEMute(isSEMute);
            MakeBGMMute(isBGMMute);

            seVolume = PlayerPrefs.GetFloat("SEVolume");
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        }
        ApplySEVolume();
        ApplyBGMVolume();
    }

    public void SetSEVolume(float value)
    {
        if (value == 0) return; // Destroy 시 초기화 방지
        seVolume = value;
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        ApplySEVolume();
    }
    
    public void ToggleSeMute() => MakeSEMute(!isSEMute);

    void MakeSEMute(bool b)
    {
        isSEMute = b;
        PlayerPrefs.SetInt("isSEMute", b ? 1 : 0);
        Mute_TMP[0].text = b ? "O" : "X";
        for (int i = 0; i < soundEffects.Length; i++) soundEffects[i].mute = b;
    }

    void ApplySEVolume()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].volume = seVolume;
        }
        seSlider.value = seVolume;
    }

    public void SetBGMVolume(float value)
    {
        if (value == 0) return; // Destroy 시 초기화 방지
        bgmVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        ApplyBGMVolume();
    }

    public void ToggleBgmMute() => MakeBGMMute(!isBGMMute);

    void MakeBGMMute(bool b)
    {
        isBGMMute = b;
        PlayerPrefs.SetInt("isBGMMute", b ? 1 : 0);
        Mute_TMP[1].text = b ? "O" : "X";
        BGM.bgm.mute = b;
    }

    void ApplyBGMVolume()
    {
        BGM.bgm.volume = bgmVolume;
        bgmSlider.value = bgmVolume;
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

    #region Set Language
    public void ChangeLocale(int index)
    {
        optionManager.ChangeLocale(index);
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
