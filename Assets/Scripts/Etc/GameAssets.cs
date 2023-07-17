using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioSource audioSource;
    }
}
