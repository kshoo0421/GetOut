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
    private int currentSignInInputFieldIndex = 0; // 현재 포커스 로그인 인덱스

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
        // 첫 번째 InputField에 포커스 설정
        signInInputFields[0].Select();
    }

    private void SignInFieldFocus()
    {
        currentInputField = signInInputFields[currentSignInInputFieldIndex];
        if (Input.GetKeyDown(KeyCode.Tab)) SignInFieldTab();
    }

    private void SignInFieldTab()
    {
        // 현재 InputField의 다음 순서 InputField로 포커스 이동
        currentSignInInputFieldIndex = (currentSignInInputFieldIndex + 1) % signInInputFields.Length;
        nextInputField = signInInputFields[currentSignInInputFieldIndex];

        // 다음 InputField에 포커스 설정
        nextInputField.Select();
        nextInputField.ActivateInputField();
    }
    #endregion

    #region Sign In
    public async void EmailSignIn()
    {
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
                Debug.Log("로그인 성공");
            }
            SceneManager.LoadScene("Lobby");
        }
    }
    #endregion

    #region Panels Control
    public void OpenSignUpPanel() => SignUpPanel.SetActive(true);
    public void CloseSignUpPanel() => SignUpPanel.SetActive(false);

    public void OpenFindPasswordPanel() => FindPasswordPanel.SetActive(true);
    public void CloseFindPasswordPanel() => FindPasswordPanel.SetActive(false);
    #endregion
}
