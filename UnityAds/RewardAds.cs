using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardAds : MonoBehaviour, IUnityAdsListener
{
    private string gameID = "4453475";
    private bool testMode = false;
    private Button adsButton;
    private string adsReward_ID = "Rewarded_Android";

    private void Start()
    {
        adsButton = GetComponent<Button>();
        adsButton.interactable = Advertisement.IsReady(adsReward_ID);
        if (adsButton)
        {
            adsButton.onClick.AddListener(ShowLongVideo);
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, testMode);
    }

    private void ShowLongVideo()
    {
        Advertisement.Show(adsReward_ID);
    }

    public void OnUnityAdsReady(string adsID)
    {
        if (adsID == adsReward_ID)
        {
            //Действия, если реклама доступна
            adsButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string adsID, ShowResult showResult) //Обработка рекламы (тут определяем вознаграждение)
    {
        if (showResult == ShowResult.Finished)
        {
            //Действие, если пользователь посмотрел рекламу до конца
            Menu.instance.CoinX2();
        }
        else if (showResult == ShowResult.Skipped)
        {
            //Действие, если пользователь пропустил рекламу
            Menu.instance.ShowLevelMenuAfterGame();
        }
        else if (showResult == ShowResult.Failed)
        {
            //Действие при ошибке
            Menu.instance.ShowLevelMenuAfterGame();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        //Ошибка рекламы
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //Только запустили рекламу
    }
}
