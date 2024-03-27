using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private Animator anim;

    private bool isDownDashActive = false;


    void Start() {
        SubscribeAndListenEvents();
    }
    private Vector2 startTouchPosition, endTouchPosition;

    void Update() {

        // ================== Keyboard Controls ==============

        if (Input.GetButtonDown("Jump")) {
            anim.SetTrigger("Float Up");
            anim.SetBool("On Ground", false);
        }

        if (Input.GetKeyDown(KeyCode.S) && isDownDashActive) {
            anim.SetBool("Go Down", true);

            Invoke(nameof(StopGoDownAnim), 0.3f);
        }

        // ================== Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
                anim.SetBool("Go Down", true);

                Invoke(nameof(StopGoDownAnim), 0.3f);
            }
            else {
                anim.SetTrigger("Float Up");
                anim.SetBool("On Ground", false);
            }
        }
    }

    private void StopGoDownAnim() {
        anim.SetBool("Go Down", false);
    }

    private void SubscribeAndListenEvents() {
        playerHealthManager.OnPlayerGetDamage += PlayHurtAnim;
        playerHealthManager.OnPlayerDied += PlayDieAnim;
    }

    private void PlayHurtAnim(int lives) {
        anim.SetTrigger("Hurt");
    }

    private void PlayDieAnim() {
        anim.SetTrigger("Die");
    }

    private void OnTriggerEnter2D(Collider2D other) {


        if (other.CompareTag("Ground")) {

            anim.SetBool("On Ground", true);
            // anim.SetTrigger("Land");
        }
        // if (other.CompareTag("Coin")) {
        //     uiAnim.SetTrigger("Collect");
        // }

        // if (other.CompareTag("Enemy")) {
        //     uiAnim.SetTrigger("Scary");
        // }

    }


}
