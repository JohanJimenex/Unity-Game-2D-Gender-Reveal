using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    [SerializeField] private Toggle toggle;

    private void Start() {
        if (gameObject.CompareTag("ToggleControls")) {
            bool changeControls = PlayerPrefs.GetInt("UseNewTouchControls", 1) == 1;
            toggle.SetIsOnWithoutNotify(changeControls);
        }
    }

    public void ChangeControls() {

        if (PlayerMovement.instance != null && PlayerAnim.instance != null) {
            PlayerMovement.instance.useNewTouchControls = !PlayerMovement.instance.useNewTouchControls;
            PlayerAnim.instance.useNewTouchControls = !PlayerAnim.instance.useNewTouchControls;
        }
        Debug.Log(toggle.isOn);
        toggle.SetIsOnWithoutNotify(toggle.isOn);
        PlayerPrefs.SetInt("UseNewTouchControls", toggle.isOn ? 1 : 0);
    }

    public void MuteMusic() {
        AudioManager.instance.musicAudioSource.mute = !AudioManager.instance.musicAudioSource.mute;
        toggle.SetIsOnWithoutNotify(AudioManager.instance.musicAudioSource.mute);
        PlayerPrefs.SetInt("MuteMusic", AudioManager.instance.musicAudioSource.mute ? 1 : 0);

    }

    public void MuteSoundFx() {
        AudioManager.instance.soundsFXAudioSource.mute = !AudioManager.instance.soundsFXAudioSource.mute;
        toggle.SetIsOnWithoutNotify(AudioManager.instance.soundsFXAudioSource.mute);
        PlayerPrefs.SetInt("MuteSoundFx", AudioManager.instance.soundsFXAudioSource.mute ? 1 : 0);
    }

}
