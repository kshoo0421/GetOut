using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public void CheckEmailOverlap()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        if (databaseManager.checkEmailOverlap())
        {
            showNotificationMessage("Error", "This e-mail is already exist");
            isEmailOvelap = false;
        }
        else
        {
            showNotificationMessage("Success", "You can use this e-mail");
            isEmailOvelap = true;
        }
    }

    public void CheckPasswordOverlap()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        if (databaseManager.checkEmailOverlap())
        {
            showNotificationMessage("Error", "You can't use this Password");
            isPasswordOvelap = false;
        }
        else
        {
            showNotificationMessage("Success", "You can use this password");
            isPasswordOvelap = true;
        }
    }

    public async void EmailSignUp()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        if (string.IsNullOrEmpty(signUpInputFields[0].text) && string.IsNullOrEmpty(signUpInputFields[1].text))
        {
            showNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        if (isEmailOvelap == true)
        {
            showNotificationMessage("Error", "You should check email address");
            return;
        }
        else if (isPasswordOvelap == true)
        {
            showNotificationMessage("Error", "You should check password");
            return;
        }
        else
        {
            await databaseManager.SignIn(signUpInputFields[0].text, signUpInputFields[1].text);
            showNotificationMessage("Success", "Sign Up Done");
        }
        return;
    }
    #endregion
}
