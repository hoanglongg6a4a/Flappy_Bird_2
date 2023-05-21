using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PipeHolder : MonoBehaviour
{
    public float speed  ;
    public static PipeHolder instance;
    //public List<GameObject> poolPipe;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        MakeInstance();

    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public float GetSpeed()
    {
        return speed;
    }
    // Update is called once per frame
    void Update()
    {
        PipeMovement();  
    } 
    void PipeMovement()
    {
        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
        Renderer pipeRenderer = gameObject.GetComponent<Renderer>();
        float maxX = pipeRenderer.bounds.max.x;
        Vector3 screenMinPoint = new Vector3(0, 0, 0);
        Vector3 worldMinPoint = Camera.main.ScreenToWorldPoint(screenMinPoint);
        float minX = worldMinPoint.x;
        if (maxX < minX-0.5f)
        {
            BirdController.instance.hasScored = false;
            gameObject.SetActive(false);
            SpawnerPipe.instance.poolPipe.Add(gameObject);
        }
    }
}
