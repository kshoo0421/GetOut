using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_InitialManager : MonoBehaviour
{
    #region Field
    /* Managers */
    TotalGameManager totalGameManager;
    FirebaseManager firebaseManager;
    B_SceneChangeManager b_SceneChangeManager;
    LanguageManager languageManager;

    /* Select InputField */ 
    TMP_InputField nextInputField;
    TMP_InputField currentInputField;
    [SerializeField] TMP_InputField[] signInInputFields; // 로그인
    [SerializeField] TMP_InputField[] signUpInputFields; // 회원가입
    int currentSignInInputFieldIndex = 0; // 현재 포커스 로그인 인덱스
    int currentSignUpInputFieldIndex = 0; // 현재 포커스 회원가입 인덱스

    /* Open Keyboard */
    TouchScreenKeyboard keyboard;

    /* Sign In */
    [SerializeField] GameObject SignInPanel;
    [SerializeField] TMP_InputField emailLogInField;
    [SerializeField] TMP_InputField passwordLogInField;

    /* Sign Up */
    bool isEmailOvelap = true;
    bool isPasswordOvelap = true;

    [SerializeField] GameObject SignUpPanel;
    [SerializeField] TMP_Text signUpMessage;

    [SerializeField] Button signUpButton;
    [SerializeField] Button signUpPanelOnBtn;
    [SerializeField] Button signUpPanelOffBtn;
    [SerializeField] Button checkEmailOverlapBtn;
    [SerializeField] Button checkPasswordOverlapBtn;

    [SerializeField] TMP_InputField emailSignUpField;
    [SerializeField] TMP_InputField passwordSignUpField;

    #endregion

    #region MonoBehaviour
    private void Start()
    {
        SetManagers();
        SelectInputField();
    }

    private void Update()
    {
        FocusUpdate();
    }
    #endregion

    #region Set Managers
    void SetManagers()
    {
        totalGameManager = TotalGameManager.Instance;
        firebaseManager = totalGameManager.firebaseManager;
        b_SceneChangeManager = totalGameManager.b_SceneChangeManager;
        languageManager = totalGameManager.languageManager;
    }
    #endregion

    #region Select InputField 
    void SelectInputField()
    {
        // 첫 번째 InputField에 포커스 설정
        signInInputFields[0].Select();
        signUpInputFields[0].Select();
    }

    void FocusUpdate()
    {
        // 현재 포커스를 가지고 있는 InputField
        if (SignUpPanel.activeSelf == true)
        {
            currentInputField = signUpInputFields[currentSignInInputFieldIndex];
            // Tab 키를 눌렀을 때 다음 InputField로 포커스 이동
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // 현재 InputField의 다음 순서 InputField로 포커스 이동
                currentSignUpInputFieldIndex = (currentSignUpInputFieldIndex + 1) % signUpInputFields.Length;
                nextInputField = signUpInputFields[currentSignUpInputFieldIndex];

                // 다음 InputField에 포커스 설정
                nextInputField.Select();
                nextInputField.ActivateInputField();
            }
        }
        else
        {
            currentInputField = signInInputFields[currentSignInInputFieldIndex];
            // Tab 키를 눌렀을 때 다음 InputField로 포커스 이동
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // 현재 InputField의 다음 순서 InputField로 포커스 이동
                currentSignInInputFieldIndex = (currentSignInInputFieldIndex + 1) % signInInputFields.Length;
                nextInputField = signInInputFields[currentSignInInputFieldIndex];

                // 다음 InputField에 포커스 설정
                nextInputField.Select();
                nextInputField.ActivateInputField();
            }
        }
    }
    #endregion

    #region Open Keyboard
    public void OpenKeyboard() => keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

    public void CloseKeyboard()
    {
        if (keyboard != null)
        {
            keyboard.active = false;
            keyboard = null;
        }
    }
    #endregion

    #region Sign In
    public void OpenSignInPanel()
    {
        if (SignInPanel.activeSelf == false)
        {
            SignInPanel.SetActive(true);
        }
    }

    public void CloseSignInPanel()
    {
        if (SignInPanel.activeSelf == true)
        {
            SignInPanel.SetActive(false);
        }
    }

    public async void SignIn()
    {
        if (firebaseManager.checkSignIn())
        {
            await firebaseManager.SignIn(emailLogInField.text, passwordLogInField.text);

            if (firebaseManager.GetCurUser() == null)
                Debug.Log("null");
            else
            {
                Debug.Log(firebaseManager.GetCurUser().Email);
                Debug.Log("로그인 성공");
            }
            ChangeToScene(2);
        }
    }
    #endregion

    #region Sign Up
    public void OpenSignUpPanel()
    {
        if (SignUpPanel.activeSelf == false) SignUpPanel.SetActive(true);
    }

    public void CloseSignUpPanel()
    {
        if (SignUpPanel.activeSelf == true) SignUpPanel.SetActive(false);
    }

    public void CheckEmailOverlap()
    {
        if (firebaseManager.checkEmailOverlap())
        {
            signUpMessage.text = "This e-mail is already exist";
            isEmailOvelap = false;
        }
        else
        {
            signUpMessage.text = "You can use this e-mail";
            isEmailOvelap = true;
        }
    }

    public void CheckPasswordOverlap()
    {
        if (firebaseManager.checkEmailOverlap())
        {
            signUpMessage.text = "You can't use this Password";
            isPasswordOvelap = false;
        }
        else
        {
            signUpMessage.text = "You can use this Password";
            isPasswordOvelap = true;
        }
    }

    public void PressSignUpButton()
    {
        if (isEmailOvelap == true)
        {
            signUpMessage.text = "You should check email address";
            return;
        }
        else if (isPasswordOvelap == true)
        {
            signUpMessage.text = "You should check password";
            return;
        }
        else
        {
            firebaseManager.SignIn(emailSignUpField.text, passwordSignUpField.text);
            signUpMessage.text = "Sign Up Done";
        }
        return;
    }
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => b_SceneChangeManager.ChangetoScene(sceneIndex);
    #endregion

    #region Set Language
    [SerializeField] GameObject LanguageSettingPanel;

    public void LanguageSettingPanelOpenAndClose()
    {
        Debug.Log(LanguageSettingPanel.activeSelf);
        if (LanguageSettingPanel.activeSelf)
        {
            LanguageSettingPanel.SetActive(false);
        }
        else
        {
            LanguageSettingPanel.SetActive(true);
        }
    }

    public void ChangeLocale(int index)
    {
        languageManager.ChangeLocale(index);
    }
    #endregion
}