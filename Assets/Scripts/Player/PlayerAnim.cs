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
            anim.SetBool("Dash Down", true);
            Debug.Log("Dash Down");
            Invoke(nameof(StopGoDownAnim), 0.3f);
        }

        //move to the left
        if (Input.GetKeyDown(KeyCode.A)) {
            anim.SetTrigger("Move Left");
        }

        //move to the rigth
        if (Input.GetKeyDown(KeyCode.D)) {
            anim.SetTrigger("Move Right");
        }


        // ================== Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
                anim.SetBool("Dash Down", true);
                Invoke(nameof(StopGoDownAnim), 0.3f);
            }
            else if (endTouchPosition.y >= startTouchPosition.y) {
                anim.SetTrigger("Float Up");
                anim.SetBool("On Ground", false);
            }

            if (endTouchPosition.x + 100 < startTouchPosition.x) {
                anim.SetTrigger("Move Left");
            }
            else if (endTouchPosition.x - 100 > startTouchPosition.x) {
                anim.SetTrigger("Move Right");
            }

        }
    }

    private void StopGoDownAnim() {
        anim.SetBool("Dash Down", false);
    }

    private void SubscribeAndListenEvents() {
        playerHealthManager.OnPlayerGetDamage += PlayerGetDamage;
        playerHealthManager.OnPlayerDied += PlayDieAnim;
    }

    private void PlayerGetDamage(int _) {
        anim.SetTrigger("Get Damage");
    }

    private void PlayDieAnim() {
        anim.SetTrigger("Die");
    }

    public void ActiveDownDash(int durationInSeconds) {
        isDownDashActive = true;
        Invoke(nameof(DeactivateDownDash), durationInSeconds);
    }

    private void DeactivateDownDash() {
        isDownDashActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        // if (other.CompareTag("Enemy")) {
        //     uiAnim.SetTrigger("Scary");
        // }

        // if (other.CompareTag("Coin")) {
        //     uiAnim.SetTrigger("Collect");
        // }

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            anim.SetBool("On Ground", true);
        }
    }

}
