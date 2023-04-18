using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CBUZhuoDaoJu : MonoBehaviour
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
        if (!b)
        {
            CUIMainManager._MainManager().mainBar.OorDBar(true);
        }
        else
        {
            //SetTagSelect(tagManager.Cur_SelectedTab);
           
        }
    }
    //刷新物品显示
    public void RefItemShow()
    {
        for (int i = 0; i < CUIMainManager._MainManager().buzhuodaojuList.Count; i++)
        {
            gridUIManager.AddDataDog(CUIMainManager._MainManager().buzhuodaojuList[i].id, 
                i, CUIMainManager._MainManager().buzhuodaojuList[i].equipName,
                CUIMainManager._MainManager().buzhuodaojuList[i].imgName, 
                CUIMainManager._MainManager().buzhuodaojuList[i].equipNum,
                CUIMainManager._MainManager().buzhuodaojuList[i].isNft,
                CUIMainManager._MainManager().buzhuodaojuList[i].grade,
                CUIMainManager._MainManager().buzhuodaojuList[i].isUse);
        }
        gridUIManager.SelectBox(0,1);
    }
    #region 切换页签
    //当前页签管理器
    public TagManager tagManager;
    //选中标签
    public void SetTagSelect(int key1)
    {
        gridUIManager.AddAllClear();
        //修改页签值
        tagManager.Cur_SelectedTab = key1;
        //UI切换页签显示
        tagManager.SetSelect();
        //显示当前页签里边有什么
        //RefItemShow();
        Debug.LogError("申请"+tagManager.Cur_SelectedTab + 1);
        CUIMainManager._MainManager().NET_BuZhuoListDate(tagManager.Cur_SelectedTab+1);

        //选中全部关闭
        gridUIManager.SelectAllDown();
        //关闭属性介绍
        DownDataShow();
    }
    #endregion

    #region 物品选择
    public GridUIManager gridUIManager;

    //确定使用按钮
    public void QueDingGengHuan()
    {
        CUIMainManager._MainManager().NET_CatchEquipUse(gridUIManager.GetID());
    }
    //卸下
    public void TuoXia()
    {
        CUIMainManager._MainManager().NET_CatchEquipUnsnatch(gridUIManager.GetID());
    }
    #endregion

    #region 物品介绍
    public Text pinzhi;
    public Image box;
    //图片
    public Image tupian;
    //耐久
    public Text naijiu;
    //名字
    public Text mingzi;
    //说明
    public Text shuoming;
    //作用
    public Text zuoyong;
    public Text shiyong;
    //已装备
    public GameObject yizhuangbei;
    //效果
    public List<Text> xiaoguo = new List<Text>();
    //刷新选中介绍
    public void RefDataShow() {
        DownDataShow();
        BuZhuoDaoJuData data = CUIMainManager._MainManager().GetIDBuZhuoDaoJuData(gridUIManager.GetID());
        if (data != null)
        {
            //图片
            tupian.gameObject.SetActive(true);
            tupian.sprite = gridUIManager.allGridData[gridUIManager.key].pic.sprite;
            //耐久
            naijiu.text = data.durabilityResidue + "/" + data.durabilityMax;
            //名字
            mingzi.text = data.equipName;
            //类型
            switch (data.catchType)
            {
                case 1:
                    shuoming.text = "抛接捕捉-装备";
                    break;
                case 2:
                    shuoming.text = "藏食捕捉-装备";
                    break;
                case 3:
                    shuoming.text = "啃咬捕捉-装备";
                    break;
                case 4:
                    shuoming.text = "发声捕捉-装备";
                    break;
            }
            //作用
            zuoyong.text = data.equipDesc;
            //框
            CUIMainManager._MainManager().HuanBox(data.grade, box);
            pinzhi.text = "Lv." + data.grade;
            //额外效果
            xiaoguo[0].text = data.extraOneDesc;
            xiaoguo[1].text = data.extraTwoDesc;
            //当前正在使用
            if (data.isUse == 0) { yizhuangbei.SetActive(false); }
            else { yizhuangbei.SetActive(true); }
           
        }
    }
    //关闭选中该介绍
    void DownDataShow()
    {
        //图片
        tupian.gameObject.SetActive(false);
        //名字
        mingzi.text = "";
        //说明
        shuoming.text = "无";
        //作用
        zuoyong.text = "无";
        //额外效果
        for (int i = 0; i < xiaoguo.Count; i++)
        {
            xiaoguo[i].text = "无";
        }
        yizhuangbei.SetActive(true);
        pinzhi.text = "Lv.1";
        CUIMainManager._MainManager().HuanBox(1, box);
    }
    #endregion

}

