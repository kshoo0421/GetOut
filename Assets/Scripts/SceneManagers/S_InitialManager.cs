using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.VersionControl;

public class S_InitialManager : MonoBehaviour
{
    #region ���� ����
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    [SerializeField] private GameObject baseManagers;
    [SerializeField] private GameObject classManagers;


    TMP_InputField nextInputField;
    TMP_InputField currentInputField;

   [SerializeField] private TMP_InputField[] signInInputFields; // �α���
    private int currentSignInInputFieldIndex = 0; // ���� ��Ŀ���� ������ �ִ� InputField �ε���
    
    [SerializeField] private TMP_InputField[] signUpInputFields; // ȸ������
    private int currentSignUpInputFieldIndex = 0; // ���� ��Ŀ���� ������ �ִ� InputField �ε���

    [SerializeField] private GameObject LanguageSettingPanel;
    #endregion

    #region Start - Awake - Update
    private void Start()
    {
        // ù ��° InputField�� ��Ŀ�� ����
        signInInputFields[0].Select(); 
        signUpInputFields[0].Select(); 
    }

    private void Awake()
    {
        DontDestroyOnLoad(baseManagers);
        DontDestroyOnLoad(classManagers);
    }

    private void Update()
    {
        // ���� ��Ŀ���� ������ �ִ� InputField
        if(SignUpPanel.activeSelf == true)
        {
            currentInputField = signUpInputFields[currentSignInInputFieldIndex];
            // Tab Ű�� ������ �� ���� InputField�� ��Ŀ�� �̵�
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
                currentSignUpInputFieldIndex = (currentSignUpInputFieldIndex + 1) % signUpInputFields.Length;
                nextInputField = signUpInputFields[currentSignUpInputFieldIndex];

                // ���� InputField�� ��Ŀ�� ����
                nextInputField.Select();
                nextInputField.ActivateInputField();
            }
        }
        else
        {
            currentInputField = signInInputFields[currentSignInInputFieldIndex];
            // Tab Ű�� ������ �� ���� InputField�� ��Ŀ�� �̵�
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // ���� InputField�� ���� ���� InputField�� ��Ŀ�� �̵�
                currentSignInInputFieldIndex = (currentSignInInputFieldIndex + 1) % signInInputFields.Length;
                nextInputField = signInInputFields[currentSignInInputFieldIndex];

                // ���� InputField�� ��Ŀ�� ����
                nextInputField.Select();
                nextInputField.ActivateInputField();
            }
        }
    }
    #endregion

    #region ȸ������ �г� ����
    private bool isEmailOvelap = true;
    private bool isPasswordOvelap = true;

    FirebaseManager firebaseManager = new FirebaseManager();

    [SerializeField] private GameObject SignUpPanel;
    [SerializeField] private TMP_Text signUpMessage;

    [SerializeField] private Button signUpButton;
    [SerializeField] private Button signUpPanelOnBtn;
    [SerializeField] private Button signUpPanelOffBtn;
    [SerializeField] private Button checkEmailOverlapBtn;
    [SerializeField] private Button checkPasswordOverlapBtn;

    public void OpenSignUpPanel()
    {
        if(SignUpPanel.activeSelf == false)
            SignUpPanel.SetActive(true);
    }

    public void CloseSignUpPanel()
    {
        if (SignUpPanel.activeSelf == true)
            SignUpPanel.SetActive(false);
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
        if(isEmailOvelap == true)
        {
            signUpMessage.text = "You should check email address";
            return;
        }
        else if(isPasswordOvelap == true)
        {
            signUpMessage.text = "You should check password";
            return;
        }
        else
        {
            firebaseManager.SignUp();
            signUpMessage.text = "Sign Up Done";
        }
        return;
    }
    #endregion

    #region �� ����
    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
    #endregion

    #region ��� ���� �г� ����
    public void LanguageSettingPanelOpenAndClose()
    {
        Debug.Log(LanguageSettingPanel.activeSelf);
        if(LanguageSettingPanel.activeSelf)
        {
            LanguageSettingPanel.SetActive(false);
        }
        else
        {
            LanguageSettingPanel.SetActive(true);
        }
    }
    #endregion
}