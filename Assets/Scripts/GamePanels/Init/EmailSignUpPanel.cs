using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

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
    [SerializeField] private TMP_Text signUpMessage;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        SelectInputField();
        databaseManager = DatabaseManager.Instance;

    }

    private void Update()
    {
        SignUpFieldFocus();
    }
    #endregion

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
    public async void EmailSignUp()
    {
        if (string.IsNullOrEmpty(signUpInputFields[0].text) && string.IsNullOrEmpty(signUpInputFields[1].text))
        {
            if (OptionManager.curLocale == 0)
            {
                showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            }
            else
            {
                showNotificationMessage("오류", "이메일과 비밀번호를 모두 입력했는지 확인해주세요.");
            }
            return;
        }

        await SignUp(signUpInputFields[0].text, signUpInputFields[1].text);
    }
    
    private async Task SignUp(string emailText, string passwordText)    // 회원가입
    {
        await DatabaseManager.auth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            task => {
                if (task.IsCanceled)
                {
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "오류",
                                task.Exception.Message);
                    return;
                }
                if (task.IsFaulted)
                {
                    ShowException(task.Exception);
                    return;
                }

                DatabaseManager.User = task.Result.User;
                int index = emailText.IndexOf("@");
                UpdateUserProfile(emailText.Substring(0, index));
            });
    }

    private void UpdateUserProfile(string UserName)
    {
        FirebaseUser user = DatabaseManager.User;
        if (user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = UserName
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "오류",
                            task.Exception.Message);
                    return;
                }
                if (task.IsFaulted)
                {
                    ShowException(task.Exception);
                    return;
                }
            });
        }
    }

    public async Task SignIn(string emailText, string passwordText)
    {
        await DatabaseManager.auth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCanceled)
                {
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "오류",
                            task.Exception.Message);
                }
                else if (task.IsFaulted)
                {
                    ShowException(task.Exception);
                }
                else
                {
                    DatabaseManager.User = task.Result.User;
                    databaseManager.SetPhotonNickName();
                }
            });
    }

    private void ShowException(Exception exception)
    {
        FirebaseException firebaseEx = exception.GetBaseException() as FirebaseException;
        if (firebaseEx != null)
        {
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "오류",
                GetErrorMessage(errorCode));
        }
    }

    private string GetErrorMessage(AuthError errorCode)
    {
        string message = "";
        if (OptionManager.curLocale == 0)
        {
            switch (errorCode)
            {
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Your Email Already in Use";
                    break;
                case AuthError.InvalidEmail:
                    message = "Your Email Invalid";
                    break;
                case AuthError.MissingEmail:
                    message = "Your Email Missing";
                    break;
                default:
                    message = "Invalid Error";
                    break;
            }
        }
        else
        {
            switch (errorCode)
            {
                case AuthError.AccountExistsWithDifferentCredentials:
                    message = "계정이 존재하지 않습니다.";
                    break;
                case AuthError.MissingPassword:
                    message = "비밀번호가 존재하지 않습니다.";
                    break;
                case AuthError.WrongPassword:
                    message = "잘못된 비밀번호입니다.";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "이미 접속된 계정입니다.";
                    break;
                case AuthError.InvalidEmail:
                    message = "이메일이 유효하지 않습니다.";
                    break;
                case AuthError.MissingEmail:
                    message = "이메일이 존재하지 않습니다.";
                    break;
                default:
                    message = "유효하지 않은 오류입니다.";
                    break;
            }
        }
        return message;
    }

    #endregion
}
