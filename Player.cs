using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] public AudioSource _audioSource;
    [SerializeField] public AudioClip _audioJump;
    [SerializeField] public AudioClip _audioDamage;
    [SerializeField] public AudioClip _audioDamageAsteroid;
    private int _health = 3;

    public static Player instance;

    public static Player Instance
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coins")
        {
            GameController.instance.AddScore(1);
            other.gameObject.SetActive(false);
            HUD.instance._textCoins.text = GameController.instance.Score.ToString();
        }

        if (other.gameObject.tag == "LoseGame")
        {
            Menu.instance.LoseGame();
        }
    }

    public void Damage()
    {
        _health--;
        switch (_health)
        {
            case 2:
                HUD.instance._health1.SetActive(false);
                break;
            case 1:
                HUD.instance._health2.SetActive(false);
                break;
            default:
                Menu.instance.LoseGame();
                break;
        }        
    }
}
