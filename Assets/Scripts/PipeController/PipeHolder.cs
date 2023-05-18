using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PipeHolder : MonoBehaviour
{
    private float speed  ;
    public static PipeHolder instance;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        _MakeInstance();
    }
    void _MakeInstance()
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
        if(BirdController.instance != null)
        {
           if (BirdController.instance.flag ==1)
            {
                Destroy(GetComponent<PipeHolder>());
            }
        }
      
        _PipeMovement();  
    } 
    void _PipeMovement()
    {
        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
        Renderer pipeRenderer = gameObject.GetComponent<Renderer>();
        float maxX = pipeRenderer.bounds.max.x;
        Vector3 screenMinPoint = new Vector3(0, 0, 0);
        Vector3 worldMinPoint = Camera.main.ScreenToWorldPoint(screenMinPoint);
        float minX = worldMinPoint.x;
        if (maxX <= minX)
        {
            Destroy(gameObject);
        }

    }

   /* void OnCollisionEnter2D(Collision2D target)
    {
        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
    }
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Destroy")
        {
            //BirdController.instance.hasScored = false;
            Destroy(gameObject);
        }
    }*/
}
