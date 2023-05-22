using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    string scene = "GamePlay";

    public void ChooseBird(BirdType birdType)
    {
        SceneManager.LoadScene(scene);
        PlayerPrefs.SetString("BirdType", birdType.ToString());
    }
    public void ChoseRedBird()
    {
        ChooseBird(BirdType.Red);
    }
    public void ChoseYellowBird()
    {
        ChooseBird(BirdType.Yellow);
    }
    public void ChoseBlueBird()
    {
        ChooseBird(BirdType.Blue);
    }
}
