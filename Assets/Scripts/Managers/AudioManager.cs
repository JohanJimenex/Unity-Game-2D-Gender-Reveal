using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource musicAudioSource;
    public AudioSource soundsFXAudioSource;

    [SerializeField] private List<AudioClip> musicsAudioClips;
    [SerializeField] private List<AudioClip> soundsFxAudioClips;

    private Dictionary<string, AudioClip> musicsAudioClipDictionary;
    private Dictionary<string, AudioClip> soundsFxAudioClipDictionary;
    private List<AudioClip> musicsAudioClipsBackUp;

    public static AudioManager instance;

    private void Awake() {
        ApplySingletonPattern();
    }

    void Start() {
        musicsAudioClipsBackUp = new List<AudioClip>(musicsAudioClips);
        CreateAudioClipDictionaries();
    }

    private void Update() {
        if (!musicAudioSource.isPlaying && musicAudioSource.mute == false) {
            SelectRandomMusic();
        }
    }

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
        musicsAudioClipDictionary = new Dictionary<string, AudioClip>();
        soundsFxAudioClipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip sountrack in musicsAudioClips) {
            musicsAudioClipDictionary.Add(sountrack.name, sountrack);
        }

        foreach (AudioClip soundFx in soundsFxAudioClips) {
            soundsFxAudioClipDictionary.Add(soundFx.name, soundFx);
        }

    }

    private void SelectRandomMusic() {
        int randomIndex = Random.Range(0, musicsAudioClips.Count);

        PlaySountrack(musicsAudioClips[randomIndex].name);

        musicsAudioClips.RemoveAt(randomIndex);

        if (musicsAudioClips.Count == 0) {
            musicsAudioClips = musicsAudioClipsBackUp;
        }
    }

    public void PlaySoundFx(string name) {
        soundsFXAudioSource.PlayOneShot(soundsFxAudioClipDictionary[name]);
    }

    public void PlaySountrack(string name) {
        musicAudioSource.clip = musicsAudioClipDictionary[name];
        musicAudioSource.Play();
        UIManager.instance.ShowMusicPlayerUI(name);
    }

}
