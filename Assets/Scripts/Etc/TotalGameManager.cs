using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalGameManager : MonoBehaviour
{
    #region �̱��� ����
    private static TotalGameManager instance;
    private TotalGameManager() { }

    public static TotalGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TotalGameManager();
            }
            return instance;
        }
    }
    #endregion

    #region �Ŵ����� ����
    public FirebaseManager firebaseManager;
    
    public PhotonManager photonManager;
    
    public LanguageManager languageManager;
    public ResolutionManager resolutionManager;
    public SoundManager soundManager;
    public VisualEffectManager visualEffectManager;
    // ���̽��Ŵ���
    public B_DatabaseManager b_DatabaseManager;
    public B_InputOutputManager b_InputOutputManager;
    public B_NetworkManager b_NetworkManager;
    public B_OptionManager b_OptionManager;
    public B_SceneChangeManager b_SceneChangeManager;
    public B_TimeManager b_TimeManager;
    
    // Ŭ���� �Ŵ���
    public C_DataManager c_DataManager;
    public C_ItemManager c_ItemManager;
    public C_MissionManager c_MissionManager;
    // Ŭ�����Ŵ��� - C_ItemManager
    public EmojiManager emojiManager;
    public ExtraTicketManager extraTicketManager;
    public GoldManager goldManager;
    public TicketManager ticketManager;
    #endregion

    #region 
    private void Start()
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
    }
    #endregion


}
