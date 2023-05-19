using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;

public class PlayerNameText : MonoBehaviour
{
    private TMP_Text nameText;

    private void Start()
    {
        nameText = GetComponent<TMP_Text>();
        
        if(FirebaseManager.User != null)
        {
            //Debug.Log($"{FirebaseManager.User.Email}");
            //nameText.text = $"Hi! {FirebaseManager.User.Email}";
        }
        else
        {
            nameText.text = "ERROR : AuthManager.User == null";
        }
    }
}
