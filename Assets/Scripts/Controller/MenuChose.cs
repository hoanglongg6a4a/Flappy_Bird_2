using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuChose : MonoBehaviour
{
    [SerializeField] 
    private Button previous, next, blue, red, yellow;
    private GameObject[] buttons;
    int index;
    void Start()
    {
        index = 0;
        buttons = new GameObject[] { blue.gameObject, red.gameObject, yellow.gameObject };
        ShowObject();
    }
    private void ShowObject()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(i == index);
        }
    }
    // Update is called once per frame

    public void PreviousButoon()
    {
        buttons[index].SetActive(false);
        index--;
        if (index < 0)
        {
            index = buttons.Length - 1;
        }
        buttons[index].SetActive(true);
    }    
    public void NextButoon()
    {
        buttons[index].SetActive(false);
        index++;
        if (index >= buttons.Length)
        {
            index = 0;
        }
        buttons[index].SetActive(true);
    }    
}
