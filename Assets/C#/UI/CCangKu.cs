using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CCangKu : MonoBehaviour
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
        if (tagManager.Cur_SelectedTab == 4)
        {
            for (int i = 0; i < CUIMainManager._MainManager().allZhuangBeiData.Count; i++)
            {
                gridUIManager.AddDataDog(CUIMainManager._MainManager().allZhuangBeiData[i].id, i,
                                         CUIMainManager._MainManager().allZhuangBeiData[i].name,
                                         CUIMainManager._MainManager().allZhuangBeiData[i].img,
                                          CUIMainManager._MainManager().allZhuangBeiData[i].isNft,
                                           CUIMainManager._MainManager().allZhuangBeiData[i].grade,
                                         CUIMainManager._MainManager().allZhuangBeiData[i].isUse);
            }
            gridUIManager.SelectBox(0, 1);
            return;
        }
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
        //申请
        CUIMainManager._MainManager().NET_GetItemData(tagManager.Cur_SelectedTab + 1);
        //RefItemShow();

        //选中全部关闭
        gridUIManager.SelectAllDown();
        //关闭属性介绍
        DownDataShow();

    }
    #endregion

    #region 物品选择
    public GridUIManager gridUIManager;
    //确定使用
    public void QueDingGengHuan()
    {
        //UI 显示
        gridUIManager.UseBox();
        //临时全部修改
        //for (int i = 0; i < CUIMainManager._MainManager().allbuZhuoDaoJuData.Count; i++)
        //{
        //    CUIMainManager._MainManager().allbuZhuoDaoJuData[i].isUse = false;
        //}
        //属性修改
    }

    #endregion

    #region 物品介绍

    #region 物品
    public GameObject isTiQu;
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
    //耐久
    public Text naijiu;
    //NFT
    public GameObject nft_wp;
    //丢弃
    public GameObject diuqi_wp;
    #endregion

    #region 宠物
    public GameObject cw_isTiQu;
    public Text cw_lv;
    public Image cw_touxiang;
    public Text cw_mingzi;
    public Text cw_pinzhi;
    public Text cw_zhanli;
    public Text cw_chengzhang;
    public Text cw_tianfu;
    //NFT
    public GameObject nft_cw;
    //丢弃
    public GameObject diuqi_cw;
    #endregion

    //刷新选中介绍
    public void RefDataShow()
    {
        DownDataShow();
        if (tagManager.Cur_SelectedTab == 4)
        {
            ZhuangBeiData data = CUIMainManager._MainManager().GetIDZBData(gridUIManager.GetID());
            //图片
            tupian.gameObject.SetActive(true);
            tupian.sprite = gridUIManager.allGridData[gridUIManager.key].pic.sprite;
            //名字
            mingzi.text = data.name;
            //类型
            switch (data.nftType)
            {
                case 1:
                    shuoming.text = "宠物";
                    break;
                case 2:
                    shuoming.text = "道具";
                    break;
                case 3:
                    shuoming.text = "饲料";
                    break;
                case 4:
                    shuoming.text = "野生";
                    break;
                case 5:
                    shuoming.text = "捕捉装备";
                    break;
                case 6:
                    shuoming.text = "对战装备";
                    break;
            }
            //作用
            zuoyong.text = data.desc;
            if (data.extraOneDesc.Length > 6)
            {
                shuoming.text = shuoming.text + "\n" + data.extraOne;
            }
            if (data.extraTwoDesc.Length > 6)
            {
                shuoming.text = shuoming.text + "\n" + data.extraTwo;
            }
            //耐久
            naijiu.gameObject.SetActive(true);
            naijiu.text = data.durabilityResidue + "/" + data.durabilityMax;
            //框
            CUIMainManager._MainManager().HuanBox(data.grade, box);
            lv.text = "Lv." + data.grade;
            //nft
            if (data.isNft == 0) { nft_wp.SetActive(false); isTiQu.SetActive(true); }
            else { nft_wp.SetActive(true); isTiQu.SetActive(false); }
            //提取
            if (data.isDraw == 1)
            {
                isTiQu.SetActive(true);
            }
            return;
        }
        //宠物
        if (tagManager.Cur_SelectedTab == 0)
        {
            ChongWuData data = CUIMainManager._MainManager().GetIDChongWuData(gridUIManager.GetID());
            //图片
            cw_touxiang.gameObject.SetActive(true);
            cw_touxiang.sprite = gridUIManager.allGridData[gridUIManager.key].pic.sprite;
            //名字
            cw_mingzi.text = data.dogName;
            //品种
            cw_pinzhi.text = data.dogBreed.ToString();
            //战力
            CUIMainManager._MainManager().SetNum(cw_zhanli, data.fightingNum);
            //成长
            cw_chengzhang.text = data.currentGrowUpNum + "/" + data.maxGrowUpNum;
            //天赋
            cw_tianfu.text = data.inbornNum.ToString();
            cw_lv.text = "Lv." + data.dogGrade;
            //nft
            if (data.isNft == 0) { nft_cw.SetActive(false); cw_isTiQu.SetActive(true); }
            else { nft_cw.SetActive(true); cw_isTiQu.SetActive(false); }
            //宠物提取
            if (data.isDraw == 1)
            {
                cw_isTiQu.SetActive(true);
            }
        }
        else
        {
            ItemData data = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
            //图片
            tupian.gameObject.SetActive(true);
            tupian.sprite = gridUIManager.allGridData[gridUIManager.key].pic.sprite;
            //名字
            mingzi.text = data.name;
            //类型
            shuoming.text = data.name;
            //作用
            zuoyong.text = data.desc;
            //耐久
            naijiu.gameObject.SetActive(false);
            //框
            CUIMainManager._MainManager().HuanBox(data.grade, box);
            lv.text = "Lv." + data.grade;
            //nft
            if (data.isNft == 0) { nft_wp.SetActive(false); isTiQu.SetActive(true); }
            else { nft_wp.SetActive(true); isTiQu.SetActive(false); }
            //提取
            if (data.isDraw == 1)
            {
                isTiQu.SetActive(true);
            }
        }
    }
    //关闭选中该介绍
    void DownDataShow()
    {
        if (tagManager.Cur_SelectedTab == 4)
        {
            chongwuBar.SetActive(false);
            daojuBar.SetActive(true);
            lv.text = "Lv.1";
            //图片
            tupian.gameObject.SetActive(false);
            //名字
            mingzi.text = "无";
            //类型
            shuoming.text = "无";
            //作用
            zuoyong.text = "无";
            CUIMainManager._MainManager().HuanBox(1, box);
            nft_wp.SetActive(false);
            return;
        }
        //宠物
        if (tagManager.Cur_SelectedTab == 0)
        {
            chongwuBar.SetActive(true);
            daojuBar.SetActive(false);
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
            cw_lv.text = "Lv.1";
            nft_cw.SetActive(false);
        }
        else
        {
            chongwuBar.SetActive(false);
            daojuBar.SetActive(true);

            //图片
            tupian.gameObject.SetActive(false);
            //名字
            mingzi.text = "无";
            //类型
            shuoming.text = "无";
            //作用
            zuoyong.text = "无";
            CUIMainManager._MainManager().HuanBox(1, box);
            lv.text = "Lv.1";
            nft_wp.SetActive(false);
        }
    }
    //NTF
    public void Btn_NTF()
    {
        if (tagManager.Cur_SelectedTab == 0)
        {
            //宠物铸造
            ChongWuData data = CUIMainManager._MainManager().GetIDChongWuData(gridUIManager.GetID());
            CUIMainManager._MainManager().NET_NFT(data.nftType, data.id);
        }
        else
        {
            if (tagManager.Cur_SelectedTab == 4)
            {
                //装备铸造
                ZhuangBeiData data = CUIMainManager._MainManager().GetIDZBData(gridUIManager.GetID());
                CUIMainManager._MainManager().NET_NFT(data.nftType, data.id);
            }
            else
            {
                //物品铸造
                ItemData data = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
                CUIMainManager._MainManager().NET_NFT(data.nftType, data.id);
            }
        }
    }
    //丢弃
    public void Btn_DiuQi()
    {
        if (tagManager.Cur_SelectedTab == 0)
        {
            ChongWuData data = CUIMainManager._MainManager().GetIDChongWuData(gridUIManager.GetID());
            if (data.isUse == 0)
            {
                CUIMainManager._MainManager().NET_DiuQi(gridUIManager.GetID(), data.discardType);
            }
            else
            {
                CUIMainManager._MainManager().cUITips.Tips("不可丢弃\n当前宠物为出战状态");
            }
        }
        else
        {
            if (tagManager.Cur_SelectedTab == 4)
            {
                //装备铸造
                ZhuangBeiData data = CUIMainManager._MainManager().GetIDZBData(gridUIManager.GetID());
                CUIMainManager._MainManager().NET_DiuQi(gridUIManager.GetID(), data.discardType);
            }
            else
            {
                //物品铸造
                ItemData data = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
                CUIMainManager._MainManager().NET_DiuQi(gridUIManager.GetID(), data.discardType);
            }
        }
    }
    //提取
    public void Btn_TiQu()
    {
        if (tagManager.Cur_SelectedTab == 0)
        {
            //宠物铸造
            ChongWuData data = CUIMainManager._MainManager().GetIDChongWuData(gridUIManager.GetID());
            CUIMainManager._MainManager().NET_NFTOut(data.nftType, data.id);
        }
        else
        {
            if (tagManager.Cur_SelectedTab == 4)
            {
                //装备铸造
                ZhuangBeiData data = CUIMainManager._MainManager().GetIDZBData(gridUIManager.GetID());
                CUIMainManager._MainManager().NET_NFTOut(data.nftType, data.id);
            }
            else
            {
                //物品铸造
                ItemData data = CUIMainManager._MainManager().GetIDItemData(gridUIManager.GetID());
                CUIMainManager._MainManager().NET_NFTOut(data.nftType, data.id);
            }
        }
       
    }
    #endregion
}
