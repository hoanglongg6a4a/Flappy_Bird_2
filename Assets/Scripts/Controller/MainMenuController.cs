using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void ChoseRedBird()
    {  
        SceneManager.LoadScene("GamePlay");
        PlayerPrefs.SetString("BirdType", BirdType.Red.ToString()); ;
    }
    public void ChoseYellowBird()
    {
        SceneManager.LoadScene("GamePlay");
        PlayerPrefs.SetString("BirdType", BirdType.Yellow.ToString());
    }
    public void ChoseBlueBird()
    {
        SceneManager.LoadScene("GamePlay");
        PlayerPrefs.SetString("BirdType", BirdType.Blue.ToString());
    }
}
