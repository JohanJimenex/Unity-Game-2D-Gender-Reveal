using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] private AudioSource soundstrackAudioSource;
    [SerializeField] private AudioSource soundsFXAudioSource;

    [SerializeField] private List<AudioClip> soundstracksAudioClips;
    [SerializeField] private List<AudioClip> soundsFxAudioClips;

    private Dictionary<string, AudioClip> soundtracksAudioClipDictionary;
    private Dictionary<string, AudioClip> soundsFxAudioClipDictionary;

    private List<AudioClip> soundstracksAudioClipsBackUp;

    private void Awake() {
        ApplySingletonPattern();
    }

    void Start() {
        soundstracksAudioClipsBackUp = new List<AudioClip>(soundstracksAudioClips);
        CreateAudioClipDictionaries();
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
        soundtracksAudioClipDictionary = new Dictionary<string, AudioClip>();
        soundsFxAudioClipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip sountrack in soundstracksAudioClips) {
            soundtracksAudioClipDictionary.Add(sountrack.name, sountrack);
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

    public void PlaySoundFx(string name) {
        soundsFXAudioSource.PlayOneShot(soundsFxAudioClipDictionary[name]);
    }

    public void PlaySountrack(string name) {
        soundstrackAudioSource.clip = soundtracksAudioClipDictionary[name];
        soundstrackAudioSource.Play();
        UIManager.instance.ShowMusicPlayerUI(name);
    }

}
