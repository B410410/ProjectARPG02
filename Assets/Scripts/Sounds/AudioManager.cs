using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundDB soundDB;

    private AudioSource _bgmAS;
    public AudioSource bgmAS
    {
        get
        {
            if (_bgmAS == null)
            {
                _bgmAS = gameObject.AddComponent<AudioSource>();
            }
            return _bgmAS;
        }
    }

    private AudioSource _sfxAS;
    public AudioSource sfxAS
    {
        get
        {
            if (_sfxAS == null)
            {
                _sfxAS = gameObject.AddComponent<AudioSource>();
            }
            return _sfxAS;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameData.SetAudioManager(this);
    }

    public void PlayBGM(string id, float vol = 0.5f)
    {
        bgmAS.clip = soundDB.SearchBGM(id);
        if (bgmAS.clip != null)
        {
            bgmAS.volume = vol;
            bgmAS.loop = true;
            bgmAS.Play();
        }
        else Debug.LogWarning("BGM Not Found.");
    }

    public void PlaySFX(string id)
    {
        AudioClip clip = soundDB.SearchSFX(id);
        if (clip != null)
        {
            sfxAS.PlayOneShot(clip);
        }
        else Debug.LogWarning("SFX Not Found.");
    }
}
