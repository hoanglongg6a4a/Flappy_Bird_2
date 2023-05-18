using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    [SerializeField]
    private Button instuctionButton;
    [SerializeField]
    private Text scoreText, endScoreText, bestScoreText;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Image goldMedal, silverMedal, bronzeMedal;

    void Awake()
    {
        Time.timeScale = 0;
        _MakeInstance();
    }
    void _MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    public void InstructionButton()
    {
        Time.timeScale = 1;
        instuctionButton.gameObject.SetActive(false);
    }
    public void _SetScore(int score)
    {
        scoreText.text = ""+score;
    }

    public void BirdDiedShowPanel(int score)
    {
        gameOverPanel.SetActive(true);
        endScoreText.text = "" + score;
        if(score > GameManager.instance.GetHighScore())
        {
            GameManager.instance.SetHighScore(score);
        }
        bestScoreText.text = "" + GameManager.instance.GetHighScore();
    }
    public void ShowMedal(int score)
    {
        if (score <= 5)
        {
            bronzeMedal.gameObject.SetActive(true);
            silverMedal.gameObject.SetActive(false);
            goldMedal.gameObject.SetActive(false);
        }
        else if (score > 5 && score <= 10)
        {
            silverMedal.gameObject.SetActive(true);
            bronzeMedal.gameObject.SetActive(false);
            goldMedal.gameObject.SetActive(false);
        }
        else if (score > 10)
        {
            goldMedal.gameObject.SetActive(true);
            silverMedal.gameObject.SetActive(false);
            bronzeMedal.gameObject.SetActive(false);
        }
    }
    public void _MenuButton()
    {
       // Application.LoadLevel("MainMenu");
        SceneManager.LoadScene("GameMenu");
    }
    public void _RestartGameButton()
    {
        //  Application.LoadLevel("GamePlay");
        SceneManager.LoadScene("GamePlay");
    }
}
