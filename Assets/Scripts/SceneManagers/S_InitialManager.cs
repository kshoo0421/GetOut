using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.VersionControl;

public class S_InitialManager : MonoBehaviour
{
    #region Start - Awake - Update
    [SerializeField] private GameObject totalGMObject;
    private TotalGameManager totalGameManager;
    private FirebaseManager firebaseManager;
    private B_DatabaseManager b_DatabaseManager;
    private B_SceneChangeManager b_SceneChangeManager;

    TMP_InputField nextInputField;
    TMP_InputField currentInputField;

    [SerializeField] private TMP_InputField[] signInInputFields; // 로그인
    private int currentSignInInputFieldIndex = 0; // 현재 포커스를 가지고 있는 InputField 인덱스

    [SerializeField] private TMP_InputField[] signUpInputFields; // 회원가입
    private int currentSignUpInputFieldIndex = 0; // 현재 포커스를 가지고 있는 InputField 인덱스

    private void Awake()
    {
        DontDestroyOnLoad(totalGMObject);
    }

    private void Start()
    {
        // 첫 번째 InputField에 포커스 설정
        signInInputFields[0].Select();
        signUpInputFields[0].Select();

        totalGameManager = TotalGameManager.Instance;
        firebaseManager = totalGameManager.firebaseManager;
        b_DatabaseManager = totalGameManager.b_DatabaseManager;
        b_SceneChangeManager = totalGameManager.b_SceneChangeManager;
    }

    private void Update()
    {
        // 현재 포커스를 가지고 있는 InputField
        if (SignUpPanel.activeSelf == true)
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

    #region 로그인
    // 로그인 패널 관리
    [SerializeField] private GameObject SignInPanel;

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

    // 로그인
    [SerializeField] private TMP_InputField emailLogInField;
    [SerializeField] private TMP_InputField passwordLogInField;

    public void SignIn()
    {
        if (firebaseManager.checkSignIn())
        {
            firebaseManager.SignIn(emailLogInField.text, passwordLogInField.text);
            ChangeToScene(1);
        }
    }
    #endregion

    #region 회원가입 패널 관리
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

    public void OpenSignUpPanel()
    {
        if (SignUpPanel.activeSelf == false)
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
            firebaseManager.SignIn(emailSignUpField.text, passwordSignUpField.text);
            signUpMessage.text = "Sign Up Done";
        }
        return;
    }
    #endregion

    #region 씬 변경

    public void ChangeToScene(int sceneIndex)
    {
        b_SceneChangeManager.ChangetoScene(sceneIndex);
    }
    #endregion

    #region 언어 설정 패널 관리
    private LanguageManager languageManager = LanguageManager.Instance;
    [SerializeField] private GameObject LanguageSettingPanel;

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
        languageManager.ChangeLocale(index);
    }
    #endregion

    #region 테스트
    public void MakeResultData()    // 데이터베이스 테스트용
    {
        ResultData resultData = new ResultData();
        resultData.GAME_KEY = "123456789";

        resultData.finalResult.first = 2;
        resultData.finalResult.second = 1;
        resultData.finalResult.third = 4;
        resultData.finalResult.fourth = 3;

        resultData.turn1.player1.isGet = true;
        resultData.turn1.player1.value = 50;
        resultData.turn1.player2.isGet = true;
        resultData.turn1.player2.value = 50;
        resultData.turn1.player3.isGet = true;
        resultData.turn1.player3.value = 50;
        resultData.turn1.player4.isGet = true;
        resultData.turn1.player4.value = 50;

        resultData.turn2.player1.isGet = true;
        resultData.turn2.player1.value = 50;
        resultData.turn2.player2.isGet = true;
        resultData.turn2.player2.value = 50;
        resultData.turn2.player3.isGet = true;
        resultData.turn2.player3.value = 50;
        resultData.turn2.player4.isGet = true;
        resultData.turn2.player4.value = 50;

        resultData.turn3.player1.isGet = true;
        resultData.turn3.player1.value = 50;
        resultData.turn3.player2.isGet = true;
        resultData.turn3.player2.value = 50;
        resultData.turn3.player3.isGet = true;
        resultData.turn3.player3.value = 50;
        resultData.turn3.player4.isGet = true;
        resultData.turn3.player4.value = 50;

        resultData.turn4.player1.isGet = true;
        resultData.turn4.player1.value = 50;
        resultData.turn4.player2.isGet = true;
        resultData.turn4.player2.value = 50;
        resultData.turn4.player3.isGet = true;
        resultData.turn4.player3.value = 50;
        resultData.turn4.player4.isGet = true;
        resultData.turn4.player4.value = 50;

        resultData.turn5.player1.isGet = true;
        resultData.turn5.player1.value = 50;
        resultData.turn5.player2.isGet = true;
        resultData.turn5.player2.value = 50;
        resultData.turn5.player3.isGet = true;
        resultData.turn5.player3.value = 50;
        resultData.turn5.player4.isGet = true;
        resultData.turn5.player4.value = 50;

        resultData.turn6.player1.isGet = true;
        resultData.turn6.player1.value = 50;
        resultData.turn6.player2.isGet = true;
        resultData.turn6.player2.value = 50;
        resultData.turn6.player3.isGet = true;
        resultData.turn6.player3.value = 50;
        resultData.turn6.player4.isGet = true;
        resultData.turn6.player4.value = 50;

        resultData.player1.playerName = "User1";
        resultData.player1.playerId = "Test1";
        resultData.player1.curGold = 200;

        resultData.player1.playerName = "User2";
        resultData.player1.playerId = "Test2";
        resultData.player1.curGold = 200;

        resultData.player1.playerName = "User3";
        resultData.player1.playerId = "Test3";
        resultData.player1.curGold = 200;

        resultData.player1.playerName = "User4";
        resultData.player1.playerId = "Test4";
        resultData.player1.curGold = 200;

        resultData.player1Mission.low.missionNum = 1;
        resultData.player1Mission.low.isAchieved = true;
        resultData.player1Mission.mid.missionNum = 1;
        resultData.player1Mission.mid.isAchieved = true;
        resultData.player1Mission.high.missionNum = 1;
        resultData.player1Mission.high.isAchieved = false;

        resultData.player2Mission.low.missionNum = 1;
        resultData.player2Mission.low.isAchieved = true;
        resultData.player2Mission.mid.missionNum = 1;
        resultData.player2Mission.mid.isAchieved = true;
        resultData.player2Mission.high.missionNum = 1;
        resultData.player2Mission.high.isAchieved = false;

        resultData.player3Mission.low.missionNum = 1;
        resultData.player3Mission.low.isAchieved = true;
        resultData.player3Mission.mid.missionNum = 1;
        resultData.player3Mission.mid.isAchieved = true;
        resultData.player3Mission.high.missionNum = 1;
        resultData.player3Mission.high.isAchieved = false;

        resultData.player4Mission.low.missionNum = 1;
        resultData.player4Mission.low.isAchieved = true;
        resultData.player4Mission.mid.missionNum = 1;
        resultData.player4Mission.mid.isAchieved = true;
        resultData.player4Mission.high.missionNum = 1;
        resultData.player4Mission.high.isAchieved = false;

        firebaseManager.SaveData(resultData);
    }
    #endregion
}