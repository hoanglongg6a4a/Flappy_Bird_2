using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : MonoBehaviour,Bird
{

        public GameObject ShowBird(GameObject chim)
        {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(chim, spawnPosition, Quaternion.identity);
        return bird;
        }
        public void skill()
        {
            PipeHolder.instance.SetSpeed(15f);
            float currentSpeed = PipeHolder.instance.GetSpeed();
        }

  
    
}
public class BlueBird : MonoBehaviour, Bird
{


    public GameObject ShowBird(GameObject chim)
    {
        Vector2 spawnPosition = new Vector2(-2f, 0f);
        GameObject bird = Instantiate(chim, spawnPosition, Quaternion.identity);
        return bird;
    }
    public void skill()
    {
        StartCoroutine(SkillCoroutine());
    }

    private IEnumerator SkillCoroutine()
    {
        PipeHolder.instance.SetSpeed(1f); // Thực hiện hàm setSpeed với tham số là 1f
        float currentSpeed = PipeHolder.instance.GetSpeed();

        yield return new WaitForSeconds(1f); // Đợi 1 giây

        // Thay đổi thông số khác sau 1 giây
        PipeHolder.instance.SetSpeed(5f);
        currentSpeed = PipeHolder.instance.GetSpeed();

    }



}
