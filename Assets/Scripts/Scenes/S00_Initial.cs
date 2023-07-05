using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S00_Initial : Scenes
{
    #region Field
    /* Initialize */
    private bool isInitialized = false;
    [SerializeField] private GameObject BeforeLoad;
    [SerializeField] private GameObject AfterLoad;

    /* Select InputField */
    private TMP_InputField nextInputField;
    private TMP_InputField currentInputField;
    [SerializeField] private TMP_InputField[] signInInputFields; // �α���
    [SerializeField] private TMP_InputField[] signUpInputFields; // ȸ������
    private int currentSignInInputFieldIndex = 0; // ���� ��Ŀ�� �α��� �ε���
    private int currentSignUpInputFieldIndex = 0; // ���� ��Ŀ�� ȸ������ �ε���


    /* Sign In */
    [SerializeField] private GameObject SignInPanel;
    [SerializeField] private TMP_InputField emailLogInField;
    [SerializeField] private TMP_InputField passwordLogInField;

    /* Sign Up */
    private bool isEmailOvelap = true;
    private bool isPasswordOvelap = true;

    [SerializeField] private GameObject SignUpPanel;
    [SerializeField] private TMP_Text signUpMessage;

    [SerializeField] private Button signUpButton;
    [SerializeField] private Button signUpPanelOnBtn;
    [SerializeField] private Button signUpPanelOffBtn;
    [SerializeField] private Button checkEmailOverlapBtn;
    [SerializeField] private Button checkPasswordOverlapBtn;

    [SerializeField] private TMP_InputField emailSignUpField;
    [SerializeField] private TMP_InputField passwordSignUpField;

    #endregion

    #region MonoBehaviour
    private void Start()
    {
        InitialSet();
        SelectInputField();
    }

    private void Update()
    {
        if (!isInitialized)
        {
            if (DatabaseManager.IsFirebaseReady && PhotonManager.IsPhotonReady)
            {
                BeforeLoad.SetActive(false);
                AfterLoad.SetActive(true);
                isInitialized = true;
                Debug.Log("�ʱ�ȭ �Ϸ�");
            }
        }
        FocusUpdate();
    }
    #endregion

    #region Select InputField 
    private void SelectInputField()
    {
        // ù ��° InputField�� ��Ŀ�� ����
        signInInputFields[0].Select();
        signUpInputFields[0].Select();
    }

    private void FocusUpdate()
    {
        // ���� ��Ŀ���� ������ �ִ� InputField
        if (SignUpPanel.activeSelf == true) SignUpFieldFocus();
        else SignInFieldFocus();
    }

    private void SignUpFieldFocus()
    {
        currentInputField = signUpInputFields[currentSignInInputFieldIndex];
        if (Input.GetKeyDown(KeyCode.Tab)) SignUpFieldTab();
    }

    private void SignUpFieldTab()
    {
        // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
        currentSignUpInputFieldIndex = (currentSignUpInputFieldIndex + 1) % signUpInputFields.Length;
        nextInputField = signUpInputFields[currentSignUpInputFieldIndex];

        // ���� InputField�� ��Ŀ�� ����
        nextInputField.Select();
        nextInputField.ActivateInputField();
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
        if (databaseManager.checkSignIn())
        {
            await databaseManager.SignIn(emailLogInField.text, passwordLogInField.text);

            if (databaseManager.GetCurUser() == null)
            {
                Debug.Log("null");
            }
            else
            {
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
        if (databaseManager.checkEmailOverlap())
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
        if (databaseManager.checkEmailOverlap())
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
            await databaseManager.SignIn(emailSignUpField.text, passwordSignUpField.text);
            signUpMessage.text = "Sign Up Done";
        }
        return;
    }
    #endregion
}