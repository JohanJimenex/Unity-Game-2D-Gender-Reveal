using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] private AudioSource soundstrackAudioSource;
    [SerializeField] private List<AudioClip> soundstracksAudioClips;
    private List<AudioClip> soundstracksAudioClipsBackUp;

    [SerializeField] private AudioSource soundsFXAudioSource;
    [SerializeField] private List<AudioClip> soundsFxAudioClips;

    [SerializeField] private Dictionary<string, AudioClip> soundsFxAudioClipDictionary;
    [SerializeField] private Dictionary<string, AudioClip> soundtracksAudioClipDictionary;

    void Start() {
        soundstracksAudioClipsBackUp = new List<AudioClip>(soundstracksAudioClips);
        ApplySingletonPattern();
        CreateAudioClipDictionaries();
        SelectRandomMusic();
    }

    private void Update() {
        if (!soundstrackAudioSource.isPlaying) {
            SelectRandomMusic();
        }
    }

    //Singleton Pattern
    public static AudioManager instance;

    private void ApplySingletonPattern() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void CreateAudioClipDictionaries() {
        soundsFxAudioClipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip sountrack in soundstracksAudioClips) {
            soundsFxAudioClipDictionary.Add(sountrack.name, sountrack);
        }

        foreach (AudioClip soundFx in soundsFxAudioClips) {
            soundsFxAudioClipDictionary.Add(soundFx.name, soundFx);
        }
    }

    private void SelectRandomMusic() {
        int randomIndex = Random.Range(0, soundstracksAudioClips.Count);

        PlaySountrack(soundstracksAudioClips[randomIndex].name);

        soundstracksAudioClips.RemoveAt(randomIndex);

        if (soundstracksAudioClips.Count == 0) {
            soundstracksAudioClips = soundstracksAudioClipsBackUp;
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
