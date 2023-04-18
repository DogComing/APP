using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CMianBar : MonoBehaviour
{
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        xiangQing.AddComponent<UIEventListener>().OnClick += Open;
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
        }
    }

    #region 按钮
    //打开地图界面
    public void OpenDiTu_Bar()
    {
        CUIMainManager._MainManager().NET_GetMapData();
    }
    //打开签到界面
    public void OpenQianDao_Bar()
    {
        CUIMainManager._MainManager().qiandao.OorDBar(true);
    }
    //打开邮件界面
    public void OpenYouJian_Bar()
    {
        CUIMainManager._MainManager().youjian.OorDBar(true);
    }
    //打开捕捉道具界面
    public void OpenBuZhuoDaoJu_Bar(int i)
    {
        CUIMainManager._MainManager().buZhuoDaoJu.OorDBar(true);
        CUIMainManager._MainManager().buZhuoDaoJu.SetTagSelect(i);
        bar.SetActive(false);
    }
    //打开饲养界面
    public void OpenSiYang()
    {
        CUIMainManager._MainManager().siYang.OorDBar(true);
        bar.SetActive(false);
    }
    //打开比赛界面
    public void OpenBiSai()
    {
        OorDBar(false);
        CUIMainManager._MainManager().biSai.OorDBar(true);
        CUIMainManager._MainManager().HuanBGMusic("zbbj");

    }
    //打开仓库界面
    public void OpenCangKu()
    {
        CUIMainManager._MainManager().cangKu.OorDBar(true);
        bar.SetActive(false);
    }
    //打开设置界面
    public void OpenSet()
    {
        CUIMainManager._MainManager().shezhi.OorDBar(true);
    }
    #endregion

    #region 小红点
    //签到0 邮件1
    public List<GameObject> hongdian = new List<GameObject>();
    public void OOrDTips(TipsType type, bool b)
    {
        hongdian[(int)type].SetActive(b);
    }
    #endregion

    #region 地图信息
    public Text gailv;
    public CUIGaiLv chongwu;
    public CUIGaiLv zhenbao;
    public CUIGaiLv zhuangbei;
    public CUIGaiLv siliao;
    public CUIGaiLv yesheng;
    public Text mapName;
    MapData mapData;
    public void SetMapData(MapData data)
    {
        mapData = data;
        mapName.text = "Lv."+data.id +"  "+ data.mapName;
        gailv.text = data.petRatio + "% " + data.gemRatio + "% " + data.equipRatio + "% " + data.forageRatio + "% " + data.wildRatio + "% ";
        chongwu.SetData(data.petRatio + "%", data.petRatioMap);
        zhenbao.SetData(data.gemRatio + "%", data.gemRatioMap);
        zhuangbei.SetData(data.equipRatio + "%", data.equipRatioMap);
        siliao.SetData(data.forageRatio + "%", data.forageRatioMap);
        yesheng.SetData(data.wildRatio + "%", data.wildRatioMap);
        SetBar(data.imgName);
    }
    public Transform bianDa;
    public GameObject xiangQing;
    public Image bj;
    public void SetBar(string name)
    {
        CUIMainManager._MainManager().HuanTu(bj, name);
    }

    public GameObject zheng;
    public GameObject dao;
    public void Open(GameObject obj)
    {
        if (bianDa.transform.localScale.y == 1)
        {
            //缩放
            zheng.SetActive(false);
            dao.SetActive(true);
            bianDa.DOScale(new Vector3(1, 0, 1), 0.1f);
        }
        else
        {
            zheng.SetActive(true);
            dao.SetActive(false);
            //展开
            bianDa.DOScale(new Vector3(1, 1, 1), 0.1f);
        }
    }
    #endregion

    #region 捕捉
    public GameObject btn;
    //点击捕捉
    public void BtnTips()
    {
        AniBuZhuo();
    }
    public SkeletonGraphic dogAn;
    public GameObject dog;
    public Image dogPic;
    //界面狗
    public void SetDog()
    {
        if (CUIMainManager._MainManager().mainDataInfo.dogMap.id != 0)
        {
            dog.SetActive(true);
            string str = "1";
            str = CUIMainManager._MainManager().mainDataInfo.dogMap.dogImg.Substring(4);
            CUIMainManager._MainManager().HuanDogA(dogAn, "anidog_" + str + "_" + str);
        }
        else
        {
            dog.SetActive(false);
        }
    }
    public GameObject buzhuo;
    public bool isbuZhuo;
    //动画捕捉
    public void AniBuZhuo()
    {
        if (CUIMainManager._MainManager().mainDataInfo.residueMuscleNum < mapData.useBrawn)
        {
            CUIMainManager._MainManager().cUITips.Tips("体力不足");
            return;
        }
        if (CUIMainManager._MainManager().mainDataInfo.dogCoin < mapData.useAgs)
        {
            CUIMainManager._MainManager().cUITips.Tips("金币不足");
            return;
        }
        if (isbuZhuo) {
            CUIMainManager._MainManager().cUITips.Tips("正在捕捉...");
            return;
        }
        //发送消息
        CUIMainManager._MainManager().NET_SendCapture(0, 0);
        isbuZhuo = true;
    }
    public void AniBuZhuoDown()
    {
        buzhuo.transform.GetChild(0).GetComponent<Animator>().SetBool("isPlay", false);
        buzhuo.SetActive(false);
    }
    #endregion

    #region 托盘显示
    public List<Image> tuopanAll = new List<Image>();
    public List<TMP_Text> naijiuAll = new List<TMP_Text>();
    //刷新托盘显示
    public void RefShow()
    {
        ALLDownTP();
        for (int i = 0; i < tuopanAll.Count; i++)
        {
            BuZhuoDaoJuData data = CUIMainManager._MainManager().GetCurCathEquip(i + 1);
            if (data != null)
            {
                tuopanAll[i].gameObject.SetActive(true);
                CUIMainManager._MainManager().HuanTu(tuopanAll[i], data.imgName);
                naijiuAll[i].text = data.durabilityResidue + "/" + data.durabilityMax;
            }
        }
    }
    public void ALLDownTP()
    {
        for (int i = 0; i < tuopanAll.Count; i++)
            tuopanAll[i].gameObject.SetActive(false);
    }
    #endregion
}
//红点类型
public enum TipsType
{
    E_qiandao = 0,
    E_youjian = 1,
}
