using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameText : MonoBehaviour
{
    private TMP_Text nameText;


    private void Start()
    {
        nameText = GetComponent<TMP_Text>();

        if(FirebaseManager.User != null )
        {
            nameText.text = $"Hi! {FirebaseManager.User.Email}";
        }
        else
        {
            nameText.text = "ERROR : AuthManager.User == null";
        }
    }
}
