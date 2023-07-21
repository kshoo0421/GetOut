using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S05_Game : Scenes
{
    #region Field
    [SerializeField] private TMP_Text TurnNumText;  // 몇 번째 턴인지
    [SerializeField] private TMP_Text LeftTime;
    [SerializeField] private TMP_Text CurGold;
    [SerializeField] private TMP_Text[] TurnTexts;
    [SerializeField] private TMP_Text[] GoldTexts;

    /* Panels */
    [SerializeField] private GameObject MissionSetPanel;
    [SerializeField] private GameObject StartTurnPanel;
    [SerializeField] private GameObject SuggestPanel;
    [SerializeField] private GameObject GetPanel;
    [SerializeField] private GameObject TurnResultPanel;
    [SerializeField] private GameObject FinalResultPanel;
    [SerializeField] private GameObject WaitingPanel;


    [SerializeField] private Button MissionCheckBtn;
    [SerializeField] private GameObject MissionCheckPanel;
    [SerializeField] private GameObject QuitBtnPanel;

    
    /* Game */
    private string[] playerName = new string[4];
    private GamePhase curGamePhase;
    private GameData gd;
    private GamePlayer MyPlayer;


    /* test */
    private GamePhase test_gp = GamePhase.Default;
    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
        InitGame();
        SetGameInfo();
    }

    private void Update()
    {
        if(test_gp != curGamePhase)
        {
            test_gp = curGamePhase;
        }
        ForUpdate();
        UpdateText();
        UpdatePanel();
        if(MyPlayer.isMasterClient)
        {
            CheckGamePhase();
        }
    }
    #endregion

    #region Init Game
    private void InitGame()
    {
        photonManager.SpawnPlayerPrefab();
        gd = DatabaseManager.gameData;
        curGamePhase = GamePhase.Default;
        DatabaseManager.gamePhase = GamePhase.InitGame;
        MyPlayer = DatabaseManager.MyPlayer;
        MissionCheckBtn.interactable = false;
        //if (MyPlayer.isMasterClient) databaseManager.FirstSaveGameData();
    }
    #endregion

    #region Set Game Info
    private void SetGameInfo()
    {
        for (int i = 0; i < 6; i++)
        {
            if (OptionManager.curLocale == 0)
            {
                TurnTexts[i].text = "T" + (i + 1).ToString();
            }
            else
            {
                TurnTexts[i].text = "턴" + (i + 1).ToString();
            }
            GoldTexts[i].text = "";
        }
        DatabaseManager.suggestCount = 1;
        DatabaseManager.getCount = 1;
    }

    private void UpdateTurnText(int curTurn)
    {
        if (curTurn == 6) return;

        for(int i = 0; i < 6; i++)
        {
            TurnTexts[i].color = Color.black;
        }

        if (gd.turnData[curTurn].isSuggestor[MyPlayer.playerNum])
        {
            TurnTexts[curTurn].text = "S" + DatabaseManager.suggestCount.ToString();
            DatabaseManager.suggestCount++;
        }
        else
        {
            TurnTexts[curTurn].text = "G" + DatabaseManager.getCount.ToString();
            DatabaseManager.getCount++;
        }
        TurnTexts[curTurn].color = Color.yellow;
    }

    private void UpdateGoldText(int curTurn)
    {
        GoldTexts[curTurn].text = gd.turnData[curTurn].gold[MyPlayer.playerNum].ToString();
    }

    private void UpdateGoldColor(int curTurn)
    {
        if (gd.turnData[curTurn].success[MyPlayer.playerNum])
        {
            GoldTexts[curTurn].color = Color.blue;
        }
        else
        {
            GoldTexts[curTurn].color = Color.red;
        }
    }
    #endregion

    #region Update for Game Scene
    private void UpdateText()
    {
        TurnTextUpdate();
        TimeTextUpdate();
    }

    private void TurnTextUpdate() => TurnNumText.text = (DatabaseManager.curTurn + 1).ToString();
    private void TimeTextUpdate() => LeftTime.text = DatabaseManager.leftTime;

    private void UpdatePanel()
    {
        if (curGamePhase != DatabaseManager.gamePhase)
        {
            curGamePhase = DatabaseManager.gamePhase;
            AllPanelOff();
            OpenSelectedPanel(curGamePhase);
            //if (MyPlayer.isMasterClient)
            //{
            NextPhaseFunction(curGamePhase);
            //}
        }
    }

    private void CheckGamePhase()
    {
        if (curGamePhase == GamePhase.SetMission || curGamePhase == GamePhase.StartTurn 
            || curGamePhase == GamePhase.Suggest || curGamePhase == GamePhase.WaitingSuggest
            || curGamePhase == GamePhase.Get || curGamePhase == GamePhase.WaitingGet
            || curGamePhase == GamePhase.TurnResult)
        {
            UpdateTime();
        }
    }
    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene("CustomMatching");

    public void BackToRandom() => ChangeToScene("RandomMatching");
    #endregion

    #region Toggle Panels
    private void ToggleMissionSetPanel(bool b) => MissionSetPanel.SetActive(b);
    private void ToggleStartTurnPanel(bool b) => StartTurnPanel.SetActive(b);
    private void ToggleSuggestPanel(bool b) => SuggestPanel.SetActive(b);
    private void ToggleGetPanel(bool b) => GetPanel.SetActive(b);
    private void ToggleTurnResultPanel(bool b) => TurnResultPanel.SetActive(b);
    private void ToggleWaitingPanel(bool b) => WaitingPanel.SetActive(b);
    private void ToggleFinalResultPanel(bool b) => FinalResultPanel.SetActive(b);

    private void ToggleMissionCheckPanel(bool b) => MissionCheckPanel.SetActive(b);
    private void ToggleQuitPanel(bool b) => QuitBtnPanel.SetActive(b);
    #endregion

    #region Panels Control
    private void OpenSelectedPanel(GamePhase gamePhase)
    {
        switch (gamePhase)
        {
            case GamePhase.Default:
            case GamePhase.InitGame:
                break;

            case GamePhase.SetMission:
                ToggleMissionSetPanel(true);
                break;

            case GamePhase.StartTurn:
                ToggleStartTurnPanel(true); 
                break;

            case GamePhase.TurnResult:
                ToggleTurnResultPanel(true);
                break;


            case GamePhase.Suggest:
                ToggleSuggestPanel(true);
                break;

            case GamePhase.Get:
                DatabaseManager.isGet = true;
                ToggleGetPanel(true);
                break;

            case GamePhase.WaitingGet:
            case GamePhase.WaitingSuggest:
                ToggleWaitingPanel(true);
                break;

            case GamePhase.FinalResult:
                ToggleFinalResultPanel(true);
                break;
        }
    }

    private void AllPanelOff()
    {
        ToggleMissionSetPanel(false);
        ToggleStartTurnPanel(false);
        ToggleSuggestPanel(false);
        ToggleGetPanel(false);
        ToggleWaitingPanel(false);
        ToggleTurnResultPanel(false);
        ToggleFinalResultPanel(false);
    }
    #endregion

    #region Test Toggle Panel
    public void ToggleGetBtn() => ToggleGetPanel(!GetPanel.activeSelf);
    public void ToggleSuggestBtn() => ToggleSuggestPanel(!SuggestPanel.activeSelf);
    public void ToggleLoadingBtn() => ToggleTurnResultPanel(!TurnResultPanel.activeSelf);
    public void ToggleMissionBtn() => ToggleMissionSetPanel(!MissionSetPanel.activeSelf);
    public void ToggleResultBtn() => ToggleFinalResultPanel(!FinalResultPanel.activeSelf);
    public void ToggleWaitingBtn() => ToggleWaitingPanel(!WaitingPanel.activeSelf);
    #endregion

    #region Quit/MissionCheck Panel On/Off
    public void ToggleMissionCheckPanel() => ToggleMissionCheckPanel(true);

    public void CloseMissionCheck() => ToggleMissionCheckPanel(false);

    public void OpenQuitPanel() => ToggleQuitPanel(true);

    public void CloseQuitPanel() => ToggleQuitPanel(false);
    
    public void ConfirmQuitBtn()
    {
        BackToLobby();
        // AI 대체 추가
    }

    private void BackToLobby()
    {
        photonManager.LeaveRoom();
        SceneManager.LoadScene(2);
    }
    #endregion

    #region timer
    private void setNewTime(int sec)    // Phase 변경과 더불어 표현되어야 함.
    {
        DatabaseManager.leftTime = sec.ToString();
        DatabaseManager.gameTime = DateTime.Now.AddSeconds(sec);
    }

    private void UpdateTime()
    {
        TimeSpan diff = DatabaseManager.gameTime - DateTime.Now;
        if (diff.Seconds > 0)
        {
            DatabaseManager.leftTime = diff.Seconds.ToString();
            if (DatabaseManager.leftTime.Length == 2)
            {
                LeftTime.text = DatabaseManager.leftTime;
            }
            else
            {
                LeftTime.text = "0" + DatabaseManager.leftTime;
            }
        }
        else
        {
            TimeOut();
        }
        MyPlayer.SynchronizeLeftTime(LeftTime.text);
    }

    private void CheckWhoIsSuggestor(ref int tmp1, ref int tmp2)
    {
        tmp1 = -1;
        tmp2 = -1;
        for (int i = 0; i < 4; i++)
        {
            if (gd.turnData[DatabaseManager.curTurn].isSuggestor[i])
            {
                if (tmp1 == -1) tmp1 = i;
                else
                {
                    tmp2 = i;
                    break;
                }
            }
        }
    }

    private void CheckWhoIsGetter(ref int tmp1, ref int tmp2)
    {
        tmp1 = -1;
        tmp2 = -1;
        for (int i = 0; i < 4; i++)
        {
            if (!gd.turnData[DatabaseManager.curTurn].isSuggestor[i])
            {
                if (tmp1 == -1) tmp1 = i;
                else
                {
                    tmp2 = i;
                    break;
                }
            }
        }
    }
    #endregion

    #region Phase Start
    private void NextPhaseFunction(GamePhase curGamePhase)
    {
        switch (curGamePhase)
        {
            case GamePhase.Default:
                DefaultBehaviour();
                break;

            case GamePhase.InitGame:
                InitGameBehaviour();
                break;

            case GamePhase.SetMission:
                SetMissionBehaviour();
                break;

            case GamePhase.StartTurn:
                StartTurnBehaviour();
                break;

            case GamePhase.Suggest:
                SuggestBehaviour();
                break;

            case GamePhase.WaitingSuggest:
                WaitingSuggestBehaviour();
                break;

            case GamePhase.Get:
                GetBehaviour();
                break;

            case GamePhase.WaitingGet:
                WaitingGetBehaviour();
                break;

            case GamePhase.TurnResult:
                TurnResultBehaviour();
                break;

            case GamePhase.FinalResult:
                FinalResultBehaviour();
                break;

            default:
                Debug.Log("Error");
                break;
        }
    }

    private void DefaultBehaviour()
    {
        MyPlayer.SetGamePhase(GamePhase.InitGame);
    }

    private void InitGameBehaviour()
    {
        MyPlayer.SetGame();
    }
    private void SetMissionBehaviour()
    {
        // setNewTime(30);
        setNewTime(10); // develop mode
    }
    private void StartTurnBehaviour()
    {
        UpdateTurnText(DatabaseManager.curTurn);
        setNewTime(5);
    }

    private void SuggestBehaviour()
    {
        // setNewTime(20);
        setNewTime(5); // develop mode
    }
    private void WaitingSuggestBehaviour()
    {
        //setNewTime(20);
        setNewTime(5); // develop mode
    }

    private void GetBehaviour()
    {
        UpdateGoldText(DatabaseManager.curTurn);
        // setNewTime(10);
        setNewTime(5);
    }

    private void WaitingGetBehaviour()
    {
        UpdateGoldText(DatabaseManager.curTurn);
        // setNewTime(10);
        setNewTime(5);
    }

    private void TurnResultBehaviour()
    {
        databaseManager.UpdateCurGold();
        CurGold.text = DatabaseManager.curGold.ToString();
        UpdateGoldColor(DatabaseManager.curTurn);
        databaseManager.SaveTurnData(DatabaseManager.curTurn);
        setNewTime(5);
    }

    private void FinalResultBehaviour()
    {
        LeftTime.text = "00";
    }
    #endregion

    #region Phase End
    void TimeOut()
    {
        DatabaseManager.leftTime = "00";
        int tmp1 = -1, tmp2 = -1;
        switch (curGamePhase)
        {
            case GamePhase.SetMission:
                MissionCheckBtn.interactable = true;
                MyPlayer.SavePlayerMissionData();
                MyPlayer.SetGamePhase(GamePhase.StartTurn);
                break;

            case GamePhase.StartTurn:
                CheckWhoIsSuggestor(ref tmp1, ref tmp2);
                MyPlayer.SetSuggestPhase(tmp1, tmp2);
                break;

            case GamePhase.Suggest:
            case GamePhase.WaitingSuggest:
                CheckWhoIsSuggestor(ref tmp1, ref tmp2);
                MyPlayer.SuggestGold(tmp1, tmp2);
                CheckWhoIsGetter(ref tmp1, ref tmp2);
                MyPlayer.SetGetPhase(tmp1, tmp2);
                break;

            case GamePhase.Get:
            case GamePhase.WaitingGet:
                CheckWhoIsGetter(ref tmp1, ref tmp2);
                MyPlayer.GetOutGold(tmp1, tmp2);
                MyPlayer.SetGamePhase(GamePhase.TurnResult);
                break;

            case GamePhase.TurnResult:
                if (DatabaseManager.curTurn < 5)
                {
                    MyPlayer.SetGamePhase(GamePhase.StartTurn);
                }
                else
                {
                    MyPlayer.SetGamePhase(GamePhase.FinalResult);
                }
                break;
        }
    }
    #endregion

    #region Option Panel
    [SerializeField] GameObject OptionPanel;
    public void ToggleOptionPanel() => OptionPanel.SetActive(!OptionPanel.activeSelf);
    #endregion
}