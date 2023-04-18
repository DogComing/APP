using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSheZhi : MonoBehaviour
{
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OorDBar(bool b)
    {
        bar.SetActive(b);
    }
    public bool isYY;
    public bool isYX;
    public GameObject yinyue;
    public GameObject yinxiao;

    //按钮点击
    public void OorDYY(bool b)
    {
        string value = "isMusic";
        int key;
        if (!b)
        {
            key = 2;
        }
        else
        {
            key = 1;
        }
        CUIMainManager._MainManager().NET_SetMainInfo(value, key);
    }
    public void OorDYX(bool b)
    {
        string value = "isEffect";
        int key;
        if (!b)
        {
            key = 2;
        }
        else
        {
            key = 1;
        }
        CUIMainManager._MainManager().NET_SetMainInfo(value, key);
    }
    //服务器接收
    public void NetOorDYY(bool b)
    {
        isYY = b;
        yinyue.SetActive(isYY);
        if (b) { CUIMainManager._MainManager().bj_music.volume = 1; }
        else { CUIMainManager._MainManager().bj_music.volume = 0; }
        
    }
    public void NetOorDYX(bool b)
    {
        isYX = b;
        yinxiao.SetActive(isYX);
        if (b) { CUIMainManager._MainManager().yx_music.volume = 1; }
        else { CUIMainManager._MainManager().yx_music.volume = 0; }
    }


    #region 切换中英
    public GameObject yingwen;
    public string zhEn = "zh";
    public void BtnEnCh(string str)
    {
        string value = "language";
        string key = str;
        CUIMainManager._MainManager().NET_SetMainInfo(value, key);
    }
    public void NetEnCh(string str)
    {
        zhEn = str;
        if (str == "en")
        {
            yingwen.SetActive(false);
        }
        else
        {
            yingwen.SetActive(true);
        }
        SwitchEnCh[] list = transform.parent.GetComponentsInChildren<SwitchEnCh>(true);
        for (int i = 0; i < list.Length; i++)
        {
            list[i].RefShow(zhEn);
        }
    }

    #endregion

    //关闭程序
    public void Btn_DownDog()
    {
        Application.Quit();
    }
    //返回登录
    public void Btn_ReturnLoding()
    {
        CUIMainManager._MainManager().isNetMapDate = false;
        CUIMainManager._MainManager().denglu.OorDBar(true);
    }
}
