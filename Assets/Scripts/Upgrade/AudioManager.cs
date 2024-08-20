using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSFX;
    public AudioSource audioMusic;
    public AudioClip backround;
    public AudioClip selectItem;
    public AudioClip dropItem;
    public AudioClip addExp;
    public AudioClip erroItem;


    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
    }
    public void Start()
    {
        audioMusic.clip = backround;
        audioMusic.Play();
    }
    public void AddExp()
    {

        audioSFX.volume = 0.2f;
        audioSFX.clip = addExp;
        audioSFX.Play();
    }
    public void SelectItem()
    {
        audioSFX.volume = 1f;
        audioSFX.clip = selectItem;
        audioSFX.Play();

    }
    public void DropItem()
    {
        audioSFX.volume = 1f;
        audioSFX.clip=dropItem;
        audioSFX.Play();
    }
    public void ErroItem()
    {
        audioSFX.volume = 1f;
        audioSFX.clip = erroItem;
        audioSFX.Play();
    }

}
