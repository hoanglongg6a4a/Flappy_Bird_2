using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const string HIGH_SCORE = "High Score";
    void Awake()
    {
        MakeSingleInstance();
        PlayerPrefs.SetInt(HIGH_SCORE, 0);
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
    public void SetHighScore(int score)
    {
        PlayerPrefs.SetInt(HIGH_SCORE, score);
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt (HIGH_SCORE);
    }
}
