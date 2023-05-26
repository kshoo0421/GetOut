using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseUser User;

    #region 싱글톤 생성용
    private static FirebaseManager instance;
    private FirebaseManager() {}
    public static FirebaseManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<FirebaseManager>();
            InitializeFM();
        }
    }
    #endregion

    #region monobehavior
    [SerializeField] private static FirebaseApp firebaseApp;
    [SerializeField] private static FirebaseAuth firebaseAuth;

    [SerializeField] private static bool IsFirebaseReady { get; set; }
    [SerializeField] private static bool IsSignInOnProgress { get; set; }

    private static DatabaseReference reference;

    void InitializeFM()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(continuation: task =>
            {
                var result = task.Result;
                if (result != DependencyStatus.Available)
                {
                    Debug.LogError(message: result.ToString());
                    IsFirebaseReady = false;
                }
                else
                {
                    IsFirebaseReady = true;
                    firebaseApp = FirebaseApp.DefaultInstance;
                    firebaseAuth = FirebaseAuth.DefaultInstance;

//                    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://getout-38c84-default-rtdb.firebaseio.com/");
                    reference = FirebaseDatabase.DefaultInstance.RootReference;

                }
            }
        );
    }
    #endregion

    #region 회원가입
    public bool checkEmailOverlap() // 이메일 중복여부 체크
    {
        return true;
    }

    public bool checkPasswordOverlap()  // 비밀번호 조건 충족여부 체크
    {
        return true;
    }

    public void SignUp(string emailText, string passwordText)    // 회원가입
    {
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailText, passwordText).ContinueWith(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if(task.IsFaulted)
            {
                // 회원가입 실패 이유 => 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등등...
                Debug.LogError("회원가입 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("회원가입 완료");
        });
    }
    #endregion

    #region 로그인
    public bool checkSignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return false;
        }
        return true;
    }

    public async Task SignIn(string emailText, string passwordText)
    {
        IsSignInOnProgress = true;
        
        await firebaseAuth.SignInWithEmailAndPasswordAsync(emailText, passwordText).ContinueWithOnMainThread(
            (task) =>
            {
                Debug.Log(message: $"Sign in status : {task.Status}");

                IsSignInOnProgress = false;
       
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError(message: "Sign-in canceled");
                }
                else
                {
                    User = task.Result.User;
                }
            });
    }
    #endregion

    #region User 정보 가져오기
    public FirebaseUser GetCurUser()
    {
        if(User != FirebaseAuth.DefaultInstance.CurrentUser)
            User = FirebaseAuth.DefaultInstance.CurrentUser;
        return User;
    }
    #endregion

    #region 데이터베이스
    public async void SaveData(ResultData resultData)
    {
        string json = JsonUtility.ToJson(resultData);
        await reference.Child("WriteTest").SetRawJsonValueAsync(json);
    }
    #endregion
}