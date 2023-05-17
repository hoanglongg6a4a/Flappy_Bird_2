using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 tempScale = transform.localScale;
        float width = sr.bounds.size.x;
        float worldWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        tempScale.x = worldWidth / width;
        transform.localScale = tempScale;
    }
}
