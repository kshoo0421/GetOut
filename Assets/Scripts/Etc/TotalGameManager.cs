using UnityEngine;

public class TotalGameManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static TotalGameManager instance;

    /* Managers */
    // 베이스 매니저 - B_OptionManager, B_DatabaseManager, B_NetworkManager
    public FirebaseManager firebaseManager;
    public PhotonManager photonManager;
    public LanguageManager languageManager;
    public ResolutionManager resolutionManager;
    public SoundManager soundManager;
    public VisualEffectManager visualEffectManager;
    // 베이스 매니저
    public B_DatabaseManager b_DatabaseManager;
    public B_InputOutputManager b_InputOutputManager;
    public B_NetworkManager b_NetworkManager;
    public B_OptionManager b_OptionManager;
    public B_SceneChangeManager b_SceneChangeManager;
    public B_TimeManager b_TimeManager;
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
    public E_AdManager e_AdManager;
    public E_CustumMatchingChatManager e_CustumMatchingChatManager;
    public E_EventManger e_EventManger;
    public E_LobbyChatManager e_LobbyChatManager;
    public E_PaymentManager e_PaymentManager;
    public GoogleAdMobManager googleAdMobManager;
    #endregion

    #region Singleton
    TotalGameManager() { }

    public static TotalGameManager Instance
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
        firebaseManager = FirebaseManager.Instance;
        photonManager = PhotonManager.Instance;

        languageManager = LanguageManager.Instance;
        resolutionManager = ResolutionManager.Instance;
        soundManager = SoundManager.Instance;
        visualEffectManager = VisualEffectManager.Instance;

        b_DatabaseManager = B_DatabaseManager.Instance;
        b_InputOutputManager = B_InputOutputManager.Instance;
        b_NetworkManager = B_NetworkManager.Instance;
        b_OptionManager = B_OptionManager.Instance;
        b_SceneChangeManager = B_SceneChangeManager.Instance;
        b_TimeManager = B_TimeManager.Instance;

        c_DataManager = C_DataManager.Instance;
        c_ItemManager = C_ItemManager.Instance;
        c_MissionManager = C_MissionManager.Instance;

        emojiManager = EmojiManager.Instance;
        extraTicketManager = ExtraTicketManager.Instance;
        goldManager = GoldManager.Instance;
        ticketManager = TicketManager.Instance;

        e_AdManager = E_AdManager.Instance;
        e_CustumMatchingChatManager = E_CustumMatchingChatManager.Instance;
        e_EventManger = E_EventManger.Instance;
        e_LobbyChatManager = E_LobbyChatManager.Instance;
        e_PaymentManager = E_PaymentManager.Instance;
        googleAdMobManager = GoogleAdMobManager.Instance;
    }
    #endregion
}
