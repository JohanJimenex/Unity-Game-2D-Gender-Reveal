using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReproduceSoundFx : MonoBehaviour {

    [Tooltip("Nombre del sonido a reproducir cuando se haga click en el botón")]
    [SerializeField] private string soundFxToReproduce;

    private UnityEngine.UI.Button button;

    void Start() {
        button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        AudioManager.instance.PlaySoundFx(soundFxToReproduce);
    }
}
