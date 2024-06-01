using UnityEngine;

public class ReviewManager : MonoBehaviour {

    public void RateGame() {
        #if UNITY_ANDROID
            Application.OpenURL("market://details?id=com.jopamstudio.nolansgalaxy");
        #elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/id6503222887");
        #endif
    }

}
