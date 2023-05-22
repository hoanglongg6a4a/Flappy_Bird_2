using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : MonoBehaviour,Bird
{
        public GameObject ShowBird(GameObject Bird)
        {
        Vector2 spawnPosition = new Vector2(-1.5f, 0f);
        GameObject bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        return bird;
        }
        public void Skill()
        {
            StartCoroutine(SkillCoroutine());
        }
        private IEnumerator SkillCoroutine()
        {
            PipeHolder.instance.SetSpeed(15f);
            yield return new WaitForSeconds(0.5f);                                            
            PipeHolder.instance.SetSpeed(5f);
        }
 }



