using TMPro;
using UnityEngine;

public class WaitingPanel : MonoBehaviour
{
    [SerializeField] TMP_Text WaitingTMP;

    void OnEnable() 
    {
        if(DatabaseManager.gamePhase == GamePhase.WaitingSuggest)
        {
            SetWaitingSuggestText();
        }
        else if(DatabaseManager.gamePhase == GamePhase.WaitingGet)
        {
            SetWaitingGetText();
        }
    }

    private void SetWaitingSuggestText()
    {
        if(OptionManager.curLocale == 0)
        {
            WaitingTMP.text = "Waiting For Opponent's Suggestion.";
        }
        else
        {
            WaitingTMP.text = "상대의 제안을 기다리고 있습니다.";
        }
    }

    private void SetWaitingGetText()
    {
        if (OptionManager.curLocale == 0)
        {
            WaitingTMP.text = "Waiting For Opponent's Get/Out Choice.";
        }
        else
        {
            WaitingTMP.text = "상대의 수락/거절 결정을 기다리고 있습니다.";
        }
    }
}
