using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S00_Initial : Scenes
{
    #region Field
    /* Select InputField */ 
    TMP_InputField nextInputField;
    TMP_InputField currentInputField;
    [SerializeField] TMP_InputField[] signInInputFields; // �α���
    [SerializeField] TMP_InputField[] signUpInputFields; // ȸ������
    int currentSignInInputFieldIndex = 0; // ���� ��Ŀ�� �α��� �ε���
    int currentSignUpInputFieldIndex = 0; // ���� ��Ŀ�� ȸ������ �ε���


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
    void Start()
    {
        SetManagers();
        SelectInputField();
        optionManager.InitializeVolume();
    }

    void Update()
    {
        FocusUpdate();
    }
    #endregion

    #region Select InputField 
    void SelectInputField()
    {
        // ù ��° InputField�� ��Ŀ�� ����
        signInInputFields[0].Select();
        signUpInputFields[0].Select();
    }

    void FocusUpdate()
    {
        // ���� ��Ŀ���� ������ �ִ� InputField
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
        // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
        currentSignUpInputFieldIndex = (currentSignUpInputFieldIndex + 1) % signUpInputFields.Length;
        nextInputField = signUpInputFields[currentSignUpInputFieldIndex];

        // ���� InputField�� ��Ŀ�� ����
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
        // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
        currentSignInInputFieldIndex = (currentSignInInputFieldIndex + 1) % signInInputFields.Length;
        nextInputField = signInInputFields[currentSignInInputFieldIndex];

        // ���� InputField�� ��Ŀ�� ����
        nextInputField.Select();
        nextInputField.ActivateInputField();
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
                Debug.Log("�α��� ����");
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
}