using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnerPipe : MonoBehaviour
{
    [SerializeField]
    private GameObject pipeHolder;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawner());
    }
    IEnumerator Spawner()
    {
        Vector3 screenMaxPoint = new Vector3(Screen.width, Screen.height, 0);
        Vector3 worldMaxPoint = Camera.main.ScreenToWorldPoint(screenMaxPoint);
        float maxX = worldMaxPoint.x;

        yield return new WaitForSeconds(1.5f);
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");
        if (pipes.Length < 2)
        {
            Vector3 temp = pipeHolder.transform.position;
            temp.y = Random.Range(-1.8f, 1.8f);
            temp.x = maxX;
            Instantiate(pipeHolder, temp, Quaternion.identity);
            //StartCoroutine(Spawner());
        }
        StartCoroutine(Spawner());
    }
  
}


