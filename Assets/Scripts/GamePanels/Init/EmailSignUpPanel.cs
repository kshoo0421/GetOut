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
    private int currentSignUpInputFieldIndex = 0; // ���� ��Ŀ�� ȸ������ �ε���
    [SerializeField] private TMP_InputField[] signUpInputFields; // ȸ������


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
        // ù ��° InputField�� ��Ŀ�� ����
        signUpInputFields[0].Select();
    }

    private void SignUpFieldFocus()
    {
        currentInputField = signUpInputFields[currentSignUpInputFieldIndex];
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
                showNotificationMessage("����", "�̸��ϰ� ��й�ȣ�� ��� �Է��ߴ��� Ȯ�����ּ���.");
            }
            return;
        }

        await SignUp(signUpInputFields[0].text, signUpInputFields[1].text);
    }
    
    private async Task SignUp(string emailText, string passwordText)    // ȸ������
    {
        await DatabaseManager.auth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            task => {
                if (task.IsCanceled)
                {
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "����",
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
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "����",
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
                    showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "����",
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

            showNotificationMessage(OptionManager.curLocale == 0 ? "Error" : "����",
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
                    message = "�̸����� ��ȿ���� �ʽ��ϴ�.";
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
