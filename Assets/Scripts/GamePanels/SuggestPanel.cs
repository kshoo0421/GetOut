using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuggestPanel : MonoBehaviour
{
    #region Field

    [SerializeField] private Button SuggestBtn;
    [SerializeField] private TMP_InputField SuggestGoldInputField;
    [SerializeField] private GameObject SuggestConfirmPanel;
    [SerializeField] private TMP_Text SuggestConfirmTMP;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        SuggestGoldInputField.text = "00";
        SuggestBtn.interactable = true;
        SuggestGoldInputField.interactable = true;
    }

    private void Update()
    {
        CheckGoldAmount();
    }

    #endregion

    #region Functions
    private void ToggleSuggestConfirmPanel(bool b) => SuggestConfirmPanel.SetActive(b);

    public void OpenSuggestConfirmPanel()
    {
        DatabaseManager.goldAmount = int.Parse(SuggestGoldInputField.text);
        ToggleSuggestConfirmPanel(true);
        SuggestConfirmTMP.text = $"Do you really want to suggest {DatabaseManager.goldAmount} Gold?\n" +
            $"(Opponent : {100 - DatabaseManager.goldAmount} Gold)";
    }

    private void CheckGoldAmount()
    {
        if (SuggestGoldInputField.text.Length > 2)
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
    #endregion
}
