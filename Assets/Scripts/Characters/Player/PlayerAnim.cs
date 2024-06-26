using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem particlesEffect;
    [SerializeField] private GameObject smokePropulsionEffect;
    [SerializeField] private GameObject shield;

    // private bool isDownDashActive = false;
    public bool useNewTouchControls;

    public static PlayerAnim instance;

    void Start() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        useNewTouchControls = PlayerPrefs.GetInt("UseNewTouchControls", 1) == 1;
        SubscribeAndListenEvents();
    }

    private Vector2 startTouchPosition, endTouchPosition;

    void Update() {

        // ================== Keyboard Controls ==============

        if (Input.GetButtonDown("Jump")) {
            anim.SetTrigger("Float Up");
            AudioManager.instance.PlaySoundFx("Air Pressure Release");
            InstantiateSmokePropulsion();
        }

        // if (Input.GetKeyDown(KeyCode.S) && isDownDashActive) {
        //     anim.SetBool("Dash Down", true);
        //     AudioManager.instance.PlaySoundFx("Dash");
        //     Invoke(nameof(StopGoDownAnim), 0.3f);
        // }

        //move to the left
        if (Input.GetKeyDown(KeyCode.A)) {
            anim.SetTrigger("Move Left");
            AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
            InstantiateSmokePropulsion(90, 0.40f);
        }

        //move to the rigth
        if (Input.GetKeyDown(KeyCode.D)) {
            anim.SetTrigger("Move Right");
            AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
            InstantiateSmokePropulsion(-90, 0.40f);
        }

        ReadTouchControls();


    }

    private void ReadTouchControls() {

        // ================== New Touch Controls ==================

        if (useNewTouchControls) {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touchPosition.x < Screen.width / 3 && touch.phase == TouchPhase.Began) {
                    anim.SetTrigger("Move Left");
                    AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
                    InstantiateSmokePropulsion(90, 0.40f);
                }
                else if (touchPosition.x > Screen.width / 3 + Screen.width / 3 && touch.phase == TouchPhase.Began) {
                    anim.SetTrigger("Move Right");
                    AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
                    InstantiateSmokePropulsion(-90, 0.40f);
                }
                else if (touch.phase == TouchPhase.Began) {
                    anim.SetTrigger("Float Up");
                    AudioManager.instance.PlaySoundFx("Air Pressure Release");
                    InstantiateSmokePropulsion();
                }
            }

            return;
        }

        // ================== Old Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.x + 100 < startTouchPosition.x) {
                anim.SetTrigger("Move Left");
                AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
                InstantiateSmokePropulsion(90, 0.40f);
            }
            else if (endTouchPosition.x - 100 > startTouchPosition.x) {
                anim.SetTrigger("Move Right");
                AudioManager.instance.PlaySoundFx("Air Pressure Release 2");
                InstantiateSmokePropulsion(-90, 0.40f);
            }
            // else if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
            //     anim.SetBool("Dash Down", true);
            //     AudioManager.instance.PlaySoundFx("Dash");
            //     Invoke(nameof(StopGoDownAnim), 0.3f);
            // }
            else if (endTouchPosition.y >= startTouchPosition.y) {
                anim.SetTrigger("Float Up");
                AudioManager.instance.PlaySoundFx("Air Pressure Release");
                InstantiateSmokePropulsion();
            }
        }


    }



    private void InstantiateSmokePropulsion(float rotation = 0, float scale = 1f) {

        GameObject smoke = SmokePool.instance.GetObjectFromPool();
        smoke.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, rotation));
        smoke.transform.localScale = new Vector3(scale, scale, scale);
        StartCoroutine(StopSmokeEffect(smoke));

    }

    private IEnumerator StopSmokeEffect(GameObject smoke) {
        yield return new WaitForSeconds(1.5f);
        SmokePool.instance.ReturnObjectToPool(smoke);
    }

    private void StopGoDownAnim() {
        anim.SetBool("Dash Down", false);
    }

    private void SubscribeAndListenEvents() {
        playerHealthManager.OnPlayerGetDamage += PlayerGetDamage;
        playerHealthManager.OnPlayerDied += PlayerDied;
    }

    private void PlayerGetDamage(int _) {
        anim.SetTrigger("Get Damage");
        AudioManager.instance.PlaySoundFx("Hurt");
    }

    // public void ActiveDownDash(int durationInSeconds) {
    //     isDownDashActive = true;
    //     Invoke(nameof(DeactivateDownDash), durationInSeconds);
    //     ActiveParticleEffect(durationInSeconds, new Color(0.7721053f, 0.514151f, 1, 1));
    // }

    // private void DeactivateDownDash() {
    //     isDownDashActive = false;
    // }

    public void ActiveInvincibleEffect(int durationInSeconds) {
        shield.SetActive(true);
        ActiveParticleEffect(durationInSeconds, Color.white);
        Invoke(nameof(ShowShieldOff), durationInSeconds - 3f);
    }

    private void ShowShieldOff() {
        shield.GetComponent<Animator>().SetTrigger("Shield Off");
        Invoke(nameof(DeactivateShield), 3f);
    }

    private void DeactivateShield() {
        shield.SetActive(false);
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

    private void PlayerDied() {
        this.enabled = false;
    }

    public void ResetValues() {
        this.enabled = true;
    }

}
