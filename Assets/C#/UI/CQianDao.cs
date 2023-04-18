using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CQianDao : MonoBehaviour
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
        if (b)
        {
            CUIMainManager._MainManager().mainBar.OOrDTips(TipsType.E_qiandao, false);
            CUIMainManager._MainManager().NET_GetSingInList();
        }
    }

    #region 刷新签到列表
    public GameObject kelong;
    public Transform parnt;
    public GameObject qiandaoBtn;
    public GameObject buyBtn;
    public Text buyShuoming;
    //删除签到列表
    public void CloneList()
    {
        for (int i = 0; i < parnt.childCount; i++)
        {
            Destroy(parnt.GetChild(i).gameObject);
        }
    }
    //刷新签到列表
    public void RefQD()
    {
        CloneList();
        for (int i = 0; i < CUIMainManager._MainManager().allSignInData.Count; i++)
        {
            SignInData data = CUIMainManager._MainManager().allSignInData[i];
            GameObject obj = Instantiate(kelong, parnt);
            obj.SetActive(true);
            if (data.isCheck)
            {
                obj.transform.Find("领过").gameObject.SetActive(true);
                obj.transform.Find("对勾").gameObject.SetActive(true);
            }
            if (data.isToday)
            {
                obj.transform.Find("要领").gameObject.SetActive(true);
                obj.name = data.id.ToString();
                obj.AddComponent<UIEventListener>().OnClick += Btn_QianDao;
            }
            obj.transform.Find("数量").GetComponent<Text>().text = data.awardNum.ToString();
            obj.transform.Find("名字").GetComponent<Text>().text = data.content;
            CUIMainManager._MainManager().HuanTu(obj.transform.Find("pic").GetComponent<Image>(), data.imgName);
        }
    }
    //签到
    public void Btn_QianDao(GameObject obj)
    {
        if (CUIMainManager._MainManager().mainDataInfo.isSignIn == 1)
        {
            CUIMainManager._MainManager().NET_SingIn();
        }
    }
    public void Btn_QianDao()
    {
        if (CUIMainManager._MainManager().mainDataInfo.isSignIn == 1)
        {
            CUIMainManager._MainManager().NET_SingIn();
        }
    }
    public void Btn_Buy()
    {
        CUIMainManager._MainManager().NET_PaySign();
    }
    #endregion

}


