using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CShiJie : MonoBehaviour
{
    public GameObject bar;
    void Start()
    {
        Init();
    }
    void Update()
    {
    }
    public void OorDBar(bool b)
    {
        bar.SetActive(b);
        if (b)
        {
            if (cur_key != -1)
            {
                SetMapData(cur_key);
            }
            MapBtnInt();
        }
        else
        {
            cur_key = -1;
        }
    }

    #region 世界地图
    public List<Image> allMapPic = new List<Image>();
    public List<Text> allMapName = new List<Text>();
    public List<GameObject> allMapsuo = new List<GameObject>();
    public List<Text> allMapLv = new List<Text>();
    void MapBtnInt()
    {
        for (int i = 0; i < CUIMainManager._MainManager().allMapData.Count; i++)
        {
            allMapName[i].text = CUIMainManager._MainManager().allMapData[i].mapName;
            allMapLv[i].text ="Lv."+ (i + 1).ToString();
            Image _image = allMapPic[i];
            if (CUIMainManager._MainManager().allMapData[i].isOpen)
            {
                CUIMainManager._MainManager().HuanTu(_image, "map_" + (i + 1) + "-" + 1);
                if (CUIMainManager._MainManager().mainDataInfo.mapId == i + 1)
                {
                    CUIMainManager._MainManager().HuanTu(_image, "map_" + (i + 1) + "-" + 2);
                }
                allMapsuo[i].SetActive(false);
            }
            else
            {
                allMapsuo[i].SetActive(true);
                CUIMainManager._MainManager().HuanTu(_image, "map_" + (i + 1) + "-" + 3);
            }
        }
    }

    #endregion

    #region 开启条件
    public GameObject obj;
    public Transform tra;
    List<TiaoJianData> allTiaoJian = new List<TiaoJianData>();
    public GameObject weijiesuo;
    MapData cur_MapData;
    public int cur_key = -1;
    public Text mingzi;
    public void Init()
    {
        MapBtnInt();
        for (int i = 0; i < tra.childCount; i++)
        {
            TiaoJianData data = new TiaoJianData();
            allTiaoJian.Add(data);
            data.kaiqi = tra.GetChild(i).Find("拥有").gameObject;
            data.weikaiqi = tra.GetChild(i).Find("未拥有").gameObject;
            data.goumai = tra.GetChild(i).Find("前往购买").gameObject;
            data.img = tra.GetChild(i).Find("物品图片").GetComponent<Image>();
            data.name = tra.GetChild(i).Find("Text").GetComponent<Text>();
        }
    }
    public void SetMapData(int key)
    {
        cur_key = key;
        cur_MapData = CUIMainManager._MainManager().allMapData[key];
        if (cur_MapData.isOpen)
        {
            CUIMainManager._MainManager().NET_GetMapDate(cur_MapData.id);
            return;
        }
        if (key!=0)
        {
            MapData shanyige_MapData = CUIMainManager._MainManager().allMapData[key-1];
            if (!shanyige_MapData.isOpen)
            {
                CUIMainManager._MainManager().cUITips.Tips("请按顺序依次解锁地图");
                return;
            }
        }
       
        mingzi.text = cur_MapData.mapName;
        obj.SetActive(true);
        int value = 0;
        for (int i = 0; i < cur_MapData.catchPropMap.Length; i++)
        {
            ConditionsData conditionsData = cur_MapData.catchPropMap[i];
            BuZhuoDaoJuData data = CUIMainManager._MainManager().GetCurCathEquip(conditionsData.name);
            if (conditionsData.isHave && data != null)
            {
                allTiaoJian[i].kaiqi.SetActive(true);
                allTiaoJian[i].weikaiqi.SetActive(false);
                allTiaoJian[i].goumai.SetActive(false);
                value++;
            }
            else
            {
                if (conditionsData.isHave)
                {
                    allTiaoJian[i].goumai.GetComponent<Text>().text = "前往装备";
                }
                else
                {
                    allTiaoJian[i].goumai.GetComponent<Text>().text = "前往购买";
                }
                allTiaoJian[i].kaiqi.SetActive(false);
                allTiaoJian[i].weikaiqi.SetActive(true);
                allTiaoJian[i].goumai.SetActive(true);
            }
            CUIMainManager._MainManager().HuanTu(allTiaoJian[i].img, conditionsData.image);
            allTiaoJian[i].name.text = conditionsData.name;
        }
        if (value == 4)
        {
            weijiesuo.SetActive(false);
        }
        else
        {
            weijiesuo.SetActive(true);
        }
    }
    public void GuanBi()
    {
        obj.SetActive(false);
    }
    public void Btn_JieSuo()
    {
        CUIMainManager._MainManager().NET_GetMapDeblock(cur_MapData.id);
    }
    #endregion
    public GameObject goumaiBar;
    public TMP_Text usdt;
    public ConditionsData curConditionsData;
    public void Btn_GouMai(int key)
    {
        curConditionsData = cur_MapData.catchPropMap[key];
        if (curConditionsData.isHave)
        {
            OorDBar(false);
            CUIMainManager._MainManager().buZhuoDaoJu.SetTagSelect(curConditionsData.type-1);
            CUIMainManager._MainManager().buZhuoDaoJu.OorDBar(true);
            obj.SetActive(false);
            return;
        }
        OorDownGouMaiBar(true);
    }
    public void OorDownGouMaiBar(bool b)
    {
        goumaiBar.SetActive(b);
        if (b)
        {
            usdt.text = curConditionsData.Unum.ToString();
        }
    }
    public void Btn_Buy()
    {
        CUIMainManager._MainManager().NET_Buy(curConditionsData.id);
    }
}

public class TiaoJianData
{
    public GameObject kaiqi;
    public GameObject weikaiqi;
    public Image img;
    public Text name;
    public GameObject goumai;
}