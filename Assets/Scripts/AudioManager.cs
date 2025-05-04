using System;
using TMPro;
using UnityEngine;

public class AudioManager : MenuManager
{
    [SerializeField] private AudioSource currenctAudioSource;
    [SerializeField] private AudioSource[] audioSourceArray;
    public TextMeshProUGUI TextMeshProUGUI;

    public void Awake()
    {
        currenctAudioSource = (audioSourceArray[0]);
        SetCurrenctAudioSource(audioSourceArray[1]);
        TextMeshProUGUI.text = (currenctAudioSource.volume * 100).ToString("000");
    }

    public void OnValueChangedAudioSlider(float value)
    {
        currenctAudioSource.volume = (float)Math.Round(value, 2);
        TextMeshProUGUI.text = (currenctAudioSource.volume * 100).ToString("000");
    }

    public void SetCurrenctAudioSource(AudioSource audioSource)
    {
        float audioVolume = (audioSource != null) ? currenctAudioSource.volume : 1;
        currenctAudioSource = audioSource;
        audioSource.volume = audioVolume;

        currenctAudioSource.Play();
    }
}
