using Mono.Cecil;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private GameObject redBird, yellowBird, blueBird;
    public Text countDownText;

    private bool isAlive;
    private bool didFlap;
    private GameObject spawner;
    public float flag=0;
    public int score = 0;
   

    public float jumpForce = 5f;
    private float gravity = 10;
    private float verticalVelocity = 0f;
    private bool canPressButton;
    public bool hasScored;
    //private Bird currentBird;

    Bird IBird;
    GameObject birtOjc;
    string birdType = "";
    //Component c = gameObject.GetComponent<Bird>() as Component;
    // Start is called before the first frame update
    void Awake()
    {
        countDownText.text = "0";
        birdType =GameManager.instance.getBird();
        ChooseBird();
        hasScored = false;
        isAlive = true;
        canPressButton = true;
        if (instance == null)
        {
            instance = this;
        }
        spawner = GameObject.Find("Spawner Pipe");
    }
    public void ChooseBird()
    {
        switch (birdType)
        {
            case "Blue":
                IBird = gameObject.AddComponent<BlueBird>();
                birtOjc = IBird.ShowBird(blueBird);
                break;
            case "Yellow":
                IBird = gameObject.AddComponent<YellowBird>();
                birtOjc = IBird.ShowBird(yellowBird);
                break;
            // Xử lý các loại chim khác...
            default:
                // Loại chim không hợp lệ
                Debug.LogError("Invalid bird type!");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        birdMoveMent(birtOjc);

    }
    public void FlapButton()
    {
        didFlap = true;
    }
    void birdMoveMent(GameObject birtOjc)
    {
       Vector3 PrevioustPosition = birtOjc.transform.position;
        if (isAlive)
        {
            if (didFlap)
            {
                didFlap = false;
                verticalVelocity = jumpForce;
                audioSource.PlayOneShot(flyClip);
            }
            verticalVelocity -= gravity * Time.deltaTime;
            birtOjc.transform.position += Vector3.up * verticalVelocity * Time.deltaTime;
            // Check Va Chạm
            if (CheckCollision(birtOjc))
            {
                //flag = 1;
                isAlive = false;              
                //anim.SetTrigger("Died");
                audioSource.PlayOneShot(diedClip);
                verticalVelocity = 0;
                Time.timeScale = 0;
                Destroy(spawner);
                GamePlayController.instance.ShowMedal(score);
                GamePlayController.instance.BirdDiedShowPanel(score);
                
            }
            // kT Ground
            if (birtOjc.transform.position.y <= -3.8f)
            {
                //flag = 1;
                isAlive = false;
                birtOjc.transform.position = new Vector3(birtOjc.transform.position.x, -3.8f, birtOjc.transform.position.z);              
                //anim.SetTrigger("Died");
                audioSource.PlayOneShot(diedClip);
                verticalVelocity = 0; // Đặt lại tốc độ dọc thành 0 khi nằm yên trên sàn
                Time.timeScale = 0;
                Destroy(spawner);
                GamePlayController.instance.ShowMedal(score);
                GamePlayController.instance.BirdDiedShowPanel(score);
                
            }
            Vector3 currentPosition = birtOjc.transform.position;
            if (currentPosition.y > PrevioustPosition.y)
            {
                //Debug.Log("Ví trí >0"+currentPosition);
                float angel = 0;
                angel = Mathf.Lerp(0, 90, currentPosition.y / 5);
                birtOjc.transform.rotation = Quaternion.Euler(0, 0, angel);
            }
            else 
            {
                //Debug.Log("Ví trí <0" + currentPosition);
                float angel = 0;
                angel = Mathf.Lerp(0, -90, -(currentPosition.y / 5));
                birtOjc.transform.rotation = Quaternion.Euler(0, 0, angel);
            }
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");
            if (pipes.Length > 0)
            {
                Transform pipeTransform = pipes[0].transform;
                int childCount = pipeTransform.childCount;
                Transform secondChildTransform = pipeTransform.GetChild(1);
                Renderer childRenderer = secondChildTransform.GetComponent<Renderer>();
                float maxX = childRenderer.bounds.max.x;
                float minX = childRenderer.bounds.min.x;                                  
                        // Kiểm tra xem chim đã đi qua khoảng cách giữa hai ống hay chưa
                        if (Mathf.Abs(maxX - birtOjc.transform.position.x) < 0.01f  && !hasScored)
                        {
                           // Debug.Log("Chim đã đi qua khoảng cách giữa hai ống");
                            score++;
                            hasScored = true;
                            if (GamePlayController.instance != null)
                            {
                                GamePlayController.instance._SetScore(score);
                            }
                            audioSource.PlayOneShot(pingClip);                         
                        }
                   
                        if (birtOjc.transform.position.x > maxX)
                        {
                            hasScored = false;
                        }
            }
            if (Input.GetKeyDown(KeyCode.G) && canPressButton)
            {
                Debug.Log("Nhan G");
                StartCoroutine(SkillCoroutine());
            }
            IEnumerator SkillCoroutine()
            {
                canPressButton = false; // Không cho phép bấm nút trong thời gian chờ
                IBird.skill();
                //yield return new WaitForSeconds(5f); // Chờ 5 giây
                int countdownValue = 5;
                while (countdownValue >= 0)
                {

                    countDownText.text = countdownValue.ToString();
                    yield return new WaitForSeconds(1f); // Chờ 1 giây

                    countdownValue--;
                    if (countdownValue == 0)
                    {
                        countDownText.text = "Ready";
                    }
                }
               
                canPressButton = true; // Cho phép bấm nút sau khi đã chờ xong
            }
         


        }
    }
    // Giá trị ngưỡng va chạm
    bool CheckCollision(GameObject birtOjc)
    {
      
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        
        foreach (GameObject pipe in pipes)
        {
         
            Renderer birdRenderer = birtOjc.GetComponent<Renderer>();
            Renderer pipeRenderer = pipe.GetComponent<Renderer>();
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
}
