using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    [SerializeField] private Toggle toggle;

    private void Start() {
        if (gameObject.CompareTag("ToggleMusic")) {
            toggle.SetIsOnWithoutNotify(AudioManager.instance.musicAudioSource.mute);
        }
        else if (gameObject.CompareTag("ToggleSoundFx")) {
            toggle.SetIsOnWithoutNotify(AudioManager.instance.soundsFXAudioSource.mute);
        }
    }

    public void MuteMusic() {
        AudioManager.instance.musicAudioSource.mute = !AudioManager.instance.musicAudioSource.mute;
        toggle.SetIsOnWithoutNotify(AudioManager.instance.musicAudioSource.mute);
    }

    public void MuteSoundFx() {
        AudioManager.instance.musicAudioSource.mute = !AudioManager.instance.soundsFXAudioSource.mute;
        toggle.SetIsOnWithoutNotify(AudioManager.instance.soundsFXAudioSource.mute);
    }

}
