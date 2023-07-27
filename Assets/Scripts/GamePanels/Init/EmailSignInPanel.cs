using System;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Firebase.Extensions;


public class EmailSignInPanel : InitPanels
{
    #region Fields
    public bool isRemember;
    private DatabaseManager databaseManager;
    private TMP_InputField nextInputField;
    private TMP_InputField currentInputField;
    private int currentSignInInputFieldIndex = 0; // 현재 포커스 로그인 인덱스

    private static bool IsSignInOnProgress { get; set; }

    [SerializeField] private TMP_InputField[] signInInputFields; // 0 : email, 1 : password

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
            if (OptionManager.curLocale == 0)
            {
                showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            }
            else
            {
                showNotificationMessage("오류", "아직 모든 입력이 이뤄지지 않았습니다. 다시 한 번 확인해주세요.");
            }
            return;
        }

        if (checkSignIn())
        {
            await SignIn(signInInputFields[0].text, signInInputFields[1].text);

            if (databaseManager.GetCurUser() != null)
            {
                await databaseManager.SetUserData();
                SceneManager.LoadScene("Lobby");
            }
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

    #region Email Sign In
    public bool checkSignIn()
    {
        if (!DatabaseManager.IsFirebaseReady || IsSignInOnProgress || DatabaseManager.User != null)
        {
            return false;
        }
        return true;
    }

    public async Task SignIn(string emailText, string passwordText)
    {
        IsSignInOnProgress = true;
        await DatabaseManager.auth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            task =>
            {
                IsSignInOnProgress = false;

                if (task.IsCanceled)
                {
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "오류",
                            task.Exception.Message);
                }
                else if (task.IsFaulted)
                {
                    Exception exception = task.Exception;
                    FirebaseException firebaseEx = exception.GetBaseException() as FirebaseException;
                    if (firebaseEx != null)
                    {
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        
                        showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "오류", 
                            GetErrorMessage(errorCode));
                    }
                    else
                    {
                        Debug.Log("firebaseEx == null");
                        AggregateException err = exception as AggregateException;
                        foreach (var errInner in err.InnerExceptions)
                        {
                            Debug.Log($"exception : {errInner}");
                        }
                    }
                }
                else
                {
                    DatabaseManager.User = task.Result.User;
                    databaseManager.SetPhotonNickName();
                }
            });
    }

    private string GetErrorMessage(AuthError errorCode)
    {
        string message = "";
        if (OptionManager.curLocale == 0)
        {
            switch (errorCode)
            {
                case AuthError.AccountExistsWithDifferentCredentials:
                    message = "Account is Not Exist";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Your Email is Already in Use";
                    break;
                case AuthError.InvalidEmail:
                    message = "Your Email is Invalid";
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
                    message = "이메일 형식이 유효하지 않습니다.";
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
