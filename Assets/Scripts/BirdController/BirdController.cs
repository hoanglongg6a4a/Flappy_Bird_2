using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public static BirdController instance;
    //private Rigidbody2D myBody;
   
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    public AudioClip flyClip,pingClip,diedClip;


    private bool isAlive;
    private bool didFlap;
    private GameObject spawner;
    public float flag=0;
    public int score = 0;
 
    public float jumpForce = 5f;
    private float gravity = 10;
    private float verticalVelocity = 0f;

    public bool hasScored;

    // Start is called before the first frame update
    void Awake()
    {
        hasScored = false;
        isAlive = true;
        if (instance == null)
        {
            instance = this;
        }
        spawner = GameObject.Find("Spawner Pipe");
        //velocity = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        _BirdMoveMent ();

    }
    public void FlapButton()
    {
        didFlap = true;
    }
    void _BirdMoveMent()
    {
       Vector3 PrevioustPosition = transform.position;
        if (isAlive)
        {
            if (didFlap)
            {
                didFlap = false;
                verticalVelocity = jumpForce;
                audioSource.PlayOneShot(flyClip);
            }
            verticalVelocity -= gravity * Time.deltaTime;
            transform.position += Vector3.up * verticalVelocity * Time.deltaTime;
            //transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime);
            // Hàm kiểm tra ground
            //if (countScore())
            //{
            //    score += collisionCount;
            //    collisionCount= 0;
            //    if (GamePlayController.instance != null)
            //    {
            //        GamePlayController.instance._SetScore(score);
            //    }
            //    audioSource.PlayOneShot(pingClip);

            //}
            // Check Va Chạm
            if (CheckCollision())
            {
                flag = 1;
                isAlive = false;              
                anim.SetTrigger("Died");
                audioSource.PlayOneShot(diedClip);
                verticalVelocity = 0;
                Time.timeScale = 0;
                Destroy(spawner);
                GamePlayController.instance._BirdDiedShowPanel(score);
                GamePlayController.instance._ShowMedal(score);
            }
            // kT Ground
            if (transform.position.y <= -3.8f)
            {
                flag = 1;
                isAlive = false;
                transform.position = new Vector3(transform.position.x, -3.8f, transform.position.z);              
                anim.SetTrigger("Died");
                audioSource.PlayOneShot(diedClip);
                verticalVelocity = 0; // Đặt lại tốc độ dọc thành 0 khi nằm yên trên sàn
                Time.timeScale = 0;
                Destroy(spawner);
                GamePlayController.instance._BirdDiedShowPanel(score);
                GamePlayController.instance._ShowMedal(score);
            }
            Vector3 currentPosition = transform.position;
            if (currentPosition.y > PrevioustPosition.y)
            {
                //Debug.Log("Ví trí >0"+currentPosition);
                float angel = 0;
                angel = Mathf.Lerp(0, 90, currentPosition.y / 5);
                transform.rotation = Quaternion.Euler(0, 0, angel);
            }
            else 
            {
                //Debug.Log("Ví trí <0" + currentPosition);
                float angel = 0;
                angel = Mathf.Lerp(0, -90, -(currentPosition.y / 5));
                transform.rotation = Quaternion.Euler(0, 0, angel);
            }
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");

            foreach (GameObject pipe in pipes)
            {

                Transform pipeTransform = pipe.transform;
                int childCount = pipeTransform.childCount;
                if (childCount >= 2)
                {
                    Transform secondChildTransform = pipeTransform.GetChild(1);
                    Renderer childRenderer = secondChildTransform.GetComponent<Renderer>();

                    if (childRenderer != null)
                    {
                        float maxX = childRenderer.bounds.max.x;
                        //Debug.Log(maxX + "MaxX cột");                      
                        // Kiểm tra xem chim đã đi qua khoảng cách giữa hai ống hay chưa
                        if ( maxX < transform.position.x && !hasScored)
                        {
                            Debug.Log("Chim đã đi qua khoảng cách giữa hai ống");
                            // Cập nhật điểm và các thao tác khác
                            score++;
                            hasScored = true;
                            if (GamePlayController.instance != null)
                            {
                                GamePlayController.instance._SetScore(score);
                            }
                            audioSource.PlayOneShot(pingClip);
                        }
                    }
                }
            }
        }
    }
    void CheckPassThrough()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");

        foreach (GameObject pipe in pipes)
        {
            Transform pipeTransform = pipe.transform;
            int childCount = pipeTransform.childCount;
            bool hasScored = false;
            if (childCount >= 2)
            {
                Transform secondChildTransform = pipeTransform.GetChild(1);
                Renderer childRenderer = secondChildTransform.GetComponent<Renderer>();

                if (childRenderer != null)
                {
                    float maxX = childRenderer.bounds.max.x;
                    Debug.Log(maxX + "MaxX cột");

                    // Kiểm tra xem chim đã đi qua khoảng cách giữa hai ống hay chưa
                    if ( transform.position.x > maxX && !hasScored)
                    {
                        Debug.Log("Chim đã đi qua khoảng cách giữa hai ống");
                        // Cập nhật điểm và các thao tác khác
                        score++;
                        hasScored = true;
                        if (GamePlayController.instance != null)
                        {
                            GamePlayController.instance._SetScore(score);
                        }
                        audioSource.PlayOneShot(pingClip);
                    }
                }
            }
        }
    }

    // Giá trị ngưỡng va chạm
    bool CheckCollision()
    {
      
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        
        foreach (GameObject pipe in pipes)
        {
            //Debug.Log("pipe"+":"+i);
            //Vector2 birdSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
            //Vector2 pipeSize = pipe.GetComponent<SpriteRenderer>().bounds.size;
            Renderer birdRenderer = gameObject.GetComponent<Renderer>();
            Renderer pipeRenderer = pipe.GetComponent<Renderer>();
            // Lấy vị trí của bird và pipe
            /*Vector3 birdPos = transform.position;
            Vector3 pipePos = pipe.transform.position;*/

            // Tính các giá trị tọa độ của hộp giới hạn xung quanh bird và pipe
            float birdMinX = birdRenderer.bounds.min.x;
            float birdMaxX = birdRenderer.bounds.max.x;
            float birdMinY = birdRenderer.bounds.min.y;
            float birdMaxY = birdRenderer.bounds.max.y;

            float pipeMinX = pipeRenderer.bounds.min.x;
            float pipeMaxX = pipeRenderer.bounds.max.x;
            float pipeMinY = pipeRenderer.bounds.min.y;
            float pipeMaxY = pipeRenderer.bounds.max.y;

            // Kiểm tra xem hai hộp giới hạn có giao nhau hay không
            if (birdMinX <= pipeMaxX && birdMaxX >= pipeMinX &&
                birdMinY <= pipeMaxY && birdMaxY >= pipeMinY)
            {
                return true;
            }
            
        }
        return false;
        // Chim không va chạm vào ống

    }
   
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "PipeHolder")
        {
            score++;
            if (GamePlayController.instance != null)
            {
                GamePlayController.instance._SetScore(score);
            }
            audioSource.PlayOneShot(pingClip);
        }
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.tag== "Pipe" || target.gameObject.tag == "Ground")
        {
            flag = 1;
            if(isAlive)
            {
                isAlive = false;
                Destroy(spawner);
                audioSource.PlayOneShot(diedClip);
                anim.SetTrigger("Died");
            }
            if (GamePlayController.instance != null)
            {
                GamePlayController.instance._BirdDiedShowPanel(score);
                GamePlayController.instance._ShowMedal(score);
            }
            
        }
        
    }


}
