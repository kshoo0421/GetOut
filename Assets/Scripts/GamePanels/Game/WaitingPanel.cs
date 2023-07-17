using TMPro;
using UnityEngine;

public class WaitingPanel : MonoBehaviour
{
    [SerializeField] TMP_Text WaitingTMP;

    void OnEnable() 
    {
        if(DatabaseManager.gamePhase == GamePhase.WaitingSuggest)
        {
            WaitingTMP.text = "Waiting For Opponent's Suggestion.";
        }
        else if(DatabaseManager.gamePhase == GamePhase.WaitingGet)
        {
            WaitingTMP.text = "Waiting For Opponent's Get/Out Choice.";
        }
    }
}