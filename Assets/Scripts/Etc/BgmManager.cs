using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private static BgmManager _instance;
    public static AudioSource bgm;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            bgm = GetComponent<AudioSource>();
            bgm.volume = 1.0f;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}