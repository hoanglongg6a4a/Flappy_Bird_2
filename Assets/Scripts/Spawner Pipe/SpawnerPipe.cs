using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnerPipe : MonoBehaviour
{
    [SerializeField] private GameObject pipeHolder;
    public List<GameObject> poolPipe;
    public static SpawnerPipe instance;

    // Start is called before the first frame update
    void Start()
    {
        MakeInstance();
        StartCoroutine(Spawner());
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
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
            GameObject pipe = GetPooledPipe();
            Vector3 temp = pipe.transform.position;
            temp.y = Random.Range(-1.8f, 1.8f);
            temp.x = maxX;
            pipe.transform.position = temp;
        }
        StartCoroutine(Spawner());
    }
    public GameObject GetPooledPipe()
    {
        /*   foreach(GameObject pipe in this.poolPipe)
           {
               if(pipe.name == pipeHolder.name)
               {
                   this.poolPipe.Remove(pipe);
                   pipe.SetActive(true);
                   return pipe;
               }    
           }    
           // T?o m?t c?t m?i n?u kh�ng c� c?t t�i s? d?ng n�o kh? d?ng
           GameObject newPipe = Instantiate(pipeHolder);
           newPipe.name = pipeHolder.name;
           newPipe.SetActive(true);
           poolPipe.Add(newPipe);
           return newPipe;*/
        if (poolPipe.Count > 0)
        {
            GameObject pipe = poolPipe[0]; // L?y c?t ??u ti�n trong danh s�ch
            poolPipe.RemoveAt(0); // X�a c?t kh?i danh s�ch
            pipe.SetActive(true);
            return pipe;
        }
        else
        {
            // T?o m?t c?t m?i n?u danh s�ch t�i s? d?ng r?ng
            GameObject newPipe = Instantiate(pipeHolder);
            newPipe.name = pipeHolder.name;
            newPipe.SetActive(true);
            return newPipe;
        }
    }
    public void AddToInactivePipes(GameObject pipe)
    {
        poolPipe.Add(pipe);
        //pipe.SetActive(false);
    }
}


