using TMPro;
using UnityEngine;

public class TurnResultPanel : MonoBehaviour
{
    [SerializeField] TMP_Text InfoText;
    [SerializeField] TMP_Text MyGold;
    [SerializeField] TMP_Text OpponentGold;
    [SerializeField] TMP_Text ResultTMP;

    private GameData gd;
    private int curTurn;
    private int player;
    private int myGold;
    private MissionSupporter missionSupporter;

    private void OnEnable()
    {
        InitData();
        if (OptionManager.curLocale == 0)
        {
            SetEnText();
        }
        else
        {
            SetKoText();
        }

        if(DatabaseManager.curTurn == 5)
        {
            missionSupporter = MissionSupporter.Instance;
            SetMissionResult();
        }
    }

    private void InitData()
    {
        gd = DatabaseManager.gameData;
        curTurn = DatabaseManager.curTurn;
        player = DatabaseManager.MyPlayer.playerNum;
        myGold = (int)gd.turnData[curTurn].gold[player];
    }

    private void SetEnText()
    {
        InfoText.text = $"Turn {curTurn + 1} Result";
        MyGold.text = myGold.ToString();
        OpponentGold.text = (100 - myGold).ToString();
        if(gd.turnData[curTurn].success[player])
        {
            ResultTMP.text = "Get";
            MyGold.color = Color.blue;
            OpponentGold.color = Color.blue;
        }
        else
        {
            ResultTMP.text = "Out";
            MyGold.color = Color.red;
            OpponentGold.color = Color.red;
        }
    }

    private void SetKoText()
    {
        InfoText.text = $"�� {curTurn + 1} ���";
        MyGold.text = myGold.ToString();
        OpponentGold.text = (100 - myGold).ToString();
        if (gd.turnData[curTurn].success[player])
        {
            ResultTMP.text = "����";
            MyGold.color = Color.blue;
            OpponentGold.color = Color.blue;
        }
        else
        {
            ResultTMP.text = "����";
            MyGold.color = Color.red;
            OpponentGold.color = Color.red;
        }
    }

    private void SetMissionResult()
    {
        for (int i = 0; i < 4; i++)
        {
            missionSupporter.CheckMissionDone(MissionLevel.Low, gd.playerMissionData[i].low.missionNum, i);
            missionSupporter.CheckMissionDone(MissionLevel.Mid, gd.playerMissionData[i].mid.missionNum, i);
            missionSupporter.CheckMissionDone(MissionLevel.High, gd.playerMissionData[i].high.missionNum, i);
        }
    }
}
