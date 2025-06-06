using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class AudioManager : MenuManager
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    private AudioClip[] OstsClips;
    private AudioClip[] SfxsClips;

    private AudioSource[] OstsSource;
    private AudioSource[] SfxsSource;

    private AudioSource currentOsts;
    private AudioSource currentSfxs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        OstsClips = Resources.LoadAll<AudioClip>("Ost");
        SfxsClips = Resources.LoadAll<AudioClip>("Sfx");

        OstsSource = new AudioSource[OstsClips.Length];
        SfxsSource = new AudioSource[SfxsClips.Length];

        for (int i = 0; i < OstsClips.Length; i++)
        {
            OstsSource[i] = gameObject.AddComponent<AudioSource>();
            OstsSource[i].clip = OstsClips[i];
            OstsSource[i].loop = true;
        }
        for (int i = 0; i < SfxsClips.Length; i++)
        {
            SfxsSource[i] = gameObject.AddComponent<AudioSource>();
            SfxsSource[i].clip = SfxsClips[i];
            SfxsSource[i].loop = false;
        }
    }

    public void PlayOst(int index)
    {
        if (currentOsts && currentOsts.isPlaying)
            currentOsts?.Stop();
        currentOsts = OstsSource[index];
        currentOsts.Play();
        Debug.Log($"Playing OST: {currentOsts.name}");
    }

    public void PlaySfx(int index)
    {
        if (currentSfxs && currentSfxs.isPlaying)
            currentSfxs?.Stop();
        currentSfxs = SfxsSource[index];
        currentSfxs.Play();
        Debug.Log($"Playing SFX: {currentSfxs.name}");
    }
}
