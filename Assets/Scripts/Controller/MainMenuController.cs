using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void _PlayButoon()
    {
        //Application.LoadLevel("GamePlay");
        SceneManager.LoadScene("GamePlay");
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
