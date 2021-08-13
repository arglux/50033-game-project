using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioClip fistBGMClip; // 오디오 소스들 지정.
    [SerializeField] AudioClip[] audioClip; // 오디오 소스들 지정.
    Dictionary<string, AudioClip> audioClipsDic;
    AudioSource sfxPlayer;
    AudioSource bgmPlayer;
    AudioSource newbgmPlayer;
    AudioSource myAudio;
    AudioSource playMaster;
    AudioClip[] audioClips;
    public static AudioManager instance;
    private GameObject child;
    private bool IsfirstBGNPlaying;

    void Start()
    {
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            AwakeAfter();
        }
    }
    void AwakeAfter()
    {
        child = new GameObject("BGM");
        child.transform.SetParent(transform);
        bgmPlayer = child.AddComponent<AudioSource>();
        newbgmPlayer = child.AddComponent<AudioSource>();
        sfxPlayer = child.AddComponent<AudioSource>();
        playMaster = child.AddComponent<AudioSource>();
        bgmPlayer.clip = fistBGMClip;
        bgmPlayer.loop = true;
        bgmPlayer.Play();
        IsfirstBGNPlaying = true;

        audioClipsDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in audioClip)
        {
            // Debug.Log("foreach()"+a.name);
            audioClipsDic.Add(a.name, a);
        }
    }


    public void StartBGM(AudioClip BGMClip)
    {
        StartCoroutine(SetupBGM(BGMClip));
    }
    public void StopBGM()
    {
        StartCoroutine(StoppingBGM());
    }

    public IEnumerator SetupBGM(AudioClip BGMClip)
    {
        newbgmPlayer.loop = true;
        if (IsfirstBGNPlaying)
        {
            newbgmPlayer.clip = BGMClip;
            newbgmPlayer.Play();
            newbgmPlayer.volume = 0.1f;
            float startVolume = newbgmPlayer.volume;

            while (newbgmPlayer.volume < 1.0f)
            {
                newbgmPlayer.volume += startVolume * Time.deltaTime / 1.5f;
                yield return null;
            }
        }
        else
        {
            bgmPlayer.clip = BGMClip;
            bgmPlayer.Play();
            bgmPlayer.volume = 0.1f;
            float startVolume = bgmPlayer.volume;

            while (bgmPlayer.volume < 1.0f)
            {
                bgmPlayer.volume += startVolume * Time.deltaTime / 1.5f;
                yield return null;
            }
        }

    }

    public IEnumerator StoppingBGM()
    {
        if (IsfirstBGNPlaying)
        {
            float startVolume = bgmPlayer.volume;

            while (bgmPlayer.volume > 0.0f)
            {
                bgmPlayer.volume -= startVolume * Time.deltaTime / 1.5f;
                yield return null;
            }
            bgmPlayer.Stop();
            bgmPlayer.volume = startVolume;
            IsfirstBGNPlaying = false;
        }
        else
        {
            float startVolume = newbgmPlayer.volume;

            while (newbgmPlayer.volume > 0.0f)
            {
                newbgmPlayer.volume -= startVolume * Time.deltaTime / 1.5f;
                yield return null;
            }
            newbgmPlayer.Stop();
            newbgmPlayer.volume = startVolume;
        }

    }

    public void PlaySound(string a_name)
    {
        if (audioClipsDic.ContainsKey(a_name) == false)
        {
            // Debug.Log(a_name + " is not Contained audioClipsDic");
            return;
        }
        // Debug.Log("Playing : "+a_name);
        sfxPlayer.PlayOneShot(audioClipsDic[a_name]);
    }





    public void Play(AudioClip clip)
    {
        playMaster.PlayOneShot(clip);
    }


    public void PlayAudioClip(int num)
    {
        //// Debug.Log("sound checking "+audioClips[num]);
        myAudio.PlayOneShot(audioClips[num], 1);
    }

    public void PlayAudioClip(int num, float volume)
    {
        //// Debug.Log("sound checking "+audioClips[num]);
        myAudio.PlayOneShot(audioClips[num], volume);
    }

    public IEnumerator BGMFadeOut(AudioSource source, AudioClip clip)
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0;

        source.clip = clip;

        while (timeElapsed < timeToFade)
        {
            source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        source.Stop();
    }

    public IEnumerator BGMFadeIn(AudioSource source, AudioClip clip)
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0;

        source.clip = clip;

        source.Play();

        while (timeElapsed < timeToFade)
        {
            source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
