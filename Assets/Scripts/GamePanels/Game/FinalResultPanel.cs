using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalResultPanel : MonoBehaviour
{
    #region Fields
    private GameData gd;
    private MissionSupporter ms;
    private long[] sum;
    private PhotonManager photonManager;

    // - 0 : name / 1 ~ 6 : turn 1 ~ 6 / 7 ~ 9 : mission 1 ~ 3 / 10 : sum / 11 : rank
    [SerializeField] TMP_Text[] player1;
    [SerializeField] TMP_Text[] player2;
    [SerializeField] TMP_Text[] player3;
    [SerializeField] TMP_Text[] player4;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        gd = DatabaseManager.gameData;
        ms = MissionSupporter.Instance;
        photonManager = PhotonManager.Instance;
        sum = new long[4] { 0, 0, 0, 0 };
        SetResultPanel();
    }
    #endregion

    #region Functions
    public void SetGameData(GameData gamedata) => gd = gamedata;

    private void SetResultPanel()
    {
        for(int i = 0; i < 4; i++)
        {
            SetPlayerData(i);
        }
        // SetRankText();
    }

    private void SetPlayerData(int playerNum)
    {
        long sum = 0;
        TMP_Text[] temp_text = SetTempTMP(playerNum);
        SetPlayerMameText(ref temp_text, playerNum);
        SetTurnText(ref temp_text, playerNum, ref sum);
        SetMissionResult();
        SetMissionText(ref temp_text, playerNum, ref sum);
        SetSumText(ref temp_text, playerNum, ref sum);
    }

    private void SetMissionResult()
    {
        for(int i = 0; i < 4; i++)
        {
            ms.CheckMissionDone(MissionLevel.Low, gd.playerMissionData[i].low.missionNum, i);
            ms.CheckMissionDone(MissionLevel.Mid, gd.playerMissionData[i].mid.missionNum, i);
            ms.CheckMissionDone(MissionLevel.High, gd.playerMissionData[i].high.missionNum, i);
        }
    }

    private TMP_Text[] SetTempTMP(int playerNum)
    {
        switch (playerNum)
        {
            case 0: return player1;
            case 1: return player2;
            case 2: return player3; 
            case 3: return player4;
            default:
                Debug.LogError("ResultPanel Set Player Error");
                break;
        }
        return new TMP_Text[11];
    }

    private void SetPlayerMameText(ref TMP_Text[] temp_text, int playerNum) => temp_text[playerNum].text = DatabaseManager.gameData.playerId[playerNum];

    private void SetTurnText(ref TMP_Text[] tmp_text, int playerNum, ref long sum)
    {
        long temp = ms.GetMissionGold(MissionLevel.Low, gd.playerMissionData[playerNum].low.missionNum);

        for (int i = 0; i < 6; i++)
        {
            temp = gd.turnData[i].gold[playerNum];
            tmp_text[i + 1].text = temp.ToString();
            if (gd.turnData[i].success[playerNum])
            {
                tmp_text[i + 1].color = Color.blue;
                sum += temp;
            }
            else
            {
                tmp_text[i + 1].color = Color.red;
            }
        }
    }

    private void SetMissionText(ref TMP_Text[] temp_text, int playerNum, ref long sum)
    {
        SetLowMissionText(ref temp_text, playerNum, ref sum);
        SetMidMissionText(ref temp_text, playerNum, ref sum);
        SetHighMissionText(ref temp_text, playerNum, ref sum);
    }

    private void SetLowMissionText(ref TMP_Text[] temp_text, int playerNum, ref long sum)
    {
        long tmp = ms.GetMissionGold(MissionLevel.Low, gd.playerMissionData[playerNum].low.missionNum);
        temp_text[7].text = tmp.ToString();
        if (gd.playerMissionData[playerNum].low.isAchieved)
        {
            temp_text[7].color = Color.blue;
            sum += tmp;
        }
        else
        {
            temp_text[7].color = Color.red;
        }
    }

    private void SetMidMissionText(ref TMP_Text[] temp_text, int playerNum, ref long sum)
    {
        long tmp = ms.GetMissionGold(MissionLevel.Mid, gd.playerMissionData[playerNum].mid.missionNum);
        temp_text[8].text = tmp.ToString();
        if (gd.playerMissionData[playerNum].mid.isAchieved)
        {
            temp_text[8].color = Color.blue;
            sum += tmp;
        }
        else
        {
            temp_text[8].color = Color.red;
        }
    }

    private void SetHighMissionText(ref TMP_Text[] temp_text, int playerNum, ref long sum)
    {
        long tmp = ms.GetMissionGold(MissionLevel.High, gd.playerMissionData[playerNum].high.missionNum);
        temp_text[9].text = tmp.ToString();
        if (gd.playerMissionData[playerNum].high.isAchieved)
        {
            temp_text[9].color = Color.blue;
            sum += tmp;
        }
        else
        {
            temp_text[9].color = Color.red;
        }
    }
    
    private void SetSumText(ref TMP_Text[] temp_text, int playerNum, ref long sum)
    {
        this.sum[playerNum] = sum;
        temp_text[10].text = sum.ToString();
    }

    private void SetRankText()
    {
        long top = 0;
        int rank = 1;
        bool[] isDone = new bool[4] { false, false, false, false };

        while (rank < 5)
        {
            for (int i = 0; i < 4; i++) // find biggest
            {
                if (top < sum[i] && !isDone[i]) top = sum[i];
            }
            for (int i = 0; i < 4; i++)
            {
                if(top == sum[i])
                {
                    isDone[i] = true;
                    UpdatePlayerRank(i, rank.ToString());
                }
            }
            rank += CountRankedPlayer(ref isDone);
        }
    }

    private int CountRankedPlayer(ref bool[] isDone)
    {
        int count = 0;
        for(int i = 0; i < 4; i++)
        {
            if (isDone[i]) count++;
        }
        return count;
    }

    private void UpdatePlayerRank(int playerNum, string text)
    {
        switch(playerNum)
        {
            case 0:
                player1[11].text = text; 
                break;
            case 1:
                player2[11].text = text;
                break;
            case 2:
                player3[11].text = text;
                break;
            case 3:
                player3[11].text = text;
                break;
            default:
                Debug.Log("Update Player Rank Error");
                break;
        }
    }
    #endregion

    #region Change Scene
    public void BackToRoom()
    {
        // if random match or custom match
        switch (DatabaseManager.randomOrCustom)
        {
            case RandomOrCustom.Default:
                BackToLobby();
                break;

            case RandomOrCustom.Random:
                BackToRandom();
                break;

            case RandomOrCustom.Custom:
                BackToCustom();
                break;

        }
        BackToCustom();
    }

    public void BackToLobby()
    {
        photonManager.LeaveRoom();
        SceneManager.LoadScene(2);
    }

    private void BackToCustom()
    {
        SceneManager.LoadScene(3);
    }

    private void BackToRandom()
    {
        SceneManager.LoadScene(4);
    }
    #endregion
}
