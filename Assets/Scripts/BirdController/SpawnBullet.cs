using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 10;
    public List<GameObject> bulletPool;
    public static SpawnBullet instance;

    private void Start()
    {
        MakeInstance();
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }
    private void Update()
    {
        Vector3 screenMaxPoint = new Vector3(Screen.width, Screen.height, 0);
        Vector3 worldMaxPoint = Camera.main.ScreenToWorldPoint(screenMaxPoint);
        float maxX = worldMaxPoint.x;
        foreach (GameObject bullet in bulletPool)
        {
            if (bullet.transform.position.x > maxX)
            {
                bullet.SetActive(false);
            }
        }    
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        GameObject newBullet = Instantiate(bulletPrefab);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}
