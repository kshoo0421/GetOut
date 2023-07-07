using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GetPanel : MonoBehaviour
{
    #region Field
    private GamePhase curGamePhase;
    private GameData gd;    // game data
    private GamePlayer mp;  // my player

    [SerializeField] private TMP_Text GetGoldTMP;

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
    #endregion

    #region Functions
    private void InitGetPanel()
    {
        GetButton.interactable = true;
        OutButton.interactable = true;
        SetGetGoldTMP();
    }

    private void SetGetGoldTMP()    // initial
    {
        DatabaseManager.goldAmount = (int)DatabaseManager.gameData.turnData[DatabaseManager.curTurn].gold[DatabaseManager.MyPlayer.playerNum];
        GetGoldTMP.color = Color.black;
        GetGoldTMP.text = DatabaseManager.goldAmount.ToString();
    }

    public void GetBtn()
    {
        GetConfirmPanel.SetActive(true);
        GetConfirmTMP.text = $"Do you really get {DatabaseManager.goldAmount} gold?" +
            $"\n(opponent : {100 - DatabaseManager.goldAmount} gold)";
    }

    public void OutBtn()
    {
        OutConfirmPanel.SetActive(true);
        OutConfirmTMP.text = $"Do you really refuse {DatabaseManager.goldAmount} gold?" +
            $"\n(opponent : {100 - DatabaseManager.goldAmount} gold)";
    }

    public void GetConfirmYesBtn()
    {
        DatabaseManager.isGet = true;
        GetConfirmPanel.SetActive(false);
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
        DatabaseManager.isGet = false;
        OutConfirmPanel.SetActive(false);
        GetGoldTMP.color = Color.red;

        GetButton.interactable = false;
        OutButton.interactable = false;
    }

    public void OutConfirmNoBtn()
    {
        OutConfirmPanel.SetActive(false);
    }
    #endregion
}