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
    private int currentSignInInputFieldIndex = 0; // ���� ��Ŀ�� �α��� �ε���

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
        // ù ��° InputField�� ��Ŀ�� ����
        signInInputFields[0].Select();
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
                showNotificationMessage("����", "���� ��� �Է��� �̷����� �ʾҽ��ϴ�. �ٽ� �� �� Ȯ�����ּ���.");
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
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "����",
                            task.Exception.Message);
                }
                else if (task.IsFaulted)
                {
                    Exception exception = task.Exception;
                    FirebaseException firebaseEx = exception.GetBaseException() as FirebaseException;
                    if (firebaseEx != null)
                    {
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        
                        showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "����", 
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
                    message = "������ �������� �ʽ��ϴ�.";
                    break;
                case AuthError.MissingPassword:
                    message = "��й�ȣ�� �������� �ʽ��ϴ�.";
                    break;
                case AuthError.WrongPassword:
                    message = "�߸��� ��й�ȣ�Դϴ�.";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "�̹� ���ӵ� �����Դϴ�.";
                    break;
                case AuthError.InvalidEmail:
                    message = "�̸��� ������ ��ȿ���� �ʽ��ϴ�.";
                    break;
                case AuthError.MissingEmail:
                    message = "�̸����� �������� �ʽ��ϴ�.";
                    break;
                default:
                    message = "��ȿ���� ���� �����Դϴ�.";
                    break;
            }
        }
        return message;
    }
    #endregion
}
