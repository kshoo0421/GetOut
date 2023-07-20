using TMPro;
using UnityEngine;

public class StartTurnPanel : MonoBehaviour
{
    [SerializeField] TMP_Text StartTMP;
    [SerializeField] TMP_Text InfoTMP;

    private GameData gd;
    private int curTurn;
    private int playerNum;

    private void OnEnable()
    {
        InitData();
        if(OptionManager.curLocale == 0)
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
        curTurn = DatabaseManager.curTurn;
        curTurn += 1;
        gd = DatabaseManager.gameData;
        playerNum = DatabaseManager.MyPlayer.playerNum;
        if (DatabaseManager.MyPlayer.isMasterClient)
        {
            DatabaseManager.MyPlayer.SynchronizeTurn(curTurn);
        }
    }

    private void SetEnText()
    {
        StartTMP.text = $"Turn {curTurn + 1} will be started.\nTurn ({curTurn + 1}/6)";
        if (gd.turnData[curTurn].isSuggestor[playerNum])
        {
            switch(DatabaseManager.suggestCount)
            {
                case 1:
                    InfoTMP.text = "First ";
                    break;
                case 2:
                    InfoTMP.text = "Second ";
                    break;
                case 3:
                    InfoTMP.text = "Last ";
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            InfoTMP.text += $"Suggest Turn\n(Suggest : {DatabaseManager.suggestCount}/3)";
        }
        else
        {
            switch (DatabaseManager.getCount)
            {
                case 1:
                    InfoTMP.text = "First ";
                    break;
                case 2:
                    InfoTMP.text = "Second ";
                    break;
                case 3:
                    InfoTMP.text = "Last ";
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            InfoTMP.text += $"Get Turn\n(Get : {DatabaseManager.getCount}/3)";
        }
    }

    private void SetKoText()
    {
        StartTMP.text = $"�� {curTurn + 1}�� ���۵˴ϴ�.\n�� ({curTurn + 1}/6)";
        if (gd.turnData[curTurn].isSuggestor[playerNum])
        {
            switch (DatabaseManager.suggestCount)
            {
                case 1:
                    InfoTMP.text = "ù ��° ";
                    break;
                case 2:
                    InfoTMP.text = "�� ��° ";
                    break;
                case 3:
                    InfoTMP.text = "������ ";
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            InfoTMP.text += $"���� ����\n(���� : {DatabaseManager.suggestCount}/3)";
        }
        else
        {
            switch (DatabaseManager.getCount)
            {
                case 1:
                    InfoTMP.text = "ù ��° ";
                    break;
                case 2:
                    InfoTMP.text = "�� ��° ";
                    break;
                case 3:
                    InfoTMP.text = "������ ";
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            InfoTMP.text += $"���� ����\n(���� : {DatabaseManager.getCount}/3)";
        }
    }
}
