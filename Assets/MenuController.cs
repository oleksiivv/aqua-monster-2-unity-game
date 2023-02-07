using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yodo1.MAS;
//using UnityEngine.Advertisements;
public class MenuController : MonoBehaviour
{
    public Text starsTotal;
    public Text money;

    public GameObject levelsPanel;
    private string appId="4246483";



    public GameObject[] panels;
    public int storyLen=3;

    public GameObject plane;


    /*
      After your home was destroyed by bomb, which has fallen into the lake you was living in, you need to find new one.

      But other lakes are far away. So you need to jump between aquariums in your way. 
      There are also items llike coins, magnets, jetpacks etc in your way. And you need to collect stars to open new levels. 
      You can buy useful items in shop by collected coins too.


      And remember that aquariums with different colors have different featuresfeatures, 
      meaning that them are not as easy as it can seems.
    */

    void Start()
    {
      
      //PlayerPrefs.SetInt("Completed9",1);

        if(PlayerPrefs.GetInt("first")==0){
            PlayerPrefs.SetInt("first",1);
            PlayerPrefs.SetInt("Magnet",2);
            PlayerPrefs.SetInt("Acceleration",4);
            PlayerPrefs.SetInt("ExtraLife",1);
            PlayerPrefs.SetInt("JetBoots",2);
            PlayerPrefs.SetInt("JetPack",1);
        }
        //Advertisement.Initialize(appId,false);
        starsTotal.text=PlayerPrefs.GetInt("AllStars").ToString();
        money.text=PlayerPrefs.GetInt("Coin").ToString();

        if(PlayerPrefs.GetInt("storyShowed",0)==0){
          storyLine=false;
          nextPanel(-1);
          PlayerPrefs.SetInt("storyShowed",-1);
        }

        InitializeSdk();
        SetPrivacy(true, false, false);
        
        InitializeInterstitialAds();

        Yodo1U3dMas.SetInitializeDelegate((bool success, Yodo1U3dAdError error) => { });
        Yodo1U3dMas.InitializeSdk();
        
        // PlayerPrefs.SetInt("Completed0",1);
        // PlayerPrefs.SetInt("Completed1",1);
        // PlayerPrefs.SetInt("Completed2",1);
        // PlayerPrefs.SetInt("Completed3",1);
        // PlayerPrefs.SetInt("Completed4",1);
        // PlayerPrefs.SetInt("Completed5",1);
        // PlayerPrefs.SetInt("Completed6",1);
        // PlayerPrefs.SetInt("Completed7",1);
    }
    bool storyLine=true;
    public void nextPanel(int currentPanel){

      foreach(var panel in panels){
        panel.SetActive(false);
      }

      if(storyLine && currentPanel+1==storyLen){

        return;

      }
      
      if(currentPanel+1!=panels.Length){
        panels[currentPanel+1].SetActive(true);
      }

    }

    public void startStoryPanel(){
      storyLine=true;
      panels[0].SetActive(true);
    }

    public void showInfoPanel(){
      panels[storyLen].SetActive(true);

    }

    // Update is called once per frame
    public void OpenScene(int id){
      if(id<9){
        if((id==1 || (PlayerPrefs.GetInt("Completed"+(id-1).ToString())==1) || PlayerPrefs.GetInt("Completed"+(id).ToString())==1)){
            StartCoroutine(loadAsync(id));
        }
      }
      else{
        if((id==9 &&  PlayerPrefs.GetInt("AllStars")>=16|| (PlayerPrefs.GetInt("Completed"+(id-1).ToString())==1) || PlayerPrefs.GetInt("Completed"+(id).ToString())==1)){
            StartCoroutine(loadAsync(id));
        }
      }
    }
    

    public GameObject bgLevelsPanel;
    public StudyControllerMenu study;

    public void OpenLevelPanel(){
        if(!study.studied()){
          study.StartStudy();
        }
        else{


          Invoke("secondPartOn",1f);

          levelsPanel.SetActive(true);
          bgLevelsPanel.SetActive(true);
        }

    }

    public void openSimpleScene(int id){
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


    
    public GameObject secondPartLevels;
    public void nextLevelsPanel(){
      levelsPanel.GetComponent<Animator>().SetBool("open",false);
      levelsPanel.GetComponent<Animator>().SetBool("close",true);
      Invoke("secondPartOn",0.5f);

    }

    public void prevLevelsPanel(){
      levelsPanel.GetComponent<Animator>().SetBool("close",false);
      levelsPanel.GetComponent<Animator>().SetBool("open",true);


    }

    void secondPartOn(){
      //levelsPanel.GetComponent<Animator>().SetBool("close",false);
      secondPartLevels.SetActive(true);
    }

    public void levelsClose(){
      levelsPanel.SetActive(false);
      secondPartLevels.SetActive(false);

      bgLevelsPanel.SetActive(false);
    }


    public void rateGame(){
      Application.OpenURL("https://play.google.com/store/apps/details?id=com.VertexStudioGame.Aqua");
    }






}
