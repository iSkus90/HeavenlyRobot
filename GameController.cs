using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int _coins = 0;
    private bool _isGame;
    private int _scoreLevel = 0;
    private bool _tutorComplite;

    public int Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            _coins = value;
        }
    }
    public int Score => _scoreLevel;
    public bool IsGame => _isGame;
    public bool TutorComplite => _tutorComplite;
    public static GameController instance;

    private void Awake()
    {
        instance = this;
        Load();
    }

    public void AddScore(int score)
    {
        _scoreLevel += score;
    }

    public void ScoreX2()
    {
        _scoreLevel *= 2;
    }

    public void AddScoreCoins()
    {
        Coins += _scoreLevel;
        _scoreLevel = 0;
    }

    public void SetCompliteTutor()
    {
        _tutorComplite = true;
    }

    public bool SpendCoins(int price)
    {
        if (price > _coins)
        {
            return false;
        }
        else
        {
            _coins -= price;
            return true;
        }
    }

    public void Save()
    {
        SaveLevel save = new SaveLevel();
        save.tutor = _tutorComplite;
        save.coins = _coins;
        save.levels = Menu.instance.GetLevelOpen();
        PlayerPrefs.SetString("save", JsonUtility.ToJson(save));
    }

    public void Load()
    {
        string s = PlayerPrefs.GetString("save", "null");
        if (s == "null")
        {
            _tutorComplite = false;
            _coins = 0;
            bool[] oLevels = new bool[Menu.instance.levels.Length];
            Menu.instance.SetLevelOpen(oLevels);
        }
        else
        {
            SaveLevel save = new SaveLevel();
            save = JsonUtility.FromJson<SaveLevel>(s);
            _tutorComplite = save.tutor;
            _coins = save.coins;
            Menu.instance.SetLevelOpen(save.levels);
        }
    }

    public void StartGame()
    {
        _isGame = true;
        _scoreLevel = 0;
    }

    public void StopGame()
    {
        _isGame = false;
    }
    
    public void OnApplicationFocus(bool focus)
    {
        Save();
    }
    
}
