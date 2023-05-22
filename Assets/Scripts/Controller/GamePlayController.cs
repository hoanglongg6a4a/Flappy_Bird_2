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
    private GameObject[] medals;
    void Awake()
    {
        Time.timeScale = 0;
        MakeInstance();
    }
    void MakeInstance()
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
    public void SetScore(int score)
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
        medals = new GameObject[] { bronzeMedal.gameObject, silverMedal.gameObject, goldMedal.gameObject };
        foreach (GameObject medal in medals)
        {
            medal.gameObject.SetActive(false);
        }

        if (score <= 5)
        {
            medals[0].gameObject.SetActive(true); 
        }
        else if (score > 5 && score <= 10)
        {
            medals[1].gameObject.SetActive(true); 
        }
        else if (score > 10)
        {
            medals[2].gameObject.SetActive(true); 
        }
    }
    public void MenuButton()
    {
        SceneManager.LoadScene("GameMenu");
    }
    public void RestartGameButton()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
