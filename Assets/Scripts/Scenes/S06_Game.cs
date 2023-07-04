using UnityEngine;

using TMPro;

using UnityEngine.UI;
using System;

public class S06_Game : Scenes
{
    #region Field
    [SerializeField] TMP_Text TurnNumText;  // �� ��° ������
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
    string[] playerName = new string[4];
    GamePhase curGamePhase;
    GameData gd;
    GamePlayer MyPlayer;
    bool isGet;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        SetMissionCheckData();
        databaseManager.InitGameData();
        MyPlayer = DatabaseManager.MyPlayer;
        gd = DatabaseManager.gameData;
        curGamePhase = GamePhase.Default;
        Debug.Log($"gd.turnData[0].matchWith[0] : {gd.turnData[0].matchWith[0]}");
    }
    void Update()
    {
        ForUpdate();
        TurnNumText.text = (DatabaseManager.gameData.curTurn + 1).ToString();

        if (curGamePhase != DatabaseManager.gamePhase)
        {
            curGamePhase = DatabaseManager.gamePhase;
            PanelUpdate(curGamePhase);
         
            if (MyPlayer.isPlayerCaptain)
            {
                NextPhaseFunction(curGamePhase);
            }
        }

        if (curGamePhase == GamePhase.GetPhase || curGamePhase == GamePhase.SetMission || 
            curGamePhase == GamePhase.WaitingGetPhase || curGamePhase == GamePhase.SuggestPhase 
            || curGamePhase == GamePhase.WaitingSuggestPhase)
        {
            UpdateTime();
        }

        if (curGamePhase == GamePhase.SuggestPhase)
        {
            CheckGoldAmount();
        }
    }
    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene(3);

    public void BackToRandom() => ChangeToScene(4);
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

    void PanelUpdate(GamePhase gamePhase)
    {
        AllPanelOff();

        switch (gamePhase)
        {
            case GamePhase.Default:
                ToggleLoadingPanel(true);
                break;

            case GamePhase.InitGame:
            case GamePhase.LoadingPhase:
                ToggleLoadingPanel(true);
                break;

            case GamePhase.SetMission:
                ToggleMissionSetPanel(true);
                break;

            case GamePhase.SuggestPhase:
                ToggleSuggestPanel(true);
                break;

            case GamePhase.GetPhase:
                isGet = true;
                ToggleGetPanel(true);
                break;

            case GamePhase.ResultPhase:
                ToggleResultPanel(true);
                break;

            case GamePhase.WaitingGetPhase:
            case GamePhase.WaitingSuggestPhase:
                ToggleWaitingPanel(true);
                break;
        }
    }

    void AllPanelOff()
    {
        ToggleLoadingPanel(false);
        ToggleSuggestPanel(false);
        ToggleGetPanel(false);
        ToggleMissionSetPanel(false);
        ToggleResultPanel(false);
        ToggleWaitingPanel(false);
    }
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
        GetGoldTMP.text = gd.turnData[gd.curTurn].gold[MyPlayer.playerNum].ToString();
        GetGoldTMP.color = Color.black;
    }

    public void GetBtn()
    {   
        GetConfirmPanel.SetActive(true);
        GetConfirmTMP.text = $"Do you really get {GetGoldTMP.text} gold?" +
            $"\n(opponent : {100 - gd.turnData[gd.curTurn].gold[MyPlayer.playerNum]} gold)";
    }

    public void OutBtn()
    {
        OutConfirmPanel.SetActive(true);
    }

    public void GetConfirmYesBtn()
    {
        isGet = true;
        // â �ݱ�
        GetConfirmPanel.SetActive(false);
        // ���� ���� ����
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
        isGet = false;
        // â �ݱ�
        OutConfirmPanel.SetActive(false);
        // ���� ���� ����
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
    
    void MissionSetTextUpdate() // need to change
    {
        MissionInfo mi = new MissionInfo();
        
        int[] missionNum = new int[3];
        missionNum[0] = (int)gd.playerMissionData[MyPlayer.playerNum].low.missionNum;
        missionNum[1] = (int)gd.playerMissionData[MyPlayer.playerNum].mid.missionNum;
        missionNum[2] = (int)gd.playerMissionData[MyPlayer.playerNum].high.missionNum;
        for(int i = 0; i < 3; i++)
        {
            MissionInfoTMPs[0].text = mi.GetMissionInfo((MissionLevel)i, missionNum[i]);
            MissionGoldTMPs[0].text = mi.GetMissionGold((MissionLevel)i, missionNum[i]).ToString() + "G";
        }
    }

    public void MissionReroll(int i)
    {
        // Mission Num Change
        switch(i)
        {
            case 0:
                MyPlayer.SetPlayerMission(MissionLevel.Low);
                break;
            case 1:
                MyPlayer.SetPlayerMission(MissionLevel.Mid);
                break;
            case 2:
                MyPlayer.SetPlayerMission(MissionLevel.High);
                break;
        }
        RerollBtns[i].interactable = false;
        MyPlayer.SavePlayerMissionData();
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
        MissionGold[0].text = "50" + "G";   // ���� ���� �ʿ�
        MissionInformation[0].text = "Information of Mission 1\nTest Text";

        MissionGold[1].text = "150" + "G";   // ���� ���� �ʿ�
        MissionInformation[1].text = "Information of Mission 2\nTest Text";

        MissionGold[2].text = "270" + "G";   // ���� ���� �ʿ�
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
        // AI ��ü �߰�
    }

    #endregion

    #region timer
    void setNewTime(int sec)    // Phase ����� ���Ҿ� ǥ���Ǿ�� ��.
    {
        DatabaseManager.leftTime = sec.ToString();
        DatabaseManager.gameTime = DateTime.Now.AddSeconds(sec);
    }

    void UpdateTime()
    {
        TimeSpan diff = DatabaseManager.gameTime - DateTime.Now;
        if (diff.Seconds > 0) DatabaseManager.leftTime = diff.Seconds.ToString();
        else
        {
            DatabaseManager.leftTime = "0";
            switch(curGamePhase)
            {
                case GamePhase.SetMission:
                    MyPlayer.SavePlayerMissionData();
                    gd.curTurn = 0;
                    MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
                    break;

                case GamePhase.SuggestPhase:
                case GamePhase.WaitingSuggestPhase:
                    if (curGamePhase == GamePhase.SuggestPhase)
                    {
                        MyPlayer.SuggestGold((int)gd.turnData[gd.curTurn].matchWith[MyPlayer.playerNum], Int32.Parse(SuggestGoldInputField.text));
                    }

                    if (MyPlayer.isPlayerCaptain)
                    {
                        int tmp1 = -1, tmp2 = -1;
                        for (int i = 0; i < 4; i++)
                        {
                            if (!gd.turnData[gd.curTurn].isProposer[i])
                            {
                                if (tmp1 == -1) tmp1 = i;
                                else
                                {
                                    tmp2 = i;
                                    break;
                                }
                            }
                        }
                        MyPlayer.SetGetPhase(tmp1, tmp2);
                    }
                    break;

                case GamePhase.GetPhase:
                case GamePhase.WaitingGetPhase:
                    if(curGamePhase == GamePhase.GetPhase) 
                    {
                        MyPlayer.GetOutGold((int)gd.turnData[gd.curTurn].matchWith[MyPlayer.playerNum], isGet);
                    }

                    if (MyPlayer.isPlayerCaptain)
                    {
                        if (gd.curTurn < 5)
                        {
                            gd.curTurn++;
                            MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
                        }
                        else
                        {
                            MyPlayer.SetGamePhase(GamePhase.ResultPhase);
                        }
                    }
                    break;
            }
            MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
        }
    }
    #endregion

    #region Game
    void NextPhaseFunction(GamePhase curGamePhase)
    {
        switch (curGamePhase)
        {
            case GamePhase.Default:
                DefaultPhaseBehaviour();
                break;

            case GamePhase.InitGame:
                InitPhaseBehaviour();
                break;
            case GamePhase.LoadingPhase:
                LoadingPhaseBehaviour();
                break;

            case GamePhase.SetMission:
                SetMissionPhaseBehaviour();
                break;

            case GamePhase.SuggestPhase:
                SuggestPhaseBehaviour();
                break;

            case GamePhase.GetPhase:
                GetPhaseBehaviour();
                break;

            case GamePhase.ResultPhase:
                ResultPhaseBehaviour();
                break;

            case GamePhase.WaitingGetPhase:
                WaitingGetPhaseBehaviour();
                break;

            case GamePhase.WaitingSuggestPhase:
                WaitingSuggestPhaseBehaviour();
                break;
        }
    }

    void DefaultPhaseBehaviour() => MyPlayer.SetGamePhase(GamePhase.InitGame);

    void InitPhaseBehaviour() => MyPlayer.SetGame();

    void SetMissionPhaseBehaviour()
    {
        InitMissionSet();
        setNewTime(30);
    }

    void SuggestPhaseBehaviour()
    {
        setNewTime(20);
    }

    void GetPhaseBehaviour()
    {
        setNewTime(10);
    }

    void LoadingPhaseBehaviour()
    {
        databaseManager.SaveTurnData((int)gd.curTurn);
        int tmp1 = -1, tmp2 = -1;
        for(int i = 0; i < 4; i++)
        {
            if (gd.turnData[gd.curTurn].isProposer[i])
            {
                if (tmp1 == -1) tmp1 = i;
                else
                {
                    tmp2 = i;
                    break;
                }
            }
        }
        MyPlayer.SetSuggestPhase(tmp1, tmp2);
    }

    void ResultPhaseBehaviour()
    {

    }

    void WaitingGetPhaseBehaviour()
    {

    }

    void WaitingSuggestPhaseBehaviour()
    {

    }
    #endregion
}