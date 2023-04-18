using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CSiYang : MonoBehaviour
{
    public GameObject bar;
    public GameObject chongwuBar;
    public GameObject daojuBar;
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
        if (!b)
        {
            CUIMainManager._MainManager().mainBar.OorDBar(true);
        }
        else
        {
            SetTagSelect(0);
        }
    }
    //刷新物品显示
    public void RefItemShow()
    {
        if (tagManager.Cur_SelectedTab == 0)
        {
            for (int i = 0; i < CUIMainManager._MainManager().AllChongWuData.Count; i++)
            {
                gridUIManager.AddDataDog(CUIMainManager._MainManager().AllChongWuData[i].id, i,
                                         CUIMainManager._MainManager().AllChongWuData[i].dogName,
                                         CUIMainManager._MainManager().AllChongWuData[i].imgName,
                                         CUIMainManager._MainManager().AllChongWuData[i].isNft,
                                         CUIMainManager._MainManager().AllChongWuData[i].dogGrade,
                                         CUIMainManager._MainManager().AllChongWuData[i].isUse);
            }
        }
        else
        {
            List<ItemData> list = CUIMainManager._MainManager().GetItemData(tagManager.Cur_SelectedTab);

            for (int i = 0; i < list.Count; i++)
            {
                gridUIManager.AddDataDog(list[i].id, i,
                                         list[i].name,
                                         list[i].img,
                                         list[i].num,
                                         list[i].isNft,
                                         list[i].grade,
                                         list[i].isUse);
            }
        }
        gridUIManager.SelectBox(0, 1);
    }
    public GridUIManager gridUIManager;
    #region 切换页签
    //当前页签管理器
    public TagManager tagManager;
    //选中标签  刷新页面
    public void SetTagSelect(int key1)
    {
        gridUIManager.AddAllClear();
        //修改页签值
        tagManager.Cur_SelectedTab = key1;
        //UI切换页签显示
        tagManager.SetSelect();
        //申请消息
        CUIMainManager._MainManager().NET_GetItemData(tagManager.Cur_SelectedTab + 1);
        //关闭属性介绍
        DownDataShow();
    }
    #endregion



    #region 选中物品属性
    public Text lv;
    public Image box;
    //图片
    public Image tupian;
    //名字
    public Text mingzi;
    //说明
    public Text shuoming;
    //作用
    public Text zuoyong;
    //刷新选中物品属性展示
    void RefCurWuPin()
    {
        ItemData data = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
        if (data != null)
        {
            //图片
            tupian.gameObject.SetActive(true);
            tupian.sprite = gridUIManager.allGridData[gridUIManager.key].pic.sprite;
            //名字
            mingzi.text = data.name;
            //类型
            shuoming.text = data.name.ToString();
            //作用
            zuoyong.text = data.desc.ToString();
            CUIMainManager._MainManager().HuanBox(data.grade, box);
            lv.text = "Lv." + data.grade;
        }
        else
        {
            tupian.gameObject.SetActive(false);
            //名字
            mingzi.text = "无";
            //类型
            shuoming.text = "无";
            //作用
            zuoyong.text = "无";
            CUIMainManager._MainManager().HuanBox(1, box);
            lv.text = "Lv.1";
        }

    }
    #endregion

    #region 选中宠物属性
    public Text cw_lv;
    public Image cw_touxiang;
    public Text cw_mingzi;
    public Text cw_pinzhi;
    public Text cw_zhanli;
    public Text cw_chengzhang;
    public Text cw_tianfu;
    public GameObject kesiyang;
    public GameObject bukesiyang;
    public GameObject tunshiBtn;
    //刷新选中物品属性展示
    void RefCurCW()
    {
        ChongWuData data = CUIMainManager._MainManager().GetIDChongWuData(gridUIManager.GetID());
        //可吞噬
        tunshiBtn.SetActive(false);
        //可更换
        gengHuanBtn.SetActive(false);
        cw_touxiang.gameObject.SetActive(false);
        cw_mingzi.text = "无";
        cw_pinzhi.text = "无";
        cw_chengzhang.text = "无";
        cw_zhanli.text = "无";
        cw_lv.text = "Lv.1";
        cw_tianfu.text = "无";
        kesiyang.SetActive(false);
        bukesiyang.SetActive(false);
        if (data != null)
        {
            //当前宠物是出战宠物
            if (data.isUse == 1)
            {
                //不可吞噬
                tunshiBtn.SetActive(true);
                //不可更换
                gengHuanBtn.SetActive(true);
            }
            ChongWuData chongWuData = CUIMainManager._MainManager().GetIDChongWuData();
            //没有出战宠物
            if (chongWuData == null)
            {
                //不可吞噬
                tunshiBtn.SetActive(true);
                //可更换
                gengHuanBtn.SetActive(false);
            }
            //图片
            cw_touxiang.gameObject.SetActive(true);
            cw_touxiang.sprite = gridUIManager.allGridData[gridUIManager.key].pic.sprite;
            //名字
            cw_mingzi.text = data.dogName;
            //品质
            cw_pinzhi.text = data.dogBreed.ToString();
            //战力
            CUIMainManager._MainManager().SetNum(cw_zhanli, data.fightingNum);
            //成长
            cw_chengzhang.text = data.currentGrowUpNum + "/" + data.maxGrowUpNum;
            //天赋
            cw_tianfu.text = data.inbornNum.ToString();

            if (data.isCool == 1)
            {
                kesiyang.SetActive(false);
                bukesiyang.SetActive(true);
            }
            else
            {
                if (data.currentGrowUpNum == data.maxGrowUpNum)
                {
                    kesiyang.SetActive(false);
                    bukesiyang.SetActive(true);
                }
            }
            cw_lv.text = "Lv." + data.dogGrade;
        }
    }
    #endregion

    #region 当前出战宠物属性
    public Text tisheng;
    public Text m_lv;
    public Image m_touxiang;
    public Text m_mingzi;
    public Text m_pinzhi;
    public Text m_zhanli;
    public Text m_chengzhang;
    public Text m_tianfu;
    public GameObject m_kesiyang;
    public GameObject m_bukesiyang;
    public GameObject gengHuanBtn;
    //刷新出战宠物属性展示
    public void RefMCW()
    {
        ChongWuData data = CUIMainManager._MainManager().GetIDChongWuData();
        if (data != null)
        {
            //图片
            m_touxiang.gameObject.SetActive(true);
            CUIMainManager._MainManager().HuanTu(m_touxiang, data.imgName);
            //名字
            m_mingzi.text = data.dogName;
            //品质
            m_pinzhi.text = data.dogBreed.ToString();
            //战力
            CUIMainManager._MainManager().SetNum(m_zhanli, data.fightingNum);
            //成长
            m_chengzhang.text = data.currentGrowUpNum + "/" + data.maxGrowUpNum;
            //天赋
            m_tianfu.text = data.inbornNum.ToString();
            m_kesiyang.SetActive(true);
            m_bukesiyang.SetActive(false);
            if (data.isCool == 1)
            {
                m_kesiyang.SetActive(false);
                m_bukesiyang.SetActive(true);
            }
            else
            {
                if (data.currentGrowUpNum == data.maxGrowUpNum)
                {
                    m_kesiyang.SetActive(false);
                    m_bukesiyang.SetActive(true);
                }
            }
            m_lv.text = "Lv." + data.dogGrade;
            tisheng.text = "0";
            if (tagManager.Cur_SelectedTab == 0)
            {
                ChongWuData data1 = CUIMainManager._MainManager().GetIDChongWuData(gridUIManager.GetID());
                if (data1 != null)
                {
                    if (data1.isUse != 1)
                    {
                        tisheng.text = (data.fightingNum + data1.fightingNum * data.inbornNum).ToString();
                        CUIMainManager._MainManager().SetNum(tisheng, (data.fightingNum + data1.fightingNum * data.inbornNum));
                    }
                }
            }
            else
            {
                ItemData data1 = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
                if (data1 != null)
                {
                    if (data1.isIgnoreTalent == 1)
                    {
                        //不受天赋影响
                        CUIMainManager._MainManager().SetNum(tisheng, data.fightingNum +  data1.fightingNum);
                    }
                    else
                    {
                        //受天赋影响
                        CUIMainManager._MainManager().SetNum(tisheng, (data.fightingNum + data1.fightingNum * data.inbornNum));
                    }
                }
            }
        }
        else
        {
            m_kesiyang.SetActive(false);
            m_bukesiyang.SetActive(false);
            m_touxiang.gameObject.SetActive(false);
            //名字
            m_mingzi.text = "无";
            //品质
            m_pinzhi.text = "无";
            //战力
            m_zhanli.text = "无";
            //成长
            m_chengzhang.text = "无";
            //天赋
            m_tianfu.text = "无";
            m_kesiyang.SetActive(false);
            m_bukesiyang.SetActive(false);
            m_lv.text = "Lv.1";
            tisheng.text = "0";
        }
    }
    #endregion

    //刷新选中介绍
    public void RefDataShow()
    {
        DownDataShow();
        //宠物
        if (tagManager.Cur_SelectedTab == 0)
        {
            RefCurCW();
            RefMCW();
        }
        else
        {
            RefCurWuPin();
            RefMCW();
        }
    }
    //关闭选中该介绍
    void DownDataShow()
    {
        //宠物
        if (tagManager.Cur_SelectedTab == 0)
        {
            daojuBar.SetActive(false);
            chongwuBar.SetActive(true);
            cw_touxiang.gameObject.SetActive(false);
            //名字
            cw_mingzi.text = "无";
            //品质
            cw_pinzhi.text = "无";
            //战力
            cw_zhanli.text = "无";
            //成长
            cw_chengzhang.text = "无";
            //天赋
            cw_tianfu.text = "无";

            //名字
            m_mingzi.text = "无";
            //品质
            m_pinzhi.text = "无";
            //战力
            m_zhanli.text = "无";
            //成长
            m_chengzhang.text = "无";
            //天赋
            m_tianfu.text = "无";
        }
        else
        {
            daojuBar.SetActive(true);
            chongwuBar.SetActive(false);
            //图片
            tupian.gameObject.SetActive(false);
            //名字
            mingzi.text = "无";
            //类型
            shuoming.text = "无";
            //作用
            zuoyong.text = "无";
        }
    }




    //宠物更换
    public void Btn_GengHuan()
    {
        CUIMainManager._MainManager().NET_UpdateChonwuDate(gridUIManager.GetID());
    }
    //物品使用
    public void Btn_Use()
    {
        if (gridUIManager.key == -1) return;
        ItemData data = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
        if (data.type == 2)
        {
            //if (CUIMainManager._MainManager().GetDogID() == -1)
            //{
            //    CUIMainManager._MainManager().Tips("当前没有狗狗出战");
            //}
            CUIMainManager._MainManager().NET_Use(CUIMainManager._MainManager().GetDogID(), gridUIManager.GetID());
        }
        else
        {
            if (CUIMainManager._MainManager().GetDogID() == -1)
            {
                CUIMainManager._MainManager().cUITips.Tips("无法使用\n当前没有狗狗出战");
                return;
            }
            CUIMainManager._MainManager().NET_Raise(tagManager.Cur_SelectedTab - 1, gridUIManager.GetID(), CUIMainManager._MainManager().GetDogID());
        }

    }
    //吞噬
    public void Btn_TunShi()
    {
        CUIMainManager._MainManager().NET_TunShi(gridUIManager.GetID());
    }
}
