using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    #region Fields
    private OptionManager optionManager;

    /* Options */
    [SerializeField] private GameObject[] InOptionPanels; // 0 : Sound; 1 : Language; 2 : Resolution

    /* SoundEffect - BGM */
    public Slider seSlider;
    public Slider bgmSlider;
    [SerializeField] private TMP_Text[] Mute_TMP;
    #endregion

    #region MonoBehaciour
    private void OnEnable()
    {
        optionManager = OptionManager.Instance;
        InitSoundText();
        InitSoundSlider();
    }

    #endregion

    #region OnEnable
    private void InitSoundText()
    {
        Mute_TMP[0].text = SoundEffectManager.isMute ? "O" : "X";
        Mute_TMP[1].text = BgmManager.bgm.mute ? "O" : "X";
    }

    private void InitSoundSlider()
    {
        seSlider.value = SoundEffectManager.volume;
        bgmSlider.value = BgmManager.bgm.volume;
    }
    #endregion

    #region Option - Basement   
    public void SelectOptionPanel(int panelNum)
    {
        SoundEffectManager.PlaySound(Sound.Button);
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
    public void SetSEVolume(float value)    // for slider
    {
        optionManager.SetSEVolume(value);
    }

    public void ToggleSeMute()
    {
        optionManager.MakeSEMute(!SoundEffectManager.isMute);
        Mute_TMP[0].text = SoundEffectManager.isMute ? "O" : "X";
        SoundEffectManager.PlaySound(Sound.Button);
    }

    public void SetBGMVolume(float value)   // for slider
    {
        optionManager.SetBGMVolume(value);
    }

    public void ToggleBgmMute()
    {
        optionManager.MakeBGMMute(!BgmManager.bgm.mute);
        Mute_TMP[1].text = BgmManager.bgm.mute ? "O" : "X";
        SoundEffectManager.PlaySound(Sound.Button);
    }
    #endregion

    #region Set Language
    public void ChangeLocale(int index)
    {
        SoundEffectManager.PlaySound(Sound.Button);
        optionManager.ChangeLocale(index);
    }
    #endregion


}
