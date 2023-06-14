using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S00_Initial : Scenes
{
    #region Field
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

    /* SoundEffect */
    [SerializeField] AudioSource[] soundEffects;

    /* BGM */
    
    #endregion

    #region MonoBehaviour
    void Start()
    {
        SetManagers();
        SelectInputField();
        //InitializeSEVolume();
    }

    void Update()
    {
        FocusUpdate();
    }
    #endregion

    #region Select InputField 
    void SelectInputField()
    {
        // 첫 번째 InputField에 포커스 설정
        signInInputFields[0].Select();
        signUpInputFields[0].Select();
    }

    void SetEnterKey()
    {
        signInInputFields[0].onEndEdit.AddListener(EnterSignInID);
        signUpInputFields[0].onEndEdit.AddListener(EnterSignUpID);

        signInInputFields[1].onEndEdit.AddListener(EnterSignInPW);
        signUpInputFields[1].onEndEdit.AddListener(EnterSignUpPW);
    }

    void EnterSignInID(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) SignInFieldTab();
    }

    void EnterSignUpID(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) SignUpFieldTab();
    }


    void EnterSignInPW(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SignIn();
        }
    }

    void EnterSignUpPW(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) SignUp();
    }
    
    void FocusUpdate()
    {
        // 현재 포커스를 가지고 있는 InputField
        if (SignUpPanel.activeSelf == true) SignUpFieldFocus();   
        else SignInFieldFocus();
    }

    void SignUpFieldFocus()
    {
        currentInputField = signUpInputFields[currentSignInInputFieldIndex];
        if (Input.GetKeyDown(KeyCode.Tab)) SignUpFieldTab();
    }

    void SignUpFieldTab()
    { 
        // 현재 InputField의 다음 순서 InputField로 포커스 이동
        currentSignUpInputFieldIndex = (currentSignUpInputFieldIndex + 1) % signUpInputFields.Length;
        nextInputField = signUpInputFields[currentSignUpInputFieldIndex];

        // 다음 InputField에 포커스 설정
        nextInputField.Select();
        nextInputField.ActivateInputField();
    }

    void SignInFieldFocus()
    {
        currentInputField = signInInputFields[currentSignInInputFieldIndex];
        if (Input.GetKeyDown(KeyCode.Tab)) SignInFieldTab();
    }

    void SignInFieldTab()
    {
        // 현재 InputField의 다음 순서 InputField로 포커스 이동
        currentSignInInputFieldIndex = (currentSignInInputFieldIndex + 1) % signInInputFields.Length;
        nextInputField = signInInputFields[currentSignInInputFieldIndex];

        // 다음 InputField에 포커스 설정
        nextInputField.Select();
        nextInputField.ActivateInputField();
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
        Debug.Log("Sign In");
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
            ChangeToScene(1);
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

    public async void SignUp()
    {
        Debug.Log("Sign Up");
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
            await firebaseManager.SignIn(emailSignUpField.text, passwordSignUpField.text);
            signUpMessage.text = "Sign Up Done";
        }
        return;
    }
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
        optionManager.ChangeLocale(index);
    }
    #endregion

    #region Set Sound
    //void InitializeSEVolume()
    //{
    //    float curSEVolume = soundManager.curSEVolume;
    //    foreach (AudioSource i in soundEffects)
    //        i.volume = curSEVolume;
    //}

    //public void SetSEVolume(float curSEVolume)
    //{
    //    soundManager.curSEVolume = curSEVolume;
    //    foreach (AudioSource i in soundEffects)
    //    {
    //        i.volume = curSEVolume;
    //    }
    //}
    #endregion

    #region Option
    [SerializeField] GameObject OptionPanel;
    public void ToggleOptionPanel()
    {
        if (OptionPanel.activeSelf)
        {
            OptionPanel.SetActive(false);
        }
        else
        {
            OptionPanel.SetActive(true);
        }
    }

    [SerializeField] GameObject[] InOptionPanels; // 0 : Sound; 1 : Language; 2 : Resolution
    
    public void SelectOptionPanel(int panelNum)
    {
        for(int i = 0; i < 3; i++)
        {
            if (i == panelNum)
            {
                InOptionPanels[i].SetActive(true);
            }
            else
            {
                InOptionPanels[i].SetActive(false);
            }
        }
    }
    #endregion

    #region Quit Game
    [SerializeField] GameObject QuitPanel;

    public void ToggleQuitPanel()
    { 
        if(QuitPanel.activeSelf)
        {
            QuitPanel.SetActive(false);
        }
        else
        {
            QuitPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}