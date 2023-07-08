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

    /* Panels */
    [SerializeField] private GameObject SuggestPanel;
    [SerializeField] private GameObject GetPanel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject MissionSetPanel;
    [SerializeField] private GameObject ResultPanel;
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
        photonManager.SpawnPlayerPrefab();
        gd = DatabaseManager.gameData;
        curGamePhase = GamePhase.Default;
        DatabaseManager.gamePhase = GamePhase.InitGame;
        MyPlayer = DatabaseManager.MyPlayer;
        MissionCheckBtn.interactable = false;
        if (MyPlayer.isMasterClient) databaseManager.SaveGameData();
    }

    private void Update()
    {
        if(test_gp != curGamePhase)
        {
            test_gp = curGamePhase;
            Debug.Log($"turn {DatabaseManager.curTurn + 1}, cur GamePhase : {test_gp}");
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

    #region Update for Game Scene
    private void UpdateText()
    {
        TurnTextUpdate();
        TimeTextUpdate();
        CurGoldUpdate();
    }

    private void TurnTextUpdate() => TurnNumText.text = (DatabaseManager.curTurn + 1).ToString();
    private void TimeTextUpdate() => LeftTime.text = DatabaseManager.leftTime;
    private void CurGoldUpdate() => CurGold.text = DatabaseManager.curGold.ToString();

    private void UpdatePanel()
    {
        if (curGamePhase != DatabaseManager.gamePhase)
        {
            curGamePhase = DatabaseManager.gamePhase;
            AllPanelOff();
            OpenSelectedPanel(curGamePhase);

            if (MyPlayer.isMasterClient)
            {
                NextPhaseFunction(curGamePhase);
            }
        }
    }

    private void CheckGamePhase()
    {
        if (curGamePhase == GamePhase.GetPhase || curGamePhase == GamePhase.SetMission ||
            curGamePhase == GamePhase.WaitingGetPhase || curGamePhase == GamePhase.SuggestPhase
            || curGamePhase == GamePhase.WaitingSuggestPhase)
        {
            UpdateTime();
        }
    }
    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene(3);

    public void BackToRandom() => ChangeToScene(4);
    #endregion

    #region Toggle Panels
    private void ToggleSuggestPanel(bool b) => SuggestPanel.SetActive(b);

    private void ToggleGetPanel(bool b) => GetPanel.SetActive(b);

    private void ToggleLoadingPanel(bool b) => LoadingPanel.SetActive(b);

    private void ToggleMissionSetPanel(bool b) => MissionSetPanel.SetActive(b);

    private void ToggleResultPanel(bool b) => ResultPanel.SetActive(b);

    private void ToggleWaitingPanel(bool b) => WaitingPanel.SetActive(b);

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
                DatabaseManager.isGet = true;
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

    private void AllPanelOff()
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
    public void ToggleGetBtn() => ToggleGetPanel(!GetPanel.activeSelf);
    public void ToggleSuggestBtn() => ToggleSuggestPanel(!SuggestPanel.activeSelf);
    public void ToggleLoadingBtn() => ToggleLoadingPanel(!LoadingPanel.activeSelf);
    public void ToggleMissionBtn() => ToggleMissionSetPanel(!MissionSetPanel.activeSelf);
    public void ToggleResultBtn() => ToggleResultPanel(!ResultPanel.activeSelf);
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

    private void UpdateTurn()
    {
        if (DatabaseManager.curTurn < 5)
        {
            MyPlayer.SynchronizeTurn(DatabaseManager.curTurn + 1);
            MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
        }
        else
        {
            MyPlayer.SetGamePhase(GamePhase.ResultPhase);
        }
    }
    #endregion

    #region Phase Start
    private void NextPhaseFunction(GamePhase curGamePhase)
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
                MissionSetPhaseBehaviour();
                break;

            case GamePhase.SuggestPhase:
                SuggestPhaseBehaviour();
                break;

            case GamePhase.WaitingSuggestPhase:
                WaitingSuggestPhaseBehaviour();
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

            default:
                Debug.Log("Error");
                break;
        }
    }

    private void DefaultPhaseBehaviour() => MyPlayer.SetGamePhase(GamePhase.InitGame);

    private void InitPhaseBehaviour() => MyPlayer.SetGame();

    private void MissionSetPhaseBehaviour()
    {
        // setNewTime(30);
        setNewTime(10); // develop mode
    }

    private void SuggestPhaseBehaviour()
    {
        // setNewTime(20);
        setNewTime(10); // develop mode
    }
    private void WaitingSuggestPhaseBehaviour()
    {
        //setNewTime(20);
        setNewTime(10); // develop mode
    }

    private void GetPhaseBehaviour()
    {
        setNewTime(10);
    }

    private void LoadingPhaseBehaviour()
    {
        databaseManager.SaveTurnData(DatabaseManager.curTurn);
        int tmp1 = -1, tmp2 = -1;
        CheckWhoIsSuggestor(ref tmp1, ref tmp2);
        Debug.Log($"Who is Suggestor? tmp1 : {tmp1}, tmp2 : {tmp2}");
        MyPlayer.SetSuggestPhase(tmp1, tmp2);
    }

    private void ResultPhaseBehaviour()
    {

    }

    private void WaitingGetPhaseBehaviour()
    {
        setNewTime(10);
    }
    #endregion

    #region Phase End
    void TimeOut()
    {
        Debug.Log("Time Out");
        DatabaseManager.leftTime = "00";
        int tmp1 = -1, tmp2 = -1;
        switch (curGamePhase)
        {
            case GamePhase.SetMission:
                MissionCheckBtn.interactable = true;
                MyPlayer.SavePlayerMissionData();
                MyPlayer.SynchronizeTurn(0);
                MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
                break;
        
            case GamePhase.SuggestPhase:
            case GamePhase.WaitingSuggestPhase:
                CheckWhoIsSuggestor(ref tmp1, ref tmp2);
                Debug.Log($"Who is Suggestor? tmp1 : {tmp1}, tmp2 : {tmp2}");
                MyPlayer.SuggestGold(tmp1, tmp2);
                CheckWhoIsGetter(ref tmp1, ref tmp2);
                Debug.Log($"Who is Getter? tmp1 : {tmp1}, tmp2 : {tmp2}");
                MyPlayer.SetGetPhase(tmp1, tmp2);
                break;

            case GamePhase.GetPhase:
            case GamePhase.WaitingGetPhase:
                CheckWhoIsGetter(ref tmp1, ref tmp2);
                Debug.Log($"Who is Getter? tmp1 : {tmp1}, tmp2 : {tmp2}");
                MyPlayer.GetOutGold(tmp1, tmp2);
                UpdateTurn();
                MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
                break;

            case GamePhase.LoadingPhase:
                MyPlayer.SetGamePhase(GamePhase.LoadingPhase);
                break;
        }
    }
    #endregion
}