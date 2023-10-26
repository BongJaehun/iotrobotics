using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public AudioClip Item_plus;
    public AudioClip Item_minus;
    public AudioClip Item_invincibility;
    public AudioClip Item_Inverse;
    public AudioClip Die;
    public AudioClip Finish;

    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudioPlay(string ID)
    {
        switch (ID)
        {
            case "+5kg":
                audioSource.clip = Item_plus;
                break;
            case "-5kg":
                audioSource.clip = Item_minus;
                break;
            case "Invincibility":
                audioSource.clip = Item_invincibility;
                break;
            case "Inverse":
                audioSource.clip = Item_Inverse;
                break;
            case "Die":
                audioSource.clip = Die;
                break;
            case "Finish":
                audioSource.clip = Finish;
                break;
        }
        audioSource.Play();
    }
}
