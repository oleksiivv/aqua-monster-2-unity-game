using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yodo1.MAS;
//using UnityEngine.Advertisements;

public class Shop : MonoBehaviour
{
    public void openScene(int id){
        StartCoroutine(loadAsync(id));
    }

    public GameObject loadingPanel;
    public Slider loadingSlider;

    IEnumerator loadAsync(int id)
    {
        AsyncOperation operation = Application.LoadLevelAsync(id);
        loadingPanel.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;
            Debug.Log(progress);
            yield return null;

        }
    }

    private List<Item> items = new List<Item>();
    public Text[] available; 

    public GameObject audioController;

    public Text money;

    private string appId="4246483";

    void Start()
    {
        //Advertisement.Initialize(appId,false);
        money.text=PlayerPrefs.GetInt("Coin").ToString();

        items.Add(new Item(1,20,"Magnet"));
        items.Add(new Item(2,150,"Acceleration"));
        items.Add(new Item(3,60,"ExtraLife"));
        items.Add(new Item(4,80,"JetBoots"));
        items.Add(new Item(5,100,"JetPack"));





        //PlayerPrefs.SetInt("money",PlayerPrefs.GetInt("money")+3000);
        if (PlayerPrefs.GetInt("!sound") == 0)
        {
            audioController.GetComponent<AudioSource>().mute=false;
        }
        else
        {
            audioController.GetComponent<AudioSource>().mute = true;
        }


        updateItems();

        InitializeSdk();
        SetPrivacy(true, false, false);
        
        InitializeInterstitialAds();

        Yodo1U3dMas.SetInitializeDelegate((bool success, Yodo1U3dAdError error) => { });
        Yodo1U3dMas.InitializeSdk();
    }

    void updateItems(){
        for(int i=0;i<available.Length;i++){
            int n=items[i].getCount();
            if(n==0){
                available[i].gameObject.SetActive(false);
            }
            else{
                available[i].gameObject.SetActive(true);
                available[i].GetComponent<Text>().text=n.ToString();
            }
        }
    }


    public void buyItem(int id){
        id-=1;

        if(PlayerPrefs.GetInt("Coin")<items[id].Price){

             return;

         }

         audioController.GetComponent<AudioSource>().Play();

        PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")-items[id].Price);

        PlayerPrefs.SetInt(items[id].Name,PlayerPrefs.GetInt(items[id].Name)+1);
        money.text=PlayerPrefs.GetInt("Coin").ToString();

        updateItems();


    }



    private Yodo1U3dBannerAdView bannerAdView;

    private void RequestBanner()
    {
        // Clean up banner before reusing
        if (bannerAdView != null)
        {
            bannerAdView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerAdView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.Banner, Yodo1U3dBannerAdPosition.BannerTop | Yodo1U3dBannerAdPosition.BannerHorizontalCenter);
    }

    private void SetPrivacy(bool gdpr, bool coppa, bool ccpa)
    {
        Yodo1U3dMas.SetGDPR(gdpr);
        Yodo1U3dMas.SetCOPPA(coppa);
        Yodo1U3dMas.SetCCPA(ccpa);
    }

    private void InitializeSdk()
    {
        Yodo1U3dMas.InitializeSdk();
    }

    private void InitializeInterstitialAds()
    {
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent +=    
        OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent +=      
        OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent +=      
        OnInterstitialAdErorEvent;
    }

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad opened");
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad closed");
    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] Interstitial ad error - " + adError.ToString());
    }

    public void show(){
        if(Yodo1U3dMas.IsInterstitialAdLoaded()){
            Yodo1U3dMas.ShowInterstitialAd();
        }
        // if(Advertisement.IsReady("video")){
        //     Advertisement.Show("video");
        // }
        // else{
        //     admob.showIntersitionalAd();
        // }
    }


    public void ShowRewardedAd(){
      show();
      PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")+5);
      money.text=PlayerPrefs.GetInt("Coin").ToString();
    }




    /*


    public void ShowRewardedAd()
          {
            if (Advertisement.IsReady())
            {
              var options = new ShowOptions { resultCallback = HandleShowResult };
              Advertisement.Show("Rewarded_Android", options);
            }
          }

          private void HandleShowResult(ShowResult result)
          {
            switch (result)
            {
              case ShowResult.Finished:
              PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")+5);
              money.text=PlayerPrefs.GetInt("Coin").ToString();
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
              case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
              case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
            }
          }

          */

    

}



