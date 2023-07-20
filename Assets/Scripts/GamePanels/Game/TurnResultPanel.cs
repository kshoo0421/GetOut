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
        InfoText.text = $"턴 {curTurn + 1} 결과";
        MyGold.text = myGold.ToString();
        OpponentGold.text = (100 - myGold).ToString();
        if (gd.turnData[curTurn].success[player])
        {
            ResultTMP.text = "수락";
            MyGold.color = Color.blue;
            OpponentGold.color = Color.blue;
        }
        else
        {
            ResultTMP.text = "거절";
            MyGold.color = Color.red;
            OpponentGold.color = Color.red;
        }
    }

}
