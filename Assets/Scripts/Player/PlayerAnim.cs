using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator anim;

    void Start() {
        SubscribeAndListenEvents();
    }

    void Update() {
        if (Input.touchCount > 0 || Input.GetButtonDown("Jump")) {
            anim.SetTrigger("Fly");
        }
    }

    private void SubscribeAndListenEvents() {
        playerController.OnHurt += PlayHurtAnim;
        playerController.OnPlayerDied += PlayDieAnim;
    }

    private void PlayHurtAnim(int lives) {
        anim.SetTrigger("Hurt");
    }

    private void PlayDieAnim() {
        anim.SetTrigger("Die");
    }
}
