using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem particlesEffect;
    [SerializeField] private GameObject smokePropulsionEffect;

    private bool isDownDashActive = false;

    void Start() {
        SubscribeAndListenEvents();
    }

    private Vector2 startTouchPosition, endTouchPosition;

    void Update() {

        // ================== Keyboard Controls ==============

        if (Input.GetButtonDown("Jump")) {
            anim.SetTrigger("Float Up");
            AudioManager.instance.PlaySoundFx("Air Pressure Release");

            InstanciateSmokePropulsion();
        }

        if (Input.GetKeyDown(KeyCode.S) && isDownDashActive) {
            anim.SetBool("Dash Down", true);
            AudioManager.instance.PlaySoundFx("Dash");
            Invoke(nameof(StopGoDownAnim), 0.3f);
        }

        //move to the left
        if (Input.GetKeyDown(KeyCode.A)) {
            anim.SetTrigger("Move Left");
            AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
            InstanciateSmokePropulsion(90, 0.40f);
        }

        //move to the rigth
        if (Input.GetKeyDown(KeyCode.D)) {
            anim.SetTrigger("Move Right");
            AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
            InstanciateSmokePropulsion(-90, 0.40f);
        }

        // ================== Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.x + 100 < startTouchPosition.x) {
                anim.SetTrigger("Move Left");
                AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
                InstanciateSmokePropulsion(90, 0.40f);
            }
            else if (endTouchPosition.x - 100 > startTouchPosition.x) {
                anim.SetTrigger("Move Right");
                AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
                InstanciateSmokePropulsion(-90, 0.40f);
            }
            else if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
                anim.SetBool("Dash Down", true);
                AudioManager.instance.PlaySoundFx("Dash");
                Invoke(nameof(StopGoDownAnim), 0.3f);
            }
            else if (endTouchPosition.y >= startTouchPosition.y) {
                anim.SetTrigger("Float Up");
                AudioManager.instance.PlaySoundFx("Air Pressure Release");
                InstanciateSmokePropulsion();
            }

        }
    }

    private void InstanciateSmokePropulsion(float rotation = 0, float scale = 1f) {
        GameObject smoke = Instantiate(smokePropulsionEffect, transform.position, Quaternion.identity);
        smoke.transform.rotation = Quaternion.Euler(0, 0, rotation);
        smoke.transform.localScale = new Vector3(scale, scale, scale);
        Destroy(smoke, 3f);
    }

    private void StopGoDownAnim() {
        anim.SetBool("Dash Down", false);
    }

    private void SubscribeAndListenEvents() {
        playerHealthManager.OnPlayerGetDamage += PlayerGetDamage;
        // playerHealthManager.OnPlayerDied += PlayDieAnim;
    }

    private void PlayerGetDamage(int _) {
        anim.SetTrigger("Get Damage");
        AudioManager.instance.PlaySoundFx("Hurt");
    }

    public void ActiveDownDash(int durationInSeconds) {
        isDownDashActive = true;
        Invoke(nameof(DeactivateDownDash), durationInSeconds);
        ActiveParticleEffect(durationInSeconds, new Color(0.7721053f, 0.514151f, 1, 1));
    }

    private void DeactivateDownDash() {
        isDownDashActive = false;
    }

    public void ActiveInvincibleEffect(int durationInSeconds) {
        spriteRenderer.material.color = Color.yellow;
        ActiveParticleEffect(durationInSeconds, Color.yellow);
        Invoke(nameof(DeactivateInvincibleEffect), durationInSeconds);
    }

    private void DeactivateInvincibleEffect() {
        spriteRenderer.material.color = Color.white;
    }

    private void ActiveParticleEffect(int durationInSeconds, Color? color = null) {
        var main = particlesEffect.main;
        main.startColor = color ?? Color.white;
        particlesEffect.Play();
        Invoke(nameof(DeactivateParticleEffect), durationInSeconds);
    }

    private void DeactivateParticleEffect() {
        particlesEffect.Stop();
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
            AudioManager.instance.PlaySoundFx("Player Landing");
            anim.SetTrigger("Player Landing");
        }
    }



}
