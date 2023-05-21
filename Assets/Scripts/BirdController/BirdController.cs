using JetBrains.Annotations;
using Mono.Cecil;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BirdType
{
    Yellow,
    Red,
    Blue

}

public class BirdController : MonoBehaviour
{
    public static BirdController instance;
   
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip flyClip,pingClip,diedClip;
    [SerializeField]
    private GameObject redBird, yellowBird, blueBird;
    public Text countDownText;
    public GameObject bullet;
    GameObject[] pipes;
    private bool isAlive;
    private bool didFlap;
    public float jumpForce = 5f;
    private float gravity = 9.8f;
    private float verticalVelocity = 0f;
    private bool canPressButton;
    public bool hasScored;
    public int score ;
    Bird IBird;
    public GameObject birdOjc;
    public BirdType birdType;
    private bool Cooldown;

    void Awake()
    {
        string birdTypeString = PlayerPrefs.GetString("BirdType");
        birdType = (BirdType)Enum.Parse(typeof(BirdType), birdTypeString);
        countDownText.text = "Go";
        score = 0;
        ChooseBird();
        hasScored = false;
        isAlive = true;
        canPressButton = true;
        if (instance == null)
        {
            instance = this;
        }
    }
    public void ChooseBird()
    {
        switch (birdType)
        {
            case BirdType.Yellow:
                IBird = gameObject.AddComponent<YellowBird>();
                birdOjc = IBird.ShowBird(yellowBird);
                break;
            case BirdType.Red:
                IBird = gameObject.AddComponent<RedBird>();
                birdOjc = IBird.ShowBird(redBird);
                break;
            case BirdType.Blue:
                IBird = gameObject.AddComponent<BlueBird>();
                birdOjc = IBird.ShowBird(blueBird);
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
        birdMoveMent();

    }
    public void FlapButton()
    {
        didFlap = true;
    }
    void birdMoveMent()
    {
       Vector3 PrevioustPosition = birdOjc.transform.position;
      
        if (isAlive)
        {
            if (didFlap)
            {
                didFlap = false;
                verticalVelocity = jumpForce;
                audioSource.PlayOneShot(flyClip);
            }
            verticalVelocity -= gravity * Time.deltaTime;
            birdOjc.transform.position += Vector3.up * verticalVelocity * Time.deltaTime;
            Vector3 currentPosition = birdOjc.transform.position;
            // Bird angle
            checkAngel(PrevioustPosition, currentPosition);
            // Check Va Chạm
            if (CheckCollision(birdOjc))
            {
                isAlive = false;              
                audioSource.PlayOneShot(diedClip);
                verticalVelocity = 0;
                Time.timeScale = 0;
                GamePlayController.instance.ShowMedal(score);
                GamePlayController.instance.BirdDiedShowPanel(score);      
            }
            else
            {
                pipes = GameObject.FindGameObjectsWithTag("PipeHolder");
                if (pipes.Length > 0)
                {
                    Transform pipeTransform = pipes[0].transform;
                    Renderer pipeRenderer = pipeTransform.GetComponent<Renderer>();
                    float maxX = pipeRenderer.bounds.max.x;
                    float minX = pipeRenderer.bounds.max.y;
                    Renderer birdRenderer = birdOjc.GetComponent<Renderer>();
                    float BirdmaxX = birdRenderer.bounds.max.x;
                    float BirdminX = birdRenderer.bounds.max.x;
                    // Kiểm tra xem chim đã đi qua khoảng cách giữa hai ống hay chưa
                    if (Mathf.Abs(maxX - birdOjc.transform.position.x) < 0.01f && !hasScored)
                    {
                        hasScored = true;
                        IncreaseScore();
                    }
                    if (birdOjc.transform.position.x > maxX && hasScored)
                    {
                        hasScored = false;
                    }
                }
            }              
            if (Input.GetKeyDown(KeyCode.G) && canPressButton)
            {
                if (birdType.Equals(BirdType.Red))
                {
                    IBird.Skill();
                }
                else
                {     
                    StartCoroutine(SkillCoolDown());
                }
            }
        }
    }
    // Giá trị ngưỡng va chạm
    bool CheckCollision(GameObject birtOjc)
    {
        pipes = GameObject.FindGameObjectsWithTag("Pipe");
        // Check Ground
        if (birtOjc.transform.position.y <= -4f)
        {
            isAlive = false;
            birtOjc.transform.position = new Vector3(birtOjc.transform.position.x, -4f, birtOjc.transform.position.z);
            audioSource.PlayOneShot(diedClip);
            verticalVelocity = 0; // Đặt lại tốc độ dọc thành 0 khi nằm yên trên sàn
            Time.timeScale = 0;
            GamePlayController.instance.ShowMedal(score);
            GamePlayController.instance.BirdDiedShowPanel(score);
        }
        // Check Var
        if (pipes.Length > 0)
        {
            float currentSpeed = PipeHolder.instance.GetSpeed();
            foreach (GameObject pipe in pipes)
            {
                Renderer birdRenderer = birtOjc.GetComponent<Renderer>();
                Renderer pipeRenderer = pipe.GetComponent<Renderer>();
                float birdMinX = birdRenderer.bounds.min.x;
                float birdMaxX = birdRenderer.bounds.max.x;
                float birdMinY = birdRenderer.bounds.min.y;
                float birdMaxY = birdRenderer.bounds.max.y;
                float pipeMinX = pipeRenderer.bounds.min.x;
                float pipeMaxX = pipeRenderer.bounds.max.x;
                float pipeMinY = pipeRenderer.bounds.min.y;
                float pipeMaxY = pipeRenderer.bounds.max.y;
                if (currentSpeed <= 5)
                {
                    // Kiểm tra xem hai hộp giới hạn có giao nhau hay không
                    if (birdMinX <= pipeMaxX && birdMaxX >= pipeMinX &&
                        birdMinY <= pipeMaxY && birdMaxY >= pipeMinY)
                    {
                        return true;
                    }
                }
            }
        } 
        return false;
        // Chim không va chạm vào ống
    }
    public void IncreaseScore()
    {
        score++;
        if (GamePlayController.instance != null)
        {
            GamePlayController.instance.SetScore(score);
        }
        audioSource.PlayOneShot(pingClip);
    }    
    public void checkAngel(Vector3 PrevioustPosition , Vector3 currentPosition)
    {
        float angle = Mathf.Lerp(0, (currentPosition.y > PrevioustPosition.y) ? 90 : -90, Mathf.Abs(verticalVelocity) / 9);
        birdOjc.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    IEnumerator SkillCoolDown()
    {
        canPressButton = false; // Không cho phép bấm nút trong thời gian chờ
        IBird.Skill();
        //yield return new WaitForSeconds(5f); // Chờ 5 giây
        int countdownValue = 5;
        while (countdownValue >= 0)
        {
            countDownText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1f); // Chờ 1 giây
            countdownValue--;
        }
        countDownText.text = "Go";
        canPressButton = true; // Cho phép bấm nút sau khi đã chờ xong
    }
}
