using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CCanZhan : MonoBehaviour
{
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        InitDaoJu();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OorDBar(bool b)
    {
        bar.SetActive(b);
        if (!b)
        {
            CUIMainManager._MainManager().biSai.OorDBar(true);
        }
        else
        {

        }
    }

    #region 宠物列表
    public GameObject fuzhiti;
    public Transform fuwuti;
    //当前选中
    public int selectKey;
    List<DogUIFitData> allData = new List<DogUIFitData>();

    public void RefList()
    {
        allData.Clear();
        for (int i = 0; i < fuwuti.childCount; i++)
        {
            Destroy(fuwuti.GetChild(i).gameObject);
        }
        for (int i = 0; i < CUIMainManager._MainManager().allZhanDouChongWuData.Count; i++)
        {
            DogUIFitData data = new DogUIFitData();
            allData.Add(data);
            ZhanDouChongWuData zddata = CUIMainManager._MainManager().allZhanDouChongWuData[i];
            GameObject obj = Instantiate(fuzhiti, fuwuti);
            obj.name = i.ToString();
            obj.AddComponent<UIEventListener>().OnClick += SelectBtn;
            obj.SetActive(true);
            data.pic = obj.transform.Find("Image").GetComponent<Image>();
            data.name = obj.transform.Find("品种").GetComponent<Text>();
            data.zhanDouLi = obj.transform.Find("战力值").GetComponent<Text>();
            data.pinzhi = obj.transform.Find("品质值").GetComponent<Text>();
            data.sudu = obj.transform.Find("速度值").GetComponent<Text>();
            data.xinqing = obj.transform.Find("心情值").GetComponent<Text>();
            data.naili = obj.transform.Find("耐力值").GetComponent<Text>();
            data.xingyun = obj.transform.Find("幸运值").GetComponent<Text>();
            data.xuanzhong = obj.transform.Find("选中").gameObject;

            data.id = zddata.id;
            //data.masterID = zddata.masterID;
            CUIMainManager._MainManager().HuanTu(data.pic, zddata.imgName);
            data.name.text = zddata.dogName.ToString();
            CUIMainManager._MainManager().SetNum(data.zhanDouLi, zddata.fightingNum);
            data.pinzhi.text = zddata.inbornNum.ToString();
            data.sudu.text = zddata.speed.ToString();
            data.xinqing.text = zddata.mood.ToString();
            data.xingyun.text = zddata.luck.ToString();
            data.naili.text = zddata.endurance.ToString();
            if (zddata.isCool == 1)
            {
                obj.transform.Find("出战").gameObject.SetActive(true);
            }
        }
        SelectBtn(0);
    }

    #endregion

    #region 选中
    //选中
    void SelectBtn(GameObject obj)
    {
        selectKey = int.Parse(obj.name);
        DogUIFitData data = allData[selectKey];
        ZhanDouChongWuData _data = CUIMainManager._MainManager().GetZhanDouChongWuID(data.id);
        if (_data.isCool == 1) { return; }
        for (int i = 0; i < allData.Count; i++)
        {
            allData[i].xuanzhong.SetActive(false);
        }
        data.xuanzhong.SetActive(true);
        SelectShow();
    }
    //选中
    void SelectBtn(int _key)
    {
        selectKey = _key;
        if (allData.Count == 0)
        {
            touxiang.gameObject.SetActive(false);
            mingzi.text = "无";
            pinzhi.text = "无";
            chengzhang.text = "无";
            tianfu.text = "无";
            CUIMainManager._MainManager().HuanBox(1, equipListBox[0]);
            CUIMainManager._MainManager().HuanBox(1, equipListBox[1]);
            CUIMainManager._MainManager().HuanBox(1, equipListBox[2]);
            equipList[0].gameObject.SetActive(false);
            equipList[1].gameObject.SetActive(false);
            equipList[2].gameObject.SetActive(false);
            zhanli.text = "无";
            lv.text = "Lv.1";
            return;
        }
        DogUIFitData data = allData[selectKey];
        ZhanDouChongWuData _data = CUIMainManager._MainManager().GetZhanDouChongWuID(data.id);
        if (_data.isCool == 1) { return; }
        for (int i = 0; i < allData.Count; i++)
        {
            allData[i].xuanzhong.SetActive(false);           
        }
        data.xuanzhong.SetActive(true);
        SelectShow();
    }
    public Text lv;
    public Image touxiang;
    public Text mingzi;
    public Text pinzhi;
    public Text zhanli;
    public Text chengzhang;
    public Text tianfu;
    public List<Image> equipList;
    public List<Image> equipListBox;
    public GameObject chuzhan;
    //选中显示
    void SelectShow()
    {
        DogUIFitData data = allData[selectKey];
        ZhanDouChongWuData _data = CUIMainManager._MainManager().GetZhanDouChongWuID(data.id);
        if (_data.isCool == 1) { return; }
        touxiang.gameObject.SetActive(true);
        touxiang.sprite = data.pic.sprite;
        mingzi.text = _data.dogName.ToString();
        pinzhi.text = _data.dogBreed.ToString();
        chengzhang.text = _data.currentGrowUpNum + "/" + _data.maxGrowUpNum;
        tianfu.text = _data.inbornNum.ToString();
        double lishizhanli = _data.fightingNum;
        CUIMainManager._MainManager().HuanBox(1, equipListBox[0]);
        CUIMainManager._MainManager().HuanBox(1, equipListBox[1]);
        CUIMainManager._MainManager().HuanBox(1, equipListBox[2]);
        for (int i = 0; i < _data.zhuangbei.Length; i++)
        {
            if (_data.zhuangbei[i] != 0)
            {
                ZhuangBeiData zhuangBeiData = CUIMainManager._MainManager().GetIDZDZBData(_data.zhuangbei[i]);
                equipList[i].gameObject.SetActive(true);
                CUIMainManager._MainManager().HuanBox(zhuangBeiData.grade, equipListBox[i]);
                CUIMainManager._MainManager().HuanTu(equipList[i], zhuangBeiData.img);
                if (zhuangBeiData.ratio != 0)
                {
                    lishizhanli = lishizhanli + lishizhanli * (zhuangBeiData.ratio/100.0f);
                }
                else
                {
                    lishizhanli = lishizhanli + zhuangBeiData.fightingNum*_data.inbornNum;
                }
            }
            else
            {
                equipList[i].gameObject.SetActive(false);
            }
        }
        CUIMainManager._MainManager().SetNum(data.zhanDouLi, lishizhanli);
        CUIMainManager._MainManager().SetNum(zhanli, lishizhanli);
        lv.text = "Lv." + _data.dogGrade;
        //if (_data.isCool == 1) { chuzhan.SetActive(true); }
        //else { chuzhan.SetActive(false); }
    }
    //选中信息关闭关闭
    public void EquipDownBtn()
    {
        for (int i = 0; i < equipList.Count; i++)
        {
            equipList[i].gameObject.SetActive(false);
        }
    }

    #endregion

    #region 装备页面
    public GameObject equipPage;
    public GridUIManager gridUIManager;
    public void OOreDownEquipPage(bool b)
    {
        equipPage.SetActive(b);
        if (b)
        {
            CUIMainManager._MainManager().NET_EquipAll();
        }
    }
    public void RefEquipPage()
    {
        gridUIManager.AddAllClear();
        for (int i = 0; i < CUIMainManager._MainManager().allZhuangDZBeiData.Count; i++)
        {
            gridUIManager.AddDataDog(CUIMainManager._MainManager().allZhuangDZBeiData[i].id, i,
                                     CUIMainManager._MainManager().allZhuangDZBeiData[i].name,
                                     CUIMainManager._MainManager().allZhuangDZBeiData[i].img,
                                     CUIMainManager._MainManager().allZhuangDZBeiData[i].isNft,
                                     CUIMainManager._MainManager().allZhuangDZBeiData[i].grade,
                                     CUIMainManager._MainManager().allZhuangDZBeiData[i].isUse);
        }
        if (CUIMainManager._MainManager().allZhuangDZBeiData.Count == 0)
        {
            tupian.gameObject.SetActive(false);
            //名字
            e_name.text = "无";
            //类型
            shuoming.text = "无";
            //作用
            zuoyong.text = "无";
        }
        gridUIManager.SelectBox(0, 1);
    }
    public Text e_lv;
    //图片
    public Image box;
    //图片
    public Image tupian;
    //耐久
    public Text e_naijiu;
    //名字
    public Text e_name;
    //说明
    public Text shuoming;
    //作用
    public Text zuoyong;
    //刷新选中介绍
    public void RefDataShow()
    {
        ZhuangBeiData data = null;
        if (CUIMainManager._MainManager().allZhuangDZBeiData.Count > gridUIManager.key)
        {
            data = CUIMainManager._MainManager().allZhuangDZBeiData[gridUIManager.key];
        }
        if (data != null)
        {
            Debug.LogError(gridUIManager.GetID());
            //图片
            tupian.gameObject.SetActive(true);
            CUIMainManager._MainManager().HuanTu(tupian,data.img);
            //名字
            e_name.text = data.name;
            //类型
            shuoming.text = "战斗装备";
            //作用
            zuoyong.text = data.desc;
            e_naijiu.gameObject.SetActive(true);
            e_naijiu.text = data.durabilityResidue + "/" + data.durabilityMax;
            CUIMainManager._MainManager().HuanBox(data.grade, box);
            e_lv.text = "Lv." + data.grade;
        }
        else
        {
            tupian.gameObject.SetActive(false);
            //名字
            e_name.text = "无";
            //类型
            shuoming.text = "无";
            //作用
            zuoyong.text = "无";
            e_naijiu.gameObject.SetActive(false);
            CUIMainManager._MainManager().HuanBox(1, box);
            e_lv.text = "Lv.1";
        }
    }
    //穿戴
    public void Btn_Equipment()
    {
        DogUIFitData data = allData[selectKey];
        ZhanDouChongWuData _data = CUIMainManager._MainManager().GetZhanDouChongWuID(data.id);
        ZhuangBeiData zhuangBeiData = CUIMainManager._MainManager().GetIDZDZBData(gridUIManager.GetID());
        //给宠物穿装备
        _data.zhuangbei[zhuangBeiData.type - 1] = zhuangBeiData.id;

        SelectShow();
        OOreDownEquipPage(false);
    }
    #endregion

    #region 道具使用
    public Transform daojuTra;
    List<DataUsing> allUsing = new List<DataUsing>();
    public void InitDaoJu()
    {
        for (int i = 0; i < daojuTra.childCount; i++)
        {
            DataUsing data = new DataUsing();
            allUsing.Add(data);
            data.obj = daojuTra.GetChild(i).gameObject;
            daojuTra.GetChild(i).gameObject.AddComponent<UIEventListener>().OnClick += DaoJuOpen;
            //pic
            data.pic = daojuTra.GetChild(i).Find("pic").GetComponent<Image>();
            data.id = i + 2;
            //开关
            data.guan = daojuTra.GetChild(i).Find("开/关").gameObject;
            GameObject kai = daojuTra.GetChild(i).Find("开").gameObject;
            //数量
            data.count = daojuTra.GetChild(i).Find("Text").GetComponent<Text>();
            //名字
            data.name = daojuTra.GetChild(i).Find("name").GetComponent<Text>();
        }
    }
    public void RefDatJu()
    {
        for (int q = 0; q < allUsing.Count; q++)
            allUsing[q].obj.SetActive(false);
        for (int i = 0; i < CUIMainManager._MainManager().allFightItem.Count; i++)
        {
            FightData data = CUIMainManager._MainManager().allFightItem[i];
            allUsing[i].id = data.id;
            allUsing[i].obj.SetActive(true);
            CUIMainManager._MainManager().HuanTu(allUsing[i].pic, data.imgName);
            allUsing[i].SetNum(data.propNum);
            allUsing[i].name.text = data.propName;
        }
    }
    public void DaoJuOpen(GameObject obj)
    {
        int key = int.Parse(obj.name);
        if (allUsing[key].num == 0)
        {
            CUIMainManager._MainManager().cUITips.Tips("没有此道具");
            return;
        }
        allUsing[key].SetDown();
    }
    #endregion

    public bool isCanZhan;
    public int curID;
    //参赛
    public void CanSaiBtn()
    {
        if (isCanZhan) {
            CUIMainManager._MainManager().cUITips.Tips("正在报名请稍后...");
            return;
        }
        DogUIFitData data = allData[selectKey];
        ZhanDouChongWuData _data = CUIMainManager._MainManager().GetZhanDouChongWuID(data.id);
        curID = _data.id;
        CUIMainManager._MainManager().NET_BaoMing(_data.id, _data.zhuangbei, allUsing);
        isCanZhan = true;
    }
}
//宠物战斗属性
public class DogUIFitData
{
    public int id;
    public int masterID;
    //图片
    public Image pic;
    //名字
    public Text name;
    //主人名字
    public Text masterName;
    //战斗力
    public Text zhanDouLi;
    //品质
    public Text pinzhi;
    //速度
    public Text sudu;
    //心情
    public Text xinqing;
    //耐力
    public Text naili;
    //幸运
    public Text xingyun;
    //士气
    public Text shiqi;
    //选中
    public GameObject xuanzhong;
}
//道具使用
public class DataUsing
{
    public GameObject obj;
    public int id;
    public Image pic;
    public Text count;
    public Text name;
    public int num;
    public GameObject guan;
    public bool isUse;
    public void SetDown()
    {
        isUse = !isUse;
        guan.SetActive(!isUse);
    }
    public void SetNum(int value)
    {
        num = value;
        count.text = value.ToString();
    }
}
