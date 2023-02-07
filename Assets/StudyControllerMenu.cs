using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyControllerMenu : MonoBehaviour
{
    public GameObject studyPanel;
    public MenuController menu;

    public void StartStudy(){
        if(PlayerPrefs.GetInt("studied",0)==0){
            studyPanel.SetActive(true);
        }
        else{
            studyPanel.SetActive(false);
        }
    }

    public bool studied(){
        return PlayerPrefs.GetInt("studied",0)!=0;
    }

    public void startStudy(int scene){
        PlayerPrefs.SetInt("studied",1);
        StartCoroutine(loadAsync(scene));
    }

    public void skipStudy(){

        PlayerPrefs.SetInt("studied",1);
        studyPanel.SetActive(false);

        menu.OpenLevelPanel();
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
}
