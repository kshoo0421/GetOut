using UnityEngine;

public static class SoundEffectManager   
{
    public static float volume = 1.0f;
    public static bool isMute = false;
    public static void PlaySound(Sound sound)
    {
        AudioSource audioSource = GetAudioSource(sound);
        audioSource.volume = volume;
        audioSource.mute = isMute;
        audioSource.Play();
    }

    private static AudioSource GetAudioSource(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioSource;
            }
        }
        Debug.LogError($"Sound {sound} not found!");
        return null;
    }
}
