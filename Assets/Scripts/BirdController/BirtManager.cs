using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : MonoBehaviour,Bird
{

        public GameObject ShowBird(GameObject chim)
        {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(chim, spawnPosition, Quaternion.identity);
        return bird;
        }
        public void skill()
        {
        Debug.Log("Vang");
            // Triển khai logic bay của RedBird
        }

        public void DisplayColor()
        {
            // Triển khai logic hiển thị màu sắc của RedBird
        }
    
}
public class BlueBird : MonoBehaviour, Bird
{


    public GameObject ShowBird(GameObject chim)
    {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(chim, spawnPosition, Quaternion.identity);
        return bird;
    }
    public void skill()
    {
        Debug.Log("Vang");
        // Triển khai logic bay của RedBird
    }

    public void DisplayColor()
    {
        // Triển khai logic hiển thị màu sắc của RedBird
    }

}
