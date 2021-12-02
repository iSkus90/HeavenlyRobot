using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class IntersAds : MonoBehaviour
{
    private string gameID="4453475";
    private bool testMode = false;

    private void Start()
    {
        Advertisement.Initialize(gameID, testMode);
    }

    public static void ShowShortADS()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Interstitial_Android");
        }
        else
        {
            Debug.Log("Рекламный ролик не готов");
        }
    }
}
