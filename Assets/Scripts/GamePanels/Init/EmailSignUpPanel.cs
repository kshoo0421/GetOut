using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailSignUpPanel : InitPanels
{
    #region Fields
    private DatabaseManager databaseManager;
    private TMP_InputField nextInputField;
    private TMP_InputField currentInputField;
    private int currentSignUpInputFieldIndex = 0; // 현재 포커스 회원가입 인덱스
    [SerializeField] private TMP_InputField[] signUpInputFields; // 회원가입


    /* Sign Up */
    private bool isEmailOvelap = true;
    private bool isPasswordOvelap = true;


    [SerializeField] private Button signUpButton;
    [SerializeField] private Button checkEmailOverlapBtn;
    [SerializeField] private Button checkPasswordOverlapBtn;
    #endregion

    private void OnEnable()
    {
        SelectInputField();
        databaseManager = DatabaseManager.Instance;
    }

    private void Update()
    {
        SignUpFieldFocus();
    }

    #region Select InputField 
    private void SelectInputField()
    {
        // 첫 번째 InputField에 포커스 설정
        signUpInputFields[0].Select();
    }

    private void SignUpFieldFocus()
    {
        currentInputField = signUpInputFields[currentSignUpInputFieldIndex];
        if (Input.GetKeyDown(KeyCode.Tab)) SignUpFieldTab();
    }

    private void SignUpFieldTab()
    {
        // 현재 InputField의 다음 순서 InputField로 포커스 이동
        currentSignUpInputFieldIndex = (currentSignUpInputFieldIndex + 1) % signUpInputFields.Length;
        nextInputField = signUpInputFields[currentSignUpInputFieldIndex];

        // 다음 InputField에 포커스 설정
        nextInputField.Select();
        nextInputField.ActivateInputField();
    }
    #endregion

    #region Sign Up
    public void CheckEmailOverlap()
    {
        if (databaseManager.checkEmailOverlap())
        {
            showNotificationMessage("Error", "This e-mail is already exist");
            isEmailOvelap = false;
        }
        else
        {
            showNotificationMessage("Success", "You can use this e-mail");
            isEmailOvelap = true;
        }
    }

    public void CheckPasswordOverlap()
    {
        if (databaseManager.checkEmailOverlap())
        {
            showNotificationMessage("Error", "You can't use this Password");
            isPasswordOvelap = false;
        }
        else
        {
            showNotificationMessage("Success", "You can use this password");
            isPasswordOvelap = true;
        }
    }

    public async void EmailSignUp()
    {
        if (string.IsNullOrEmpty(signUpInputFields[0].text) && string.IsNullOrEmpty(signUpInputFields[1].text))
        {
            showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        if (isEmailOvelap == true)
        {
            showNotificationMessage("Error", "You should check email address");
            return;
        }
        else if (isPasswordOvelap == true)
        {
            showNotificationMessage("Error", "You should check password");
            return;
        }
        else
        {
            await databaseManager.SignIn(signUpInputFields[0].text, signUpInputFields[1].text);
            showNotificationMessage("Success", "Sign Up Done");
        }
        return;
    }
    #endregion
}
