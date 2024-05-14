using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] private AudioSource soundstrackAudioSource;
    [SerializeField] private List<AudioClip> soundstracksAudioClips;

    [SerializeField] private AudioSource soundsFXAudioSource;
    [SerializeField] private List<AudioClip> soundsFxAudioClips;

    [SerializeField] private Dictionary<string, AudioClip> soundsFxAudioClipDictionary;
    [SerializeField] private Dictionary<string, AudioClip> soundtracksAudioClipDictionary;

    //Singleton Pattern
    public static AudioManager instance;

    void Start() {
        ApplySingletonPattern();
        CreateAudioClipDictionary();
    }

    private void ApplySingletonPattern() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void CreateAudioClipDictionary() {
        soundsFxAudioClipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip sountrack in soundstracksAudioClips) {
            soundsFxAudioClipDictionary.Add(sountrack.name, sountrack);
        }

        foreach (AudioClip soundFx in soundsFxAudioClips) {
            soundsFxAudioClipDictionary.Add(soundFx.name, soundFx);
        }
    }

    public void PlaySountrack(string name) {
        soundstrackAudioSource.clip = soundsFxAudioClipDictionary[name];
        soundstrackAudioSource.Play();
    }

    public void PlaySoundFx(string name) {
        soundsFXAudioSource.PlayOneShot(soundsFxAudioClipDictionary[name]);
    }


}
