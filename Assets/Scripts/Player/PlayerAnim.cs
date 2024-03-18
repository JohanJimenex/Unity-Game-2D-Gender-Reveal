using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    [SerializeField] private PlayerHealthManager playerHealthManager;
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
        playerHealthManager.OnHurt += PlayHurtAnim;
        playerHealthManager.OnPlayerDied += PlayDieAnim;
    }

    private void PlayHurtAnim(int lives) {
        anim.SetTrigger("Hurt");
    }

    private void PlayDieAnim() {
        anim.SetTrigger("Die");
    }
}
