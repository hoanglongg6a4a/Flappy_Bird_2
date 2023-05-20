using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : MonoBehaviour,Bird
{

        public GameObject ShowBird(GameObject Bird)
        {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        return bird;
        }
        public void Skill()
        {
            PipeHolder.instance.SetSpeed(15f);
           
        }
}
public class RedBird : MonoBehaviour, Bird
{

    public GameObject ShowBird(GameObject Bird)
    {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        return bird;
    }
    public void Skill()
    {  
         Instantiate(BirdController.instance.bullet, BirdController.instance.birtOjc.transform.position, Quaternion.identity);

    }
}
public class BlueBird : MonoBehaviour, Bird
{
    public GameObject ShowBird(GameObject Bird)
    {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        return bird;
    }
    public void Skill()
    {
        StartCoroutine(SkillCoroutine());

    }
    private IEnumerator SkillCoroutine()
    {
        PipeHolder.instance.SetSpeed(1f); // Thực hiện hàm setSpeed với tham số là 1f
        yield return new WaitForSeconds(0.5f); // Đợi 1 giây                                                 // Thay đổi thông số khác sau 1 giây
        PipeHolder.instance.SetSpeed(5f);
    }
}

