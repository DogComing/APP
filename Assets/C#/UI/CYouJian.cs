using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CYouJian : MonoBehaviour
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
            CUIMainManager._MainManager().mainBar.OOrDTips(TipsType.E_youjian, false);
            MailDateBarOpen(false);
            CUIMainManager._MainManager().NET_GetMailList();
            cur_id = -1;
        }
    }

    #region 刷新邮件列表
    public GameObject kelong;
    public Transform parnt;
    public Dictionary<int, MailUIData> allMail = new Dictionary<int, MailUIData>();
    //删除邮件列表
    public void CloneList()
    {
        for (int i = 0; i < parnt.childCount; i++)
        {
            Destroy(parnt.GetChild(i).gameObject);
        }
        allMail.Clear();
    }
    //刷新邮件列表
    public void RefMail()
    {
        CloneList();
        for (int i = 0; i < CUIMainManager._MainManager().allMailData.Count; i++)
        {
            MailData data = CUIMainManager._MainManager().allMailData[i];
            MailUIData uidata = new MailUIData();
            allMail.Add(data.id, uidata);
            GameObject obj = Instantiate(kelong, parnt);
            obj.name = data.id.ToString();
            obj.AddComponent<UIEventListener>().OnClick += MailBtn;
            obj.SetActive(true);
            uidata.yidu = obj.transform.Find("true").gameObject;
            uidata.weidu = obj.transform.Find("false").gameObject;
            uidata.time = obj.transform.Find("shijian").GetComponent<Text>();
            uidata.xiangqing = obj.transform.Find("详情").GetComponent<Text>();
            uidata.shuoming = obj.transform.Find("shuoming").GetComponent<Text>();
            if (data.isRead == 0)
            {
                uidata.weidu.SetActive(true);
            }
            else
            {
                uidata.yidu.SetActive(true);
            }
            uidata.time.text = FormatTimestamp(data.createTime);
            uidata.xiangqing.text = data.mailContent.ToString();
            uidata.shuoming.text = data.mailTitle.ToString();
        }
    }
    public static string FormatTimestamp(long lTime)
    {
        long begtime = lTime * 10000;
        System.DateTime dt_1970 = new System.DateTime(1970, 1, 1, 8, 0, 0);
        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
        long time_tricks = tricks_1970 + begtime;//日志日期刻度
        System.DateTime dt = new System.DateTime(time_tricks);//转化为DateTime
        return string.Format("{0:D4}-{1:D2}-{2:D2} " + "{3:D2}:{4:D2}:{5:D2}", dt.Year, dt.Month,
            dt.Day, dt.Hour, dt.Minute, dt.Second); ;
    }
    #endregion

    #region 邮件详情
    //邮件详情
    public GameObject mailDataBar;
    //全部删除按钮
    public GameObject btn1;
    //全部领取按钮
    public GameObject btn2;
    //领取按钮
    public GameObject btn11;
    //已领取按钮
    public GameObject btn12;
    public int cur_id;
    //打开详情按钮关闭
    void MailDateBarOpen(bool b)
    {
        mailDataBar.SetActive(b);
        btn1.SetActive(!b);
        btn2.SetActive(!b);
    }
    //介绍
    public Text jieshao;
    //内容
    public Text neirong;
    public Transform item1;
    public Transform item2;
    public Transform item3;
    //普通邮件
    public void RefMailXQ(MailData data)
    {
        btn11.SetActive(false);
        btn12.SetActive(false);
        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
        item3.gameObject.SetActive(false);
        MailDateBarOpen(true);
        jieshao.text = data.mailTitle;
        neirong.gameObject.SetActive(true);
        yazhuBar.gameObject.SetActive(false);
        neirong.text = data.mailContent;
        if (data.imgName != "")
        {
            string[] str = data.imgName.Split(',');
            string[] str2 = { };
            if (data.gameAward != "")
            {
                 str2 = data.gameAward.Split(',');
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0)
                {
                    item1.gameObject.SetActive(true);
                    CUIMainManager._MainManager().HuanTu(item1.Find("pic").GetComponent<Image>(), str[i]);
                    if (str2.Length != 0)
                    {
                        CUIMainManager._MainManager().SetNum(item1.Find("shuliang").GetComponent<Text>(), double.Parse(str2[i]));
                    }
                    else
                    {
                        CUIMainManager._MainManager().SetNum(item1.Find("shuliang").GetComponent<Text>(), data.awardNum);
                    }
                    
                }
                if (i == 1)
                {
                    item2.gameObject.SetActive(true);
                    CUIMainManager._MainManager().HuanTu(item2.Find("pic").GetComponent<Image>(), str[i]);
                    if (str2.Length != 0)
                    {
                        CUIMainManager._MainManager().SetNum(item2.Find("shuliang").GetComponent<Text>(), double.Parse(str2[i]));
                    }
                    else
                    {
                        CUIMainManager._MainManager().SetNum(item2.Find("shuliang").GetComponent<Text>(), data.awardNum);
                    }
                    
                }
                if (i == 2)
                {
                    item3.gameObject.SetActive(true);
                    CUIMainManager._MainManager().HuanTu(item3.Find("pic").GetComponent<Image>(), str[i]);
                    if (str2.Length != 0)
                    {
                        CUIMainManager._MainManager().SetNum(item3.Find("shuliang").GetComponent<Text>(), double.Parse(str2[i]));
                    }
                    else
                    {
                        CUIMainManager._MainManager().SetNum(item3.Find("shuliang").GetComponent<Text>(), data.awardNum);
                    }
                }

            }
        }
        if (data.awardNum != 0 || data.imgName.Length != 0)
        {
            //有奖励邮件
            if (data.isReceive == 0)
            {
                //未领取
                btn11.SetActive(true);
            }
            else
            {
                //以领取
                btn12.SetActive(true);
            }
        }
    }

    #region 押注邮件  没用到
    public GameObject yazhuBar;
    //押注号
    public Text yaZhuHao;
    //第几号
    public List<Text> diJiHao;
    //押注预制体
    public GameObject yazhuData;
    //押注邮件
    public void RefMailXQYZ(MailData data)
    {
        btn11.SetActive(false);
        btn12.SetActive(false);
        item1.gameObject.SetActive(false);
        item2.gameObject.SetActive(false);
        item3.gameObject.SetActive(false);
        MailDateBarOpen(true);
        neirong.gameObject.SetActive(false);
        yazhuBar.gameObject.SetActive(true);
        //介绍
        jieshao.text = data.mailTitle;
        //押注号
        yaZhuHao.text = "No." + "123123123";
        //排名
        for (int i = 0; i < diJiHao.Count; i++)
        {
            diJiHao[i].text = (i + 1).ToString();
        }
        //删除所有押注
        for (int i = 0; i < yazhuBar.transform.childCount; i++)
        {
            Destroy(yazhuBar.transform.GetChild(i + 2).gameObject);
        }
        //押注详情
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate(yazhuData, yazhuBar.transform);
            obj.SetActive(true);
            //押注类型
            obj.transform.Find("押注类型").GetComponent<Text>().text = "独赢";
            //obj.transform.Find("押注类型").GetComponent<Text>().text = "名次-前三";
            //号
            Text _hao = obj.transform.Find("号").GetComponent<Text>();
            _hao.text = _hao.text + 1 + "号" + " ";
            //倍率
            obj.transform.Find("倍率T/倍率").GetComponent<Text>().text = "1:" + 1;
        }

        if (data.awardNum != 0)
        {
            //有奖励邮件
            if (data.isReceive == 0)
            {
                //未领取
                btn11.SetActive(true);
            }
            else
            {
                //以领取
                btn12.SetActive(true);
            }
        }
    }
    #endregion

    #endregion

    #region 按钮
    //全部领取
    public void ALLLingQu()
    {
        CUIMainManager._MainManager().NET_AllLingQu();
    }
    //全部删除
    public void ALLShanChu()
    {
        CUIMainManager._MainManager().NET_AllShanChu();
    }
    //打开邮件详情
    void MailBtn(GameObject obj)
    {
        cur_id = int.Parse(obj.name);
        CUIMainManager._MainManager().NET_GetMailDate(cur_id);
    }
    //领取按钮
    public void BtnLingqu()
    {
        CUIMainManager._MainManager().NET_ReceiveMailDate(cur_id);
    }
    //详情返回 
    public void BtnRetrun()
    {
        OorDBar(true);
    }
    #endregion
}
public class MailUIData
{
    public int id;
    //已读
    public GameObject yidu;
    //未读
    public GameObject weidu;
    //时间
    public Text time;
    public Text xiangqing;
    //说明
    public Text shuoming;
}
