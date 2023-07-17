using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject EmailPanel;
    #endregion

    #region Toggle Panels
    public void ToggleEmailPanel(bool b)
    {
        SoundEffectManager.PlaySound(Sound.Button);
        EmailPanel.SetActive(b);
    }
    #endregion

    #region Google Login
    public async void GoogleLogin()
    { 
        await DatabaseManager.Instance.GoogleSignInMethod();
        if (DatabaseManager.Instance.GetCurUser() == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log("로그인 성공");
            SceneManager.LoadScene(1);
        }
    }
    #endregion
}