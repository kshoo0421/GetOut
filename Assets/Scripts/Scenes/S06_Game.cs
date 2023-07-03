using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class S06_Game : Scenes
{
    #region Field
    int curPlayer;

    [SerializeField] TMP_Text TurnNumText;  // 몇 번째 턴인지
    [SerializeField] TMP_Text LeftTime;

    /* Panels */
    [SerializeField] GameObject SuggestPanel;
    [SerializeField] GameObject GetPanel;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject MissionSetPanel;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] GameObject WaitingPanel;


    [SerializeField] GameObject MissionCheckPanel;
    [SerializeField] GameObject QuitBtnPanel;

    /* Game */
    bool isTmdInit = false;
    string[] playerName = new string[4];
    int curTurnNum;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        SetMissionCheckData();
    }
    void Update()
    {
        ForUpdate();
        if(SuggestPanel.activeSelf == true)
        {
            CheckGoldAmount();
        }
    }
    #endregion

    #region State Set
    public void TimerSet(int sec)
    {
        
    }

    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene(3);

    public void BackToRandom() => ChangeToScene(4);
    #endregion

    #region Set Game
    public void SetGame()
    {
        FirebaseManager.gameData = new GameData();
        if (FirebaseManager.MyPlayer.view.ViewID == 1001)
        {
            FirebaseManager.turnMatchData = new TurnMatchData();
            FirebaseManager.turnMatchData.turn = new Turn[6];
            SetOpponent();
            FirebaseManager.MyPlayer.SetTmdIfP1();
        }
    }

    void SetOpponent()
    {
        Turn[] turns = InitTurns();
        System.Random random = new System.Random();
        for (int i = 0; i < 3; i++)
        {
            if (random.Next() % 2 == 0)
            {
                FirebaseManager.turnMatchData.turn[i * 2] = turns[i * 4];
                FirebaseManager.turnMatchData.turn[i * 2 + 1] = turns[i * 4 + 1];

            }
            else
            {
                FirebaseManager.turnMatchData.turn[i * 2] = turns[i * 4 + 2];
                FirebaseManager.turnMatchData.turn[i * 2 + 1] = turns[i * 4 + 3];
            }
        }
        FirebaseManager.turnMatchData.turn = FirebaseManager.turnMatchData.turn.OrderBy(x => random.Next()).ToArray();
    }

    Turn[] InitTurns()
    {
        Turn[] turns = new Turn[12];
        turns[0].Room1.getter = 1;
        turns[0].Room1.proposer = 2;
        turns[0].Room2.getter = 3;
        turns[0].Room2.proposer = 4;

        turns[1].Room1.getter = 2;
        turns[1].Room1.proposer = 1;
        turns[1].Room2.getter = 4;
        turns[1].Room2.proposer = 3;
        //----------------------------
        turns[2].Room1.getter = 1;
        turns[2].Room1.proposer = 2;
        turns[2].Room2.getter = 4;
        turns[2].Room2.proposer = 3;

        turns[3].Room1.getter = 2;
        turns[3].Room1.proposer = 1;
        turns[3].Room2.getter = 3;
        turns[3].Room2.proposer = 4;
        //----------------------------
        turns[4].Room1.getter = 1;
        turns[4].Room1.proposer = 3;
        turns[4].Room2.getter = 2;
        turns[4].Room2.proposer = 4;

        turns[5].Room1.getter = 3;
        turns[5].Room1.proposer = 1;
        turns[5].Room2.getter = 4;
        turns[5].Room2.proposer = 2;
        //----------------------------
        turns[6].Room1.getter = 1;
        turns[6].Room1.proposer = 3;
        turns[6].Room2.getter = 4;
        turns[6].Room2.proposer = 2;

        turns[7].Room1.getter = 3;
        turns[7].Room1.proposer = 1;
        turns[7].Room2.getter = 2;
        turns[7].Room2.proposer = 4;
        //----------------------------
        turns[8].Room1.getter = 1;
        turns[8].Room1.proposer = 4;
        turns[8].Room2.getter = 2;
        turns[8].Room2.proposer = 3;

        turns[9].Room1.getter = 4;
        turns[9].Room1.proposer = 1;
        turns[9].Room2.getter = 3;
        turns[9].Room2.proposer = 2;
        //----------------------------
        turns[10].Room1.getter = 1;
        turns[10].Room1.proposer = 4;
        turns[10].Room2.getter = 3;
        turns[10].Room2.proposer = 2;

        turns[11].Room1.getter = 4;
        turns[11].Room1.proposer = 1;
        turns[11].Room2.getter = 2;
        turns[11].Room2.proposer = 3;

        return turns;
    }

    public void SetPlayerMission(int playerNumber, MissionLevel missionLevel)
    {
        System.Random random = new System.Random();
        int tmp = random.Next(1, 10);
        switch (missionLevel)
        {
            case MissionLevel.Low:
                while (FirebaseManager.gameData.players[playerNumber].playerMission.low.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                FirebaseManager.gameData.players[playerNumber].playerMission.low.missionNum = tmp;
                break;
            case MissionLevel.Mid:
                while (FirebaseManager.gameData.players[playerNumber].playerMission.mid.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                FirebaseManager.gameData.players[playerNumber].playerMission.mid.missionNum = tmp;
                break;
            case MissionLevel.High:
                while (FirebaseManager.gameData.players[playerNumber].playerMission.high.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                FirebaseManager.gameData.players[playerNumber].playerMission.high.missionNum = tmp;
                break;
            default: break;
        }
    }
    #endregion

    #region Toggle Panels
    void ToggleSuggestPanel(bool b) => SuggestPanel.SetActive(b);

    void ToggleGetPanel(bool b) => GetPanel.SetActive(b);

    void ToggleLoadingPanel(bool b) => LoadingPanel.SetActive(b);

    void ToggleMissionSetPanel(bool b) => MissionSetPanel.SetActive(b);

    void ToggleResultPanel(bool b) => ResultPanel.SetActive(b);

    void ToggleWaitingPanel(bool b) => WaitingPanel.SetActive(b);

    void ToggleMissionCheckPanel(bool b) => MissionCheckPanel.SetActive(b);

    void ToggleQuitBtnPanel(bool b) => QuitBtnPanel.SetActive(b);
    #endregion

    #region Test Toggle Panel
    public void ToggleGetBtn()
    {
        if (GetPanel.activeSelf == true) ToggleGetPanel(false);
        else
        {
            InitGetPanel();
            ToggleGetPanel(true);
        }
    }

    public void ToggleSuggestBtn()
    {
        if (SuggestPanel.activeSelf == true) ToggleSuggestPanel(false);
        else
        {
            SuggestGoldInputField.text = "00";
            ToggleSuggestPanel(true);
        }
    }

    public void ToggleLoadingBtn()
    {
        if (LoadingPanel.activeSelf == true) ToggleLoadingPanel(false);
        else ToggleLoadingPanel(true);
    }

    public void ToggleMissionBtn()
    {
        if (MissionSetPanel.activeSelf == true) ToggleMissionSetPanel(false);
        else
        {
            InitMissionSet();
            ToggleMissionSetPanel(true);
        }
    }

    public void ToggleResultBtn()
    {
        if (ResultPanel.activeSelf == true) ToggleResultPanel(false);
        else ToggleResultPanel(true);
    }

    public void ToggleWaitingBtn()
    {
        if (WaitingPanel.activeSelf == true) ToggleWaitingPanel(false);
        else ToggleWaitingPanel(true);
    }
    #endregion

    #region Suggest Panel
    [SerializeField] Button SuggestBtn;
    [SerializeField] TMP_InputField SuggestGoldInputField;
    [SerializeField] GameObject SuggestConfirmPanel;
    [SerializeField] TMP_Text SuggestConfirmTMP;

    void ToggleSuggestConfirmPanel(bool b) => SuggestConfirmPanel.SetActive(b);

    public void PressSuggestBtn()
    {
        ToggleSuggestConfirmPanel(true);
    }

    void CheckGoldAmount()
    {
        if(SuggestGoldInputField.text.Length > 2)
        {
            if (SuggestGoldInputField.text != "100")
            {
                string tmp = SuggestGoldInputField.text.Substring(SuggestGoldInputField.text.Length - 2);
                SuggestGoldInputField.text = tmp;
            }
        }
    }

    public void ConfirmSuggestYesBtn()
    {
        SuggestGoldInputField.interactable = false;
        SuggestBtn.interactable = false;
        ToggleSuggestConfirmPanel(false);
    }

    public void ConfirmSuggestNoBtn()
    {
        ToggleSuggestConfirmPanel(false);
    }

    void SuggestTurnTimeEnd()
    {

    }
    #endregion

    #region GetPanel
    [SerializeField] TMP_Text GetGoldTMP;

    [SerializeField] GameObject GetConfirmPanel;
    [SerializeField] TMP_Text GetConfirmTMP;

    [SerializeField] GameObject OutConfirmPanel;
    [SerializeField] TMP_Text OutConfirmTMP;

    [SerializeField] Button GetButton;
    [SerializeField] Button OutButton;

    void InitGetPanel()
    {
        GetButton.interactable = true;
        OutButton.interactable = true;
        SetGetGoldTMP();
    }

    void SetGetGoldTMP()    // initial
    {
        GetGoldTMP.text = "40"; // need to change
        GetGoldTMP.color = Color.black;
    }

    public void GetBtn()
    {   
        GetConfirmPanel.SetActive(true);
    }

    public void OutBtn()
    {
        OutConfirmPanel.SetActive(true);
    }

    public void GetConfirmYesBtn()
    {
        // Get RPC Function
        // FirebaseManager.MyPlayer.GetOutGold(1, true);
        // 창 닫기
        GetConfirmPanel.SetActive(false);
        // 글자 색상 변경
        GetGoldTMP.color = Color.blue;

        GetButton.interactable = false;
        OutButton.interactable = false;
    }

    public void GetConfirmNoBtn()
    {
        GetConfirmPanel.SetActive(false);
    }

    public void OutConfirmYesBtn()
    {
        // Out RPC Function
        // FirebaseManager.MyPlayer.GetOutGold(1, false);
        // 창 닫기
        OutConfirmPanel.SetActive(false);
        // 글자 색상 변경
        GetGoldTMP.color = Color.red;

        GetButton.interactable = false;
        OutButton.interactable = false;
    }

    public void OutConfirmNoBtn()
    {
        OutConfirmPanel.SetActive(false);
    }
    #endregion

    #region LoadingPanel

    #endregion

    #region Mission Set Panel
    [SerializeField] Button[] RerollBtns;
    [SerializeField] TMP_Text[] MissionInfoTMPs;
    [SerializeField] TMP_Text[] MissionGoldTMPs;

    void InitMissionSet()
    {
        for(int i = 0; i < 3; i++)
        {
            RerollBtns[i].interactable = true;
        }

        // Mission Low, Mid, High Update


        MissionSetTextUpdate();
    }
    
    void MissionSetTextUpdate()
    {
        for(int i = 0; i < 3; i++)
        {
            MissionInfoTMPs[i].text = "This is Example of Missions Information.\nL2";
            MissionGoldTMPs[i].text = "50" + "G";
        }
    }

    public void MissionReroll(int i)
    {
        // Mission Num Change
        RerollBtns[i].interactable = false;
    }

    public void MissionFix()
    {
        for(int i = 0;i < 3;i++)
        {
            RerollBtns[i].interactable = false;
        }
    }


    #endregion

    #region Result Panel
    public void BackToRoom()
    {
        // if random match or custom match
        ChangeToScene(3);
    }

    public void BackToLobby()
    {
        photonManager.LeaveRoom();
        ChangeToScene(2);
    }
    #endregion

    #region Waiting Panel

    #endregion

    #region Mission Check Panel
    [SerializeField] TMP_Text[] MissionGold;
    [SerializeField] TMP_Text[] MissionInformation;
    [SerializeField] GameObject AllMissionsPanel;

    public void MissionCheckBtn() => ToggleMissionCheckPanel(true);

    public void CloseMissionCheck() => ToggleMissionCheckPanel(false);

    void ToggleAllMissionsPanel(bool b) => AllMissionsPanel.SetActive(b);
    public void LookAllMissionsBtn() => ToggleAllMissionsPanel(true);

    public void CloseAllMissionsBtn() => ToggleAllMissionsPanel(false);

    void SetMissionCheckData()
    {
        MissionGold[0].text = "50" + "G";   // 숫자 변경 필요
        MissionInformation[0].text = "Information of Mission 1\nTest Text";

        MissionGold[1].text = "150" + "G";   // 숫자 변경 필요
        MissionInformation[1].text = "Information of Mission 2\nTest Text";

        MissionGold[2].text = "270" + "G";   // 숫자 변경 필요
        MissionInformation[2].text = "Information of Mission 3\nTest Text";
    }

    #endregion

    #region Quit Panel
    public void QuitBtn() => ToggleQuitBtnPanel(true);

    public void CancelQuitBtn() => ToggleQuitBtnPanel(false);

    public void ConfirmQuitBtn()
    {
        photonManager.LeaveRoom();
        ChangeToScene(2);
        // AI 대체 추가
    }

    #endregion

    #region Game
    public void ProposeGold(int gold)
    {

    }
    #endregion
}