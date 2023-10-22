using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorFiliter : MonoBehaviour
{
    //用來控制顏色的變化與按鈕的判斷
    public GameObject touchCanva;

    [Header("顏色遮罩")]
    public Image colorFiliterPanel;

    [Header("顏色遮罩物體")]
    public GameObject colorFiliterObject;

    [Header("文字顏色")]
    public Text text1;
    public Text text2;
    public Text text3;

    [Header("文字物體")]
    public GameObject textA;
    public GameObject textB;
    public GameObject textC;

    private void Awake()
    {
        colorFiliterObject.SetActive(false);
        textA.SetActive(false);
        textB.SetActive(false);
        textC.SetActive(false);
    }

    public void redBtn()
    {
        Color changeColor = new Color(1f,0.3f,0.3f);
        colorFiliterObject.SetActive(true);
        colorFiliterPanel.color = changeColor;

        //讓文字顯示
        textA.SetActive(true);
        textB.SetActive(false);
        textC.SetActive(false);

        //改變文字顏色
        text1.color = Color.black;
    }

    public void yellowBtn()
    {
        Color changeColor = new Color(1f,1f,0.3f);
        colorFiliterObject.SetActive(true);
        colorFiliterPanel.color = changeColor;

        //讓文字顯示
        textA.SetActive(false);
        textB.SetActive(true);
        textC.SetActive(false);

        //改變文字顏色
        text2.color = Color.black;
    }

    public void blueBtn()
    {
        Color changeColor = new Color(0.3f,0.6f,1);
        colorFiliterObject.SetActive(true);
        colorFiliterPanel.color = changeColor;

        //讓文字顯示
        textB.SetActive(false);
        textC.SetActive(false);
        textC.SetActive(true);

        //改變文字顏色
        text3.color = Color.black;
    }

    public void closeBtn()
    {
        touchCanva.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
}
