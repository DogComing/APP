using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CYaZhu : MonoBehaviour
{
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        InitMC();
        InitBW();
        xiazhu_DY.onValueChanged.AddListener(ChangedValue);
    }
    
    // Update is called once per frame
    void Update()
    {

    }
    public void OorDBar(bool b)
    {
        if (b)
        {
            SendMingCi(1);
            CUIMainManager._MainManager().NET_BiSaiDog();
            //Debug.LogError("申请押注当前时间");
            CUIMainManager._MainManager().NET_GameType();//申请押注当前时间
            CUIMainManager._MainManager().HuanBGMusic("zbbj");
            CUIMainManager._MainManager().NET_JiangChi();
            CUIMainManager._MainManager().daoHang.OorDBar(true);
        }
        else
        {
            CUIMainManager._MainManager().mainBar.OorDBar(true);
            CUIMainManager._MainManager().daoHang.OorDBar(true);
            CUIMainManager._MainManager().HuanBGMusic("mainbj");
        }
        bar.SetActive(b);
    }
    #region 奖池
    //奖池
    public Text jiangchi;
    public int cur_JiangChi;
    public void IncreaseAnim(int startValue, int targetValue)
    {
        if (startValue == targetValue) return;
        var mScoreSequence = DOTween.Sequence();
        mScoreSequence.Append(DOTween.To(delegate (float value)
        {
            var temp = Mathf.FloorToInt(value);
            jiangchi.text = temp + "";
        }, startValue, targetValue, 0.8f));
        cur_JiangChi = targetValue;
    }
    #endregion

    #region 最大下注
    int[] betAll = { 0, 0, 0, 0, 0, 0 };
    public int GetMax()
    {
        int _zhu = 0;
        int hao = 0;
        for (int i = 0; i < betAll.Length; i++)
        {
            if (betAll[i] > _zhu)
            {
                _zhu = betAll[i];
                hao = i;
            }
        }
        return hao;
    }
    public void SetHao(int _hao,int _zhu)
    {
        if (betAll[_hao] < _zhu)
        {
            betAll[_hao] = _zhu;
        }
    }
    #endregion

    #region 页签
    public TagManager tagManager;
    public List<GameObject> allTag = new List<GameObject>();
    public void Btn_Tag(int i)
    {
        tagManager.Cur_SelectedTab = i;
        tagManager.SetSelect();
        for (int a = 0; a < allTag.Count; a++)
        {
            allTag[a].SetActive(false);
        }
        allTag[i].SetActive(true);
        RefTipsYaZhu();
    }
    #endregion

    #region 独赢
    int dycur = 0;
    //下注数量
    public InputField xiazhu_DY;
    public SelectedManager selected_DY;
    private void ChangedValue(string value)
    {
        if (value == "")
        {
            return;
        }
        if (value == "-")
        {
            xiazhu_DY.text = "";
            return;
        }
        if (value == "0")
        {
            xiazhu_DY.text = "";
            return;
        }
        int shengyu = 100 - dycur;
        if (int.Parse(value) > shengyu)
        {
            xiazhu_DY.text = shengyu.ToString();
            if (dycur >= 100)
            {
                CUIMainManager._MainManager().cUITips.Tips("每赛道最多赞助100注\n您可继续参加与其他玩法");
            }
        }
    }
    #endregion

    #region 名次
    public SelectedManager selected_MC;
    public List<BetManager> listBetManager_MC = new List<BetManager>();
    void InitMC()
    {
        for (int i = 0; i < listBetManager_MC.Count; i++)
        {
            listBetManager_MC[i].data = selected_MC;
        }
    }
    int GetAllBetValue()
    {
        int value = 0;
        for (int i = 0; i < listBetManager_MC.Count; i++)
        {
            if (listBetManager_MC[i].yazhu)
            {
                if (listBetManager_MC[i].yazhu.text != "")
                {
                    value += int.Parse(listBetManager_MC[i].yazhu.text);
                }
            }
        }
        return value;
    }
    int GetAllBetQian()
    {
        int value = 0;
        for (int i = 0; i < listBetManager_MC.Count; i++)
        {
            int beilv = i + 2;
            if (listBetManager_MC[i].yazhu)
            {
                if (listBetManager_MC[i].yazhu.text != "")
                {
                    value += (int.Parse(listBetManager_MC[i].yazhu.text)) * beilv;
                }
            }
        }
        return value;
    }
    void GetNetMC(out List<List<int>> migcihao, out List<int> mingcizhu)
    {
        migcihao = new List<List<int>>();
        mingcizhu = new List<int>();
        for (int i = 0; i < listBetManager_MC.Count; i++)
        {
            List<int> qwe = new List<int>();
            if (listBetManager_MC[i].GetValue(out qwe))
            {
                migcihao.Add(qwe);
            }
            mingcizhu.Add(listBetManager_MC[i].GetZhu());
        }
    }

    #endregion

    #region 包围
    public SelectedManager selected_BW;
    public List<BetManager> listBetManager_BW = new List<BetManager>();
    void InitBW()
    {
        for (int i = 0; i < listBetManager_BW.Count; i++)
        {
            listBetManager_BW[i].data = selected_BW;
        }
    }
    int GetAllBetValue1()
    {
        int value = 0;
        for (int i = 0; i < listBetManager_BW.Count; i++)
        {
            if (listBetManager_BW[i].yazhu)
            {
                if (listBetManager_BW[i].yazhu.text != "")
                {
                    value += int.Parse(listBetManager_BW[i].yazhu.text);
                }
            }
        }
        return value;
    }
    int GetAllBetJiBi1()
    {
        int value = 0;
        for (int i = 0; i < listBetManager_BW.Count; i++)
        {
            int beilv = i + 2;
            if (listBetManager_BW[i].yazhu)
            {
                if (listBetManager_BW[i].yazhu.text != "")
                {
                    value += (int.Parse(listBetManager_BW[i].yazhu.text) * beilv);
                }
            }
        }
        return value;
    }
    void GetNetBW(out List<List<int>> baoweihao, out List<int> baoweizhu)
    {
        baoweihao = new List<List<int>>();
        baoweizhu = new List<int>();
        for (int i = 0; i < listBetManager_BW.Count; i++)
        {
            List<int> qwe = new List<int>();
            if (listBetManager_BW[i].GetValue(out qwe))
            {
                baoweihao.Add(qwe);
            }
            baoweizhu.Add(listBetManager_BW[i].GetZhu());
        }

    }
    #endregion

    public void SendMingCi(int type)
    {
        List<List<int>> list = new List<List<int>>();
        if (type == 1)
        {
            CUIMainManager._MainManager().NET_GetBeiLv(1);
            return;
        }
        if (type == 2)
        {
            for (int i = 0; i < listBetManager_MC.Count; i++)
            {
                list.Add(listBetManager_MC[i].a_jihao);
            }
        }
        if (type == 3)
        {
            for (int i = 0; i < listBetManager_BW.Count; i++)
            {
                list.Add(listBetManager_BW[i].a_jihao);
            }
        }
        CUIMainManager._MainManager().NET_GetBeiLv(type, list);
    }
    //赞助后记录当前最大
    public void ChongZhiXiaZhu(bool b = true)
    {
        if (b)
        {
            dycur = 0;
            for (int i = 0; i < listBetManager_MC.Count; i++)
            {
                listBetManager_MC[i].curZhu = 0;
            }
            for (int i = 0; i < listBetManager_BW.Count; i++)
            {
                listBetManager_BW[i].curZhu = 0;
            }
        }
        else
        {
            if (xiazhu_DY.text!="")
            {
                dycur += int.Parse(xiazhu_DY.text);
                if (dycur > 100)
                {
                    dycur = 100;
                }
            }
            
            for (int i = 0; i < listBetManager_MC.Count; i++)
            {
                if (listBetManager_MC[i].yazhu!=null)
                {
                    if (listBetManager_MC[i].yazhu.text != "")
                    {
                        listBetManager_MC[i].curZhu += int.Parse(listBetManager_MC[i].yazhu.text);
                        if (listBetManager_MC[i].curZhu > 100)
                        {
                            listBetManager_MC[i].curZhu = 100;
                        }
                    }
                }
            }
            for (int i = 0; i < listBetManager_BW.Count; i++)
            {
                if (listBetManager_BW[i].yazhu != null)
                {
                    if (listBetManager_BW[i].yazhu.text != "")
                    {
                        listBetManager_BW[i].curZhu += int.Parse(listBetManager_BW[i].yazhu.text);
                        if (listBetManager_BW[i].curZhu > 100)
                        {
                            listBetManager_BW[i].curZhu = 100;
                        }
                    }
                }
            }
        }
    }
    //重置
    public void chongZhiXiaZhu()
    {
        //独赢重置
        xiazhu_DY.text = "";
        SendMingCi(1);
        //包围重置
        for (int i = 0; i < listBetManager_BW.Count; i++)
        {
            listBetManager_BW[i].SetPeiL("0");
            if (listBetManager_BW[i].yazhu)
            {
                listBetManager_BW[i].yazhu.text = "";
                for (int q = 0; q < listBetManager_BW[i].jihaoList.Count; q++)
                {
                    listBetManager_BW[i].jihaoList[q].text = "";
                    listBetManager_BW[i].a_jihao[q] = 0;
                }
            }
        }
        //名次重置
        for (int i = 0; i < listBetManager_MC.Count; i++)
        {
            listBetManager_MC[i].SetPeiL("0");
            if (listBetManager_MC[i].yazhu)
            {
                listBetManager_MC[i].yazhu.text = "";
                for (int q = 0; q < listBetManager_MC[i].jihaoList.Count; q++)
                {
                    listBetManager_MC[i].jihaoList[q].text = "";
                    listBetManager_MC[i].a_jihao[q] = 0;
                }
            }
        }
        RefDY();
        RefYZ();
        RefBY();
    }
    #region 赞助记录
    public GameObject 赞助记录版面;
    public GameObject 赞助记录预制体;
    public Transform 赞助记录父物体;
    //打开赞助记录
    public void OorDZhanZhu(bool b)
    {
        赞助记录版面.SetActive(b);
        if (b)
        {
            CUIMainManager._MainManager().NET_BetData();
        }
    }
    //刷新赞助记录
    public void RefZhanZhu()
    {
        for (int i = 0; i < 赞助记录父物体.childCount; i++)
        {
            Destroy(赞助记录父物体.GetChild(i).gameObject);
        }
        List<BetData> list = CUIMainManager._MainManager().allBet;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject obj = Instantiate(赞助记录预制体, 赞助记录父物体).gameObject;
            obj.SetActive(true);
            obj.transform.Find("用户").GetComponent<Text>().text = list[i].userName;

            string fangshi = "";
            Transform tra = obj.transform.Find("押注方式1");

            if (list[i].stakeType == 1) { fangshi = "独赢"; }
            if (list[i].stakeType == 2)
            {
                fangshi = "名次";
                if (list[i].selectType == 2) { tra.gameObject.SetActive(true); tra.GetComponent<Text>().text = "-前2"; }
                if (list[i].selectType == 3) { tra.gameObject.SetActive(true); tra.GetComponent<Text>().text = "-前3"; }
                if (list[i].selectType == 4) { tra.gameObject.SetActive(true); tra.GetComponent<Text>().text = "-前4"; }
            }
            if (list[i].stakeType == 3)
            {
                fangshi = "包围";
                if (list[i].selectType == 2) { tra.gameObject.SetActive(true); tra.GetComponent<Text>().text = "-前2"; }
                if (list[i].selectType == 3) { tra.gameObject.SetActive(true); tra.GetComponent<Text>().text = "-前3"; }
                if (list[i].selectType == 4) { tra.gameObject.SetActive(true); tra.GetComponent<Text>().text = "-前4"; }
            }
            obj.transform.Find("押注方式").GetComponent<Text>().text = fangshi;

            Text hao = obj.transform.Find("押注几号").GetComponent<Text>();
            //for (int q = 0; q < list[i].betNo.Length; q++)
            //{
            //    hao.text = hao.text + list[i].betNo[q] + "号 ";
            //}
            hao.text = list[i].dogNum;
            obj.transform.Find("注").GetComponent<Text>().text = list[i].pourNum.ToString();
        }
    }

    #endregion

    //下注按钮
    public void TiaoZhuan()
    {
        switch (tagManager.Cur_SelectedTab)
        {
            case 0:
                if (xiazhu_DY.text != "" && selected_DY.GetKey() != -1)
                {
                    int zhu = int.Parse(xiazhu_DY.text);
                    SetHao(selected_DY.GetKey(), zhu);
                    CUIMainManager._MainManager().NET_DuYi(selected_DY.GetKey() + 1, zhu);
                }
                else
                {
                    CUIMainManager._MainManager().cUITips.Tips("请选择您心仪的狗进行赞助"); return;
                }
                return;
            case 1:
                List<List<int>> migcihao;
                List<int> mingcizhu;
                GetNetMC(out migcihao, out mingcizhu);
                for (int i = 0; i < migcihao.Count; i++)
                {
                    for (int q = 0; q < migcihao[i].Count; q++)
                    {
                        SetHao(migcihao[i][q]-1, mingcizhu[i]);
                    }
                }
                CUIMainManager._MainManager().NET_MingCi(migcihao, mingcizhu);
                return;
            case 2:
                List<List<int>> baoweihao = new List<List<int>>();
                List<int> baoweizhu = new List<int>();
                GetNetBW(out baoweihao, out baoweizhu);
                for (int i = 0; i < baoweihao.Count; i++)
                {
                    for (int q = 0; q < baoweihao[i].Count; q++)
                    {
                        SetHao(baoweihao[i][q]-1, baoweizhu[i]);
                    }
                }
                CUIMainManager._MainManager().NET_BaoWei(baoweihao, baoweizhu);
                return;
        }
    }
    #region 计时
    public Text yazhuTime;
    public int cur_miao;
    public static string FormatSToHMS(int _time)
    {
        int _hour = _time / 3600;
        int _min = 0;
        int _sec = 0;
        if (_hour > 0)
        {
            _min = (_time % 3600) / 60;
            _sec = _min > 0 ? (_time % 3600) % 60 : _time % 3600;
        }
        else
        {
            _min = _time / 60;
            _sec = _min > 0 ? _time % 60 : _time;
        }

        return _hour > 0 ? string.Format("{0:00}:{1:00}:{2:00}", _hour, _min, _sec) : string.Format("{0:00}:{1:00}", _min, _sec);
    }
    public void SetYaZhuTime(int _miao)
    {
        cur_miao = _miao;
        if (cur_miao <= 0) { cur_miao = 0; yazhuTime.text = "00:00"; return; }
        yazhuTime.text = FormatSToHMS(cur_miao);
    }

    //士气秒
    public Text uiShiqi;
    public int cur_ShiqiM;
    public void SetShiQiM(int _miao)
    {
        cur_ShiqiM = _miao;
        if (cur_ShiqiM <= 0) { cur_ShiqiM = 0; CUIMainManager._MainManager().yaZhu.yazhuTime.text = "00:00"; return; }
        if (_miao < 10)
        {
            uiShiqi.text = "00:0" + _miao;
        }
        else
        {
            uiShiqi.text = "00:" + _miao;
        }
    }
    #endregion
    public Transform dogPran;
    public Text xuhao;
    //刷新狗狗数据
    public void RefDogData()
    {
        for (int i = 0; i < CUIMainManager._MainManager().allUseDog.Count; i++)
        {
            Transform tra = dogPran.GetChild(i);
            tra.Find("狗头").gameObject.SetActive(true);
            CUIMainManager._MainManager().HuanTu(tra.Find("狗头").GetComponent<Image>(), CUIMainManager._MainManager().allUseDog[i].imgName);
            CUIMainManager._MainManager().SetNum(tra.Find("战力/战力值").GetComponent<Text>(), CUIMainManager._MainManager().allUseDog[i].fightingNum);
            //tra.Find("胜率").GetComponent<Text>();
            tra.Find("主人/品质值").GetComponent<Text>().text = CUIMainManager._MainManager().allUseDog[i].masterName.ToString();
            tra.Find("速度/品质值").GetComponent<Text>().text = CUIMainManager._MainManager().allUseDog[i].speed.ToString();
            tra.Find("心情/品质值").GetComponent<Text>().text = CUIMainManager._MainManager().allUseDog[i].mood.ToString();
            tra.Find("耐力/品质值").GetComponent<Text>().text = CUIMainManager._MainManager().allUseDog[i].endurance.ToString();
            tra.Find("幸运/品质值").GetComponent<Text>().text = CUIMainManager._MainManager().allUseDog[i].luck.ToString();
        }
    }
    #region 自己押注的展示
    public GameObject 独赢押注展示;
    public void RefDY()
    {
        if (xiazhu_DY.text == "")
        {
            独赢押注展示.transform.Find("几号/号").GetComponent<Text>().text = "无";
            独赢押注展示.transform.Find("ASG/总计").GetComponent<Text>().text = "0";
            独赢押注展示.transform.Find("ASG/总计/Text/几注").GetComponent<TMP_Text>().text = "0";
        }
        else
        {
            独赢押注展示.transform.Find("几号/号").GetComponent<Text>().text = (selected_DY.GetKey() + 1).ToString();
            CUIMainManager._MainManager().SetNum(独赢押注展示.transform.Find("ASG/总计").GetComponent<Text>(), (int.Parse(xiazhu_DY.text) * 100));
            独赢押注展示.transform.Find("ASG/总计/Text/几注").GetComponent<TMP_Text>().text = xiazhu_DY.text;
        }
    }
    public GameObject 名次押注展示;
    bool zhedieMC;
    public Transform MCPrant;
    public Text mcText;
    public void RefYZ()
    {
        //折叠
        CUIMainManager._MainManager().SetNum(名次押注展示.transform.Find("ASG/总计").GetComponent<Text>(), (GetAllBetQian() * 100));
        名次押注展示.transform.Find("ASG/总计/Text/几注").GetComponent<TMP_Text>().text = GetAllBetValue().ToString();
        if (zhedieMC)
        {
            //展开
            Ref(名次押注展示.transform, listBetManager_MC);
        }
    }
    public GameObject 包赢押注展示;
    bool zhedieBY;
    public Transform BYPrant;
    public Text bwText;
    public void RefBY()
    {
        CUIMainManager._MainManager().SetNum(包赢押注展示.transform.Find("ASG/总计").GetComponent<Text>(), (GetAllBetJiBi1() * 100));
        包赢押注展示.transform.Find("ASG/总计/Text/几注").GetComponent<TMP_Text>().text = GetAllBetValue1().ToString();
        if (zhedieBY)
        {
            //展开
            Ref(包赢押注展示.transform, listBetManager_BW);
        }
    }

    void Ref(Transform tra, List<BetManager> list)
    {
        Transform tra2 = tra.transform.Find("Content/2");
        YaZhuData data2M = list[0].Get();
        bool b = false;
        if (data2M == null || b)
        {
            tra2.Find("1/号").GetComponent<Text>().text = "无";
            tra2.Find("2/号").GetComponent<Text>().text = "无";
            tra2.Find("几注").GetComponent<TMP_Text>().text = "0";
        }
        else
        {
            tra2.Find("1/号").GetComponent<Text>().text = data2M.list[0] + "号";
            tra2.Find("2/号").GetComponent<Text>().text = data2M.list[1] + "号";
            tra2.Find("几注").GetComponent<TMP_Text>().text = data2M.zhu + "";
        }

        Transform tra3 = tra.transform.Find("Content/3");
        YaZhuData data3M = list[1].Get();
        if (data3M == null || b)
        {
            tra3.Find("1/号").GetComponent<Text>().text = "无";
            tra3.Find("2/号").GetComponent<Text>().text = "无";
            tra3.Find("3/号").GetComponent<Text>().text = "无";
            tra3.Find("几注").GetComponent<TMP_Text>().text = "0";
        }
        else
        {
            tra3.Find("1/号").GetComponent<Text>().text = data3M.list[0] + "号";
            tra3.Find("2/号").GetComponent<Text>().text = data3M.list[1] + "号";
            tra3.Find("3/号").GetComponent<Text>().text = data3M.list[2] + "号";
            tra3.Find("几注").GetComponent<TMP_Text>().text = data3M.zhu + "";
        }

        Transform tra4 = tra.transform.Find("Content/4");
        YaZhuData data4M = list[2].Get();
        if (data4M == null || b)
        {
            tra4.Find("1/号").GetComponent<Text>().text = "无";
            tra4.Find("2/号").GetComponent<Text>().text = "无";
            tra4.Find("3/号").GetComponent<Text>().text = "无";
            tra4.Find("4/号").GetComponent<Text>().text = "无";
            tra4.Find("几注").GetComponent<TMP_Text>().text = "0";
        }
        else
        {
            tra4.Find("1/号").GetComponent<Text>().text = data4M.list[0] + "号";
            tra4.Find("2/号").GetComponent<Text>().text = data4M.list[1] + "号";
            tra4.Find("3/号").GetComponent<Text>().text = data4M.list[2] + "号";
            tra4.Find("4/号").GetComponent<Text>().text = data4M.list[3] + "号";
            tra4.Find("几注").GetComponent<TMP_Text>().text = data4M.zhu + "";
        }
    }

    public void RefTipsYaZhu()
    {
        switch (tagManager.Cur_SelectedTab)
        {
            case 0:
                独赢押注展示.SetActive(true);
                名次押注展示.SetActive(false);
                包赢押注展示.SetActive(false);
                RefDY();
                break;
            case 1:
                独赢押注展示.SetActive(false);
                名次押注展示.SetActive(true);
                包赢押注展示.SetActive(false);
                break;
            case 2:
                独赢押注展示.SetActive(false);
                名次押注展示.SetActive(false);
                包赢押注展示.SetActive(true);
                break;
        }
    }

    public void BtnMCZheDie()
    {
        zhedieMC = !zhedieMC;
        if (zhedieMC)
        {
            //展开
            名次押注展示.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1054f, 357f), 0.2f).OnComplete(() =>
            {
                RefYZ();
                MCPrant.gameObject.SetActive(true);
                mcText.text = "折叠";
            });
        }
        else
        {
            MCPrant.gameObject.SetActive(false);
            //折叠
            名次押注展示.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1054f, 117f), 0.2f).OnComplete(() =>
            {
                mcText.text = "展开";
            });
        }
    }

    public void BtnBYZheDie()
    {
        zhedieBY = !zhedieBY;
        if (zhedieBY)
        {
            //展开
            包赢押注展示.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1054f, 357f), 0.2f).OnComplete(() =>
            {
                RefBY();
                BYPrant.gameObject.SetActive(true);
                bwText.text = "折叠";
            });
        }
        else
        {
            BYPrant.gameObject.SetActive(false);
            //折叠
            包赢押注展示.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1054f, 117f), 0.2f).OnComplete(() =>
            {
                bwText.text = "展开";
            });
        }
    }
    #endregion
}

