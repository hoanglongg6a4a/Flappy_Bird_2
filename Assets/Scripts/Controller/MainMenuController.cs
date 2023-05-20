using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void choseRedBird()
    {
        //Application.LoadLevel("GamePlay");
        SceneManager.LoadScene("GamePlay");
        GameManager.instance.setBird("Red");
    }
    public void choseYellowBird()
    {
        SceneManager.LoadScene("GamePlay");
        GameManager.instance.setBird("Yellow");
    }
    public void choseBlueBird()
    {
        SceneManager.LoadScene("GamePlay");
        GameManager.instance.setBird("Blue");
    }
}
