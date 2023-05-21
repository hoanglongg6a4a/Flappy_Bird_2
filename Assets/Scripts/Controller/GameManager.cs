using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const string HIGH_SCORE = "High Score";
    private const string Bird = "Chose Bird";
    void Awake()
    {
        MakeSingleInstance();
        //IsGameStartedForTheFirstTime();
        PlayerPrefs.SetInt(HIGH_SCORE, 0);
    }
    void IsGameStartedForTheFirstTime()
    {
        if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime")){
             PlayerPrefs.SetInt(HIGH_SCORE, 0);
            PlayerPrefs.SetInt("IsGameStartedForTheFirstTime",0);
        }
    }
    void MakeSingleInstance()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }  
    }
    public void SetBird(string bird)
    {
        PlayerPrefs.SetString(Bird, bird);
    }
    public string SetBird()
    {
        return PlayerPrefs.GetString(Bird);
    }
    public void SetHighScore(int score)
    {
        PlayerPrefs.SetInt(HIGH_SCORE, score);
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt (HIGH_SCORE);
    }
}
