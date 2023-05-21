using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void choseRedBird()
    {
        
        SceneManager.LoadScene("GamePlay");
        PlayerPrefs.SetString("BirdType", BirdType.Red.ToString()); ;
    }
    public void choseYellowBird()
    {
        SceneManager.LoadScene("GamePlay");
        PlayerPrefs.SetString("BirdType", BirdType.Yellow.ToString());
    }
    public void choseBlueBird()
    {
        SceneManager.LoadScene("GamePlay");
        PlayerPrefs.SetString("BirdType", BirdType.Blue.ToString());
    }
}
