using UnityEngine;

public class B_TotalGameManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static B_TotalGameManager instance;

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
    // 클래스 매니저 - C_ItemManager
    public EmojiManager emojiManager;
    public ExtraTicketManager extraTicketManager;
    public GoldManager goldManager;
    public TicketManager ticketManager;
    // 기타 매니저
    public E_CustumMatchingChatManager e_CustumMatchingChatManager;
    public E_EventManger e_EventManger;
    public E_LobbyChatManager e_LobbyChatManager;
    public E_GoogleAdMobManager e_GoogleAdMobManager;
    public E_GooglePayManager e_GooglePayManager;
    #endregion

    #region Singleton
    B_TotalGameManager() { }

    public static B_TotalGameManager Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();
        SetManagers();
    }
    #endregion

    #region Set Managers
    private void SetManagers()
    {
        Debug.Log("Set Manager 시작");
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

        emojiManager = EmojiManager.Instance;
        extraTicketManager = ExtraTicketManager.Instance;
        goldManager = GoldManager.Instance;
        ticketManager = TicketManager.Instance;

        e_CustumMatchingChatManager = E_CustumMatchingChatManager.Instance;
        e_EventManger = E_EventManger.Instance;
        e_LobbyChatManager = E_LobbyChatManager.Instance;
        e_GooglePayManager = E_GooglePayManager.Instance;
        e_GoogleAdMobManager = E_GoogleAdMobManager.Instance;
        Debug.Log("total 완");
    }
    #endregion
}
