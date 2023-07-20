using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GetPanel : MonoBehaviour
{
    #region Field
    private GamePhase curGamePhase;
    private GameData gd;    // game data
    private GamePlayer mp;  // my player

    [SerializeField] private TMP_Text MyGoldTMP;
    [SerializeField] private TMP_Text OpponentGoldTMP;

    [SerializeField] private GameObject GetConfirmPanel;
    [SerializeField] private TMP_Text GetConfirmTMP;

    [SerializeField] private GameObject OutConfirmPanel;
    [SerializeField] private TMP_Text OutConfirmTMP;

    [SerializeField] private Button GetButton;
    [SerializeField] private Button OutButton;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        InitGetPanel();
        gd = DatabaseManager.gameData;
        mp = DatabaseManager.MyPlayer;
    }

    private void OnDisable()
    {
        GetConfirmPanel.SetActive(false);
        OutConfirmPanel.SetActive(false);
    }
    #endregion

    #region Functions
    private void InitGetPanel()
    {
        GetButton.interactable = true;
        OutButton.interactable = true;
        DatabaseManager.isGet = true;
        SetGetGoldTMP();
    }

    private void SetGetGoldTMP()    // initial
    {
        DatabaseManager.goldAmount = (int)DatabaseManager.gameData.turnData[DatabaseManager.curTurn].gold[DatabaseManager.MyPlayer.playerNum];
        MyGoldTMP.color = Color.black;
        MyGoldTMP.text = DatabaseManager.goldAmount.ToString();
        OpponentGoldTMP.color = Color.black;
        OpponentGoldTMP.text = (100 - DatabaseManager.goldAmount).ToString();
    }

    public void GetBtn()
    {
        GetConfirmPanel.SetActive(true);
        
        if(OptionManager.curLocale == 0)
        {
            GetConfirmTMP.text = $"Do you really get {DatabaseManager.goldAmount} gold?" +
                $"\n(opponent : {100 - DatabaseManager.goldAmount} gold)";
        }
        else
        {
            GetConfirmTMP.text = $"정말 {DatabaseManager.goldAmount} 골드를 획득하겠습니까??" +
                $"\n(상대 : {100 - DatabaseManager.goldAmount} 골드)";

        }
    }

    public void OutBtn()
    {
        OutConfirmPanel.SetActive(true);
        if(OptionManager.curLocale == 0)
        {
            OutConfirmTMP.text = $"Do you really refuse {DatabaseManager.goldAmount} gold?" +
                $"\n(opponent : {100 - DatabaseManager.goldAmount} gold)";
        }
        else
        {
            OutConfirmTMP.text = $"정말 {DatabaseManager.goldAmount} 골드를 거절하시겠습니까?" +
                $"\n(상대 : {100 - DatabaseManager.goldAmount} 골드)";
        }
    }

    public void GetConfirmYesBtn()
    {
        DatabaseManager.isGet = true;
        GetConfirmPanel.SetActive(false);
        MyGoldTMP.color = Color.blue;
        OpponentGoldTMP.color= Color.blue;

        GetButton.interactable = false;
        OutButton.interactable = false;
    }

    public void GetConfirmNoBtn()
    {
        GetConfirmPanel.SetActive(false);
    }

    public void OutConfirmYesBtn()
    {
        DatabaseManager.isGet = false;
        OutConfirmPanel.SetActive(false);
        MyGoldTMP.color = Color.red;
        OpponentGoldTMP.color = Color.red;

        GetButton.interactable = false;
        OutButton.interactable = false;
    }

    public void OutConfirmNoBtn()
    {
        OutConfirmPanel.SetActive(false);
    }
    #endregion
}