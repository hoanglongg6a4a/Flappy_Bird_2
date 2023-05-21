using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBird : MonoBehaviour, Bird
{
    public GameObject ShowBird(GameObject Bird)
    {
        Vector2 spawnPosition = new Vector2(-1.5f, 0f);
        GameObject bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        return bird;
    }
    public void Skill()
    {
        Instantiate(BirdController.instance.bullet, BirdController.instance.birdOjc.transform.position, Quaternion.identity);
    }
}