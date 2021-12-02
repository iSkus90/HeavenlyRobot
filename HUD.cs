using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] public GameObject _health1;
    [SerializeField] public GameObject _health2;
    [SerializeField] public GameObject _health3;
    [SerializeField] public Text _textCoins;

    public static HUD instance;

    public static HUD Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
