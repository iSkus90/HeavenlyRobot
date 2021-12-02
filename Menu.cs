using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Menu : MonoBehaviour
{
    [SerializeField] private int _timerStart;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text textCoinMenu;
    [SerializeField] public Text _textScoreWinMenu;
    [SerializeField] public GameObject _coinX2ButtonWin;
    [SerializeField] private GameObject _menuStart;
    [SerializeField] private GameObject _menuWin;
    [SerializeField] private GameObject _menuLose;
    [SerializeField] private GameObject _menuNextLevel;
    [SerializeField] private GameObject _menuPause;
    [SerializeField] private GameObject _menuOptions;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _nextlevelButtonWin;
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private GameObject _tutor;



    [SerializeField] public Level[] levels;

    public static Menu instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        textCoinMenu.text = GameController.instance.Coins.ToString();
        _menuStart.SetActive(true);
    }

    public void SetLevelOpen(bool[] oLevels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if(i<oLevels.Length)
            levels[i].open = oLevels[i];
            if (levels[i].open == false)
            {
                levels[i].buttonImage.color = Color.grey;
            }
        }
    }

    public bool[] GetLevelOpen()
    {
        bool[] oLevels = new bool[levels.Length];
        for (int i = 0; i < levels.Length; i++)
        {
            oLevels[i] = levels[i].open;
        }
        return oLevels;
    }

    public void Exit()
    {
        GameController.instance.Save();
        Application.Quit();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
        _pauseButton.SetActive(true);
        _menuLose.SetActive(false);
        _background.SetActive(false);
        _menuPause.SetActive(false);
        GameController.instance.StartGame();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        _nextlevelButtonWin.SetActive(false);
        _background.SetActive(true);
        _menuWin.SetActive(true);
        _pauseButton.SetActive(false);
        GameController.instance.StopGame();
        StartCoroutine(Timer());
        _textScoreWinMenu.text = GameController.instance.Score.ToString();
    }

    public void LoseGame()
    {
        IntersAds.ShowShortADS();
        GameController.instance.StopGame();
        Pause();
        _background.SetActive(true);
        _menuLose.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void ShowStartMenu()
    {
        _background.SetActive(true);
        _menuStart.SetActive(true);
        _menuNextLevel.SetActive(false);
        _menuPause.SetActive(false);
    }
    public void ShowLevelMenu()
    {
        _menuStart.SetActive(false);
        _menuNextLevel.SetActive(true);
        _menuWin.SetActive(false);
    }

    public void ShowLevelMenuAfterGame()
    {
        GameController.instance.AddScoreCoins();
        textCoinMenu.text = GameController.instance.Coins.ToString();
        ShowLevelMenu();
    }

    public void HideMenu()
    {
        _menuStart.SetActive(false);
        _menuNextLevel.SetActive(false);
        _background.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void PauseMenu()
    {
        Pause();
        _pauseButton.SetActive(false);
        _menuPause.SetActive(true);
    }

    public void inGameButton()
    {
        Resume();
        _pauseButton.SetActive(true);
        _menuPause.SetActive(false);
    }

    public void OptionsOpen()
    {
        _menuOptions.SetActive(true);
        _menuStart.SetActive(false);
    }

    public void OptionsClose()
    {
        _menuOptions.SetActive(false);
        _menuStart.SetActive(true);
    }

    public void CoinX2()
    {
        GameController.instance.ScoreX2();
        ShowLevelMenuAfterGame();
    }

    public void LoadLevel(int number)
    {
        if (number > levels.Length)
        {
            return;
        }

        if (levels[number].open == false)
        {
            if (GameController.instance.SpendCoins(levels[number].price))
            {
                levels[number].open = true;
                levels[number].buttonImage.color = Color.white;
                textCoinMenu.text = GameController.instance.Coins.ToString();
            }
            return;
        }
        HideMenu();
        StartCoroutine(LoadScreen(number));
        GameController.instance.StartGame();
        _timerText.gameObject.SetActive(true);
    }

    IEnumerator Timer()
    {
        int t = _timerStart;
        while (t > 0)
        {
            _timerText.text = t.ToString();
            t--;
            yield return new WaitForSeconds(1f);
        }
        _timerText.gameObject.SetActive(false);
        _nextlevelButtonWin.SetActive(true);
    }

    IEnumerator LoadScreen(int number)
    {
        Pause();
        _loadScreen.SetActive(true);
        SceneManager.LoadScene(levels[number]._sceneName);
        yield return new WaitForSecondsRealtime(1f);
        _loadScreen.SetActive(false);
        if (GameController.instance.TutorComplite == false)
        {
            GameController.instance.SetCompliteTutor();
            _tutor.SetActive(true);
        }
        else
        {
            Resume();
        }
    }
}

[Serializable]
public struct Level
{
    [SerializeField] public UnityEngine.Object scene;
    [SerializeField] public string _sceneName;
    [SerializeField] public int price;
    [SerializeField] public bool open;
    [SerializeField] public Image buttonImage;
}
