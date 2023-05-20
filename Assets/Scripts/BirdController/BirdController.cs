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
    public GameObject bullet;

    private bool isAlive;
    private bool didFlap;
    private GameObject spawner;
    public float flag=0;
    public int score = 0;
   

    public float jumpForce = 5f;
    private float gravity = 9.8f;
    private float verticalVelocity = 0f;
    private bool canPressButton;
    public bool hasScored;
    //private Bird currentBird;

    Bird IBird;
    public GameObject birtOjc;
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
            case "Yellow":
                IBird = gameObject.AddComponent<YellowBird>();
                birtOjc = IBird.ShowBird(yellowBird);
                break;
            case "Red":
                IBird = gameObject.AddComponent<RedBird>();
                birtOjc = IBird.ShowBird(redBird);
                break;
            case "Blue":
                IBird = gameObject.AddComponent<BlueBird>();
                birtOjc = IBird.ShowBird(blueBird);
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
       GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");
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
            if (birtOjc.transform.position.y <= -4f)
            {
                //flag = 1;
                isAlive = false;
                birtOjc.transform.position = new Vector3(birtOjc.transform.position.x, -4f, birtOjc.transform.position.z);              
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
                float angel = 0;
                angel = Mathf.Lerp(0, 90, verticalVelocity / 9);
                birtOjc.transform.rotation = Quaternion.Euler(0, 0, angel);
            }
            else 
            {
                float angel = 0;
                angel = Mathf.Lerp(0, -90, -(verticalVelocity / 9));
                birtOjc.transform.rotation = Quaternion.Euler(0, 0, angel);
            }
           
            if (pipes.Length > 0)
            {
                Transform pipeTransform = pipes[0].transform;
                Renderer pipeRenderer = pipeTransform.GetComponent<Renderer>();
                float maxX = pipeRenderer.bounds.max.x;
                float minX = pipeRenderer.bounds.max.y;
                Renderer birdRenderer = birtOjc.GetComponent<Renderer>();
                float BirdmaxX = birdRenderer.bounds.max.x;
                float BirdminX = birdRenderer.bounds.max.x;
                /*    int childCount = pipeTransform.childCount;
                    Transform secondChildTransform = pipeTransform.GetChild(1);
                    Renderer childRenderer = secondChildTransform.GetComponent<Renderer>();
                    float maxX = childRenderer.bounds.max.x;
                    float minX = childRenderer.bounds.min.x;*/
                //Mathf.Abs(maxX - BirdmaxX) < 0.01f
                // Kiểm tra xem chim đã đi qua khoảng cách giữa hai ống hay chưa
                if ( Mathf.Abs(maxX - BirdmaxX) < 0.023f && !hasScored)
                {
                    Debug.Log(maxX +""+BirdmaxX);
                    Debug.Log("co cham");
                    score++;
                    hasScored = true;
                    if (GamePlayController.instance != null)
                    {
                        GamePlayController.instance._SetScore(score);
                    }
                        audioSource.PlayOneShot(pingClip);
                }
                   
                if (BirdmaxX > maxX && hasScored)
                {
                    hasScored = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.G) && canPressButton)
            {
                if (birdType == "Red")
                {
                    IBird.Skill();
                }
                else if (birdType != "Red")
                {
                    StartCoroutine(SkillCoroutine());
                }
            }
            IEnumerator SkillCoroutine()
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
                if (countdownValue == 0)
                {
                    countDownText.text = "Ready";
                }

                canPressButton = true; // Cho phép bấm nút sau khi đã chờ xong
            }
            
           
         


        }
    }
    // Giá trị ngưỡng va chạm
    bool CheckCollision(GameObject birtOjc)
    {

        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        if (pipes.Length > 0)
        {
            float currentSpeed = PipeHolder.instance.GetSpeed();

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
}
