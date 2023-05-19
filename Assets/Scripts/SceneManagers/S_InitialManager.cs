using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.VersionControl;

public class S_InitialManager : MonoBehaviour
{
    #region Start - Awake - Update
    [SerializeField] private GameObject baseManagers;
    [SerializeField] private GameObject classManagers;

    TMP_InputField nextInputField;
    TMP_InputField currentInputField;

    [SerializeField] private TMP_InputField[] signInInputFields; // 로그인
    private int currentSignInInputFieldIndex = 0; // 현재 포커스를 가지고 있는 InputField 인덱스

    [SerializeField] private TMP_InputField[] signUpInputFields; // 회원가입
    private int currentSignUpInputFieldIndex = 0; // 현재 포커스를 가지고 있는 InputField 인덱스

    private void Start()
    {
        // 첫 번째 InputField에 포커스 설정
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
        // 현재 포커스를 가지고 있는 InputField
        if(SignUpPanel.activeSelf == true)
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

    #region 로그인 패널 관리
    [SerializeField] private GameObject SignInPanel;

    public void OpenSignInPanel()
    {
        if(SignInPanel.activeSelf == false) 
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
    #endregion

    #region 회원가입 패널 관리
    private bool isEmailOvelap = true;
    private bool isPasswordOvelap = true;

    FirebaseManager firebaseManager = FirebaseManager.Instance;

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

    #region 씬 변경
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
    #endregion

    #region 언어 설정 패널 관리
    [SerializeField] private GameObject LanguageSettingPanel;

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