using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmailSignInPanel : InitPanels
{
    #region Fields
    public bool isRemember;
    private DatabaseManager databaseManager;
    private TMP_InputField nextInputField;
    private TMP_InputField currentInputField;
    [SerializeField] private TMP_InputField[] signInInputFields; // 0 : email, 1 : password
    private int currentSignInInputFieldIndex = 0; // ���� ��Ŀ�� �α��� �ε���

    [SerializeField] private TMP_Text signInMessage;

    [SerializeField] private GameObject SignUpPanel;
    [SerializeField] private GameObject FindPasswordPanel;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        SelectInputField();
        databaseManager = DatabaseManager.Instance;
    }

    private void Update()
    {
        SignInFieldFocus();
    }
    #endregion

    #region Select InputField 
    private void SelectInputField()
    {
        // ù ��° InputField�� ��Ŀ�� ����
        signInInputFields[0].Select();
    }

    private void SignInFieldFocus()
    {
        currentInputField = signInInputFields[currentSignInInputFieldIndex];
        if (Input.GetKeyDown(KeyCode.Tab)) SignInFieldTab();
    }

    private void SignInFieldTab()
    {
        // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
        currentSignInInputFieldIndex = (currentSignInInputFieldIndex + 1) % signInInputFields.Length;
        nextInputField = signInInputFields[currentSignInInputFieldIndex];

        // ���� InputField�� ��Ŀ�� ����
        nextInputField.Select();
        nextInputField.ActivateInputField();
    }
    #endregion

    #region Sign In
    public async void EmailSignIn()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        if (string.IsNullOrEmpty(signInInputFields[0].text) && string.IsNullOrEmpty(signInInputFields[1].text))
        {
            showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        if (databaseManager.checkSignIn())
        {
            await databaseManager.SignIn(signInInputFields[0].text, signInInputFields[1].text);

            if (databaseManager.GetCurUser() == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log("�α��� ����");
            }
            SceneManager.LoadScene("Lobby");
        }
    }
    #endregion

    #region Panels Control
    public void OpenSignUpPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        SignUpPanel.SetActive(true);
    }

    public void CloseSignUpPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        SignUpPanel.SetActive(false);
    }

    public void OpenFindPasswordPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        FindPasswordPanel.SetActive(true); 
    }

    public void CloseFindPasswordPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        FindPasswordPanel.SetActive(false); 
    }
    #endregion
}
