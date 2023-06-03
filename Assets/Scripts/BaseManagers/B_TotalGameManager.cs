using UnityEngine;

public class B_TotalGameManager : BehaviorSingleton<B_TotalGameManager>
{
    #region Field
    /* Managers */
    // B_OptionManager
    public LanguageManager languageManager;
    public ResolutionManager resolutionManager;
    public SoundManager soundManager;
    public VisualEffectManager visualEffectManager;
    // 베이스 매니저
    public B_OptionManager b_OptionManager;
    public B_SceneChangeManager b_SceneChangeManager;
    public B_FirebaseManager firebaseManager;
    public B_PhotonManager photonManager;
    // 클래스 매니저
    public C_DataManager c_DataManager;
    public C_ItemManager c_ItemManager;
    public C_MissionManager c_MissionManager;
    // 기타 매니저
    public E_CustumMatchingChatManager e_CustumMatchingChatManager;
    public E_EventManger e_EventManger;
    public E_LobbyChatManager e_LobbyChatManager;
    public E_GoogleAdMobManager e_GoogleAdMobManager;
    public E_GooglePayManager e_GooglePayManager;
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetManagers();
    }
    #endregion

    #region Set Managers
    private void SetManagers()
    {
        languageManager = LanguageManager.Instance;
        resolutionManager = ResolutionManager.Instance;
        soundManager = SoundManager.Instance;
        visualEffectManager = VisualEffectManager.Instance;

        firebaseManager = B_FirebaseManager.Instance;
        photonManager = B_PhotonManager.Instance;
        b_OptionManager = B_OptionManager.Instance;
        b_SceneChangeManager = B_SceneChangeManager.Instance;

        c_DataManager = C_DataManager.Instance;
        c_ItemManager = C_ItemManager.Instance;
        c_MissionManager = C_MissionManager.Instance;

        e_CustumMatchingChatManager = E_CustumMatchingChatManager.Instance;
        e_EventManger = E_EventManger.Instance;
        e_LobbyChatManager = E_LobbyChatManager.Instance;
        e_GooglePayManager = E_GooglePayManager.Instance;
        e_GoogleAdMobManager = E_GoogleAdMobManager.Instance;
    }
    #endregion
}
