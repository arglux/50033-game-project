using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource rifleSound;
    public AudioSource shotgunSound;
    public AudioSource sniperSound;
    public AudioSource machinegunSound;
    public AudioSource ouchSound;
    public AudioSource dieSound;
    public AudioSource emptySound;
    public AudioSource rifleReloadSound;
    public AudioSource shotgunReloadSound;
    public AudioSource sniperReloadSound;
    public AudioSource machinegunReloadSound;
    public AudioSource casingSound;
    
    public static AudioManager SharedInstance;

    void Start()
    {
        AudioManager.SharedInstance = this;    
    }

    public AudioSource Play(AudioSource source, bool oneShot = true)
    {
        if (oneShot)
        {
            source.PlayOneShot(source.clip);
            return null;
        }
        else
        {
            source.Play();
            return source;
        }
    }
}
