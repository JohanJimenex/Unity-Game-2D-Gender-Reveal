using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {

    [SerializeField] private bool _testMode = false;

    private readonly string iOsGameId = "5625360";
    private readonly string androidGameId = "5625361";

    private readonly string iOsBanner = "Banner_iOS";
    private readonly string androidBanner = "Banner_Android";

    private readonly string iOsInterticial = "Interstitial_iOS";
    private readonly string androidInterticial = "Interstitial_Android";

    private readonly string iOsRewarded = "Rewarded_iOS";
    private readonly string androidRewarded = "Rewarded_Android";

    private string gameIdToUse;
    private string idBannerToUse;
    private string idInterticialToUse;
    private string idRewardedToUse;

    public static UnityAds instance;

    void Awake() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        IdentifyPlatform();
    }

    //Inicializar anuncios
    private void IdentifyPlatform() {

        Debug.Log("Plataforma: " + Application.platform);

        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            gameIdToUse = iOsGameId;
            idBannerToUse = iOsBanner;
            idInterticialToUse = iOsInterticial;
            idRewardedToUse = iOsRewarded;
        }
        else {
            gameIdToUse = androidGameId;
            idBannerToUse = androidBanner;
            idInterticialToUse = androidInterticial;
            idRewardedToUse = androidRewarded;
        }

        if (Advertisement.isInitialized) {
            LoadBannerAd();
            LoadInterticialAd();
            LoadRewardedAd();
        }
        else {
            Advertisement.Initialize(gameIdToUse, _testMode, this);
        }
    }

    //Cuando se complete la inicializacion de los servicios de anuncios este metodo se llama solo:
    public void OnInitializationComplete() {
        Debug.Log("Se inicializaron los anuncios");
        LoadBannerAd();
        LoadInterticialAd();
        LoadRewardedAd();
    }

    // Cargar Banner
    private void LoadBannerAd() {
        //Posicion del banner
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(idBannerToUse);

        ShowBannerAd();
    }

    //Mostrar Banner
    private void ShowBannerAd() {

        if (Advertisement.Banner.isLoaded) {
            Advertisement.Banner.Show(idBannerToUse);
        }
        else {
            StartCoroutine(TryLoadBanner());
        }
    }

    private IEnumerator TryLoadBanner() {
        yield return new WaitForSeconds(1);
        ShowBannerAd();
    }

    //Cargar interticial
    private void LoadInterticialAd() {
        Advertisement.Load(idInterticialToUse, this);
    }

    //Mostrar interticial se llama desde el boton Aceptar de los resultados
    public void ShowInterticialAd() {
        Advertisement.Show(idInterticialToUse);
    }

    //Cargar recompensado
    private void LoadRewardedAd() {
        Advertisement.Load(idRewardedToUse, this);
        Debug.Log("Cargando recompensado");
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId) {
        Debug.Log("Anuncio cargado: " + adUnitId);

        if (adUnitId.Equals(idRewardedToUse)) {
            // Configure the button to call the ShowAd() method when clicked:
            Debug.Log("Habilitamos el boton porque es el reward y escuchamos el evento de click");
            UIManager.instance.ShowRewardedButton();
        }
    }

    //Mostrar recompensado
    public void ShowRewardedAd() {
        Debug.Log("Mostrando recompensado");
        Advertisement.Show(idRewardedToUse, this);
    }


    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        if (adUnitId.Equals(idRewardedToUse) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            Debug.Log("Unity Ads Rewarded Ad Completed klk");
            PlayerAnim.instance.ResetValues();
            PlayerMovement.instance.ResetValues();
            PlayerHealthManager.instance.RevivePlayer();
            CameraController.instance.playerIsDead = false;
        }
    }

    //Estos metodos son implementados por la interfazes
    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {
        // throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId) {
        // throw new System.NotImplementedException();
    }


}


