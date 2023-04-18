using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CUITips : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject 匹配提示页面;
    public Text changci;
    public GameObject chengGong;
    public GameObject shiBai;
    public int juLiChangCi;
    /// <summary>
    /// 避免二次弹出成功
    /// </summary>
    public bool ischenggong;

    public void OorDTispPiPei(int _type, int _changci = 0)
    {
        匹配提示页面.SetActive(true);
        if (_type == 0)
        {
            //失败
            shiBai.SetActive(true);
            chengGong.SetActive(false);
        }
        else
        {
            //成功
            chengGong.SetActive(true);
            shiBai.SetActive(false);
            changci.text = _changci.ToString();
            juLiChangCi = _changci;
        }
    }
    public void DownPiPei()
    {
        匹配提示页面.SetActive(false);
    }

    public Text xiaoxi;
    public GameObject obj;
    //提醒押注
    public GameObject 押注提醒页;
    /// <summary>
    /// 避免二次弹出
    /// </summary>
    public bool isyazhu;

    public void OpeYaZhuTips()
    {
        if (!CUIMainManager._MainManager().yaZhu.bar.activeSelf)
            押注提醒页.SetActive(true);
    }
    public void DownYaZhu()
    {
        押注提醒页.SetActive(false);
        CUIMainManager._MainManager().关闭所有界面();
        CUIMainManager._MainManager().yaZhu.OorDBar(true);
    }
    public void BtnDownYaZhu()
    {
        押注提醒页.SetActive(false);
    }
    public void aaaa()
    {
        obj.gameObject.SetActive(!obj.gameObject.activeSelf);
    }

    //提醒押注
    public GameObject 登录提醒页;

    public void OpeDLTips(bool b)
    {
        登录提醒页.SetActive(b);
    }
    public void BtnFanLoding()
    {
        CUIMainManager._MainManager().isNetMapDate = false;
        OpeDLTips(false);
        CUIMainManager._MainManager().关闭所有界面();
        CUIMainManager._MainManager().denglu.OorDBar(true);
    }
    public void BtnTC()
    {
        Application.Quit();
    }

    //提示框
    public GameObject tips;
    public Transform tipsParent;
    public void Tips(string str, Transform parent = null)
    {
        if (parent == null)
            parent = tipsParent;
        Transform tra = Instantiate(tips, parent).transform;
        tra.gameObject.SetActive(true);
        tra.localPosition = Vector3.zero;
        tra.GetComponent<Tips>().str.text = str;
    }
    public GameObject tips1;
    public void Tips1(string str, Transform parent = null)
    {
        if (parent == null)
            parent = tipsParent;
        Transform tra = Instantiate(tips1, parent).transform;
        tra.gameObject.SetActive(true);
        tra.localPosition = Vector3.zero;
        tra.GetComponent<Tips>().str.text = str;
    }
}
