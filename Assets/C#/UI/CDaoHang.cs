using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CDaoHang : MonoBehaviour
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
        
    }
    public void Open()
    {
        bar.SetActive(true);
    }
    public void DuHuanBtn()
    {
        CUIMainManager._MainManager().duihuan.OorDBar(true);
    }

    #region 刷新体力 金币 名字战斗力
    public Text 体力;
    public Text 金币;
    public Text 战斗力;
    public Text 名字;

    public TMP_Text 胜场;
    public TMP_Text 宠物总数量;
    public TMP_Text 珍宝总数量;
    public void RefShow()
    {
        if (CUIMainManager._MainManager().mainDataInfo.residueMuscleNum <= 0)
        { CUIMainManager._MainManager().mainDataInfo.residueMuscleNum = 0; }
        体力.text = CUIMainManager._MainManager().mainDataInfo.residueMuscleNum + "/" + CUIMainManager._MainManager().mainDataInfo.totalMuscleNum;
        CUIMainManager._MainManager().SetNum(金币, CUIMainManager._MainManager().mainDataInfo.dogCoin);
        名字.text = CUIMainManager._MainManager().mainDataInfo.userName;

        胜场.text = CUIMainManager._MainManager().mainDataInfo.winNum.ToString();
        宠物总数量.text = CUIMainManager._MainManager().mainDataInfo.dogNum.ToString();
        珍宝总数量.text = CUIMainManager._MainManager().mainDataInfo.gemNum.ToString();
    }
    #endregion

    #region 修改名字
    public GameObject setNameBar;
    public InputField nameSet;
    //免费次数 服务器是 0/1  我就当成免费剩余次数
    public Text num;
    public GameObject tips;
    public void OorDownSetNameBar(bool b)
    {
        setNameBar.SetActive(b);
        if (b)
        {
            num.text = CUIMainManager._MainManager().mainDataInfo.isFreeNameEdit.ToString();
            nameSet.text = CUIMainManager._MainManager().mainDataInfo.userName;
            if (CUIMainManager._MainManager().mainDataInfo.isFreeNameEdit == 0)
            { tips.SetActive(true);}
            else
            { tips.SetActive(false); }
        }
        else
        {
            CUIMainManager._MainManager().NET_GetMainInfo();
        }
    }
    public void Net_SetName()
    {
        if (nameSet.text == CUIMainManager._MainManager().mainDataInfo.userName)
        {
            CUIMainManager._MainManager().cUITips.Tips("修改失败\n名字相同");
            return;
        }
        if (nameSet.text == "")
        {
            CUIMainManager._MainManager().cUITips.Tips("修改失败\n请填写名字");
            return;
        }
        if (nameSet.text.Length >= 8)
        {
            CUIMainManager._MainManager().cUITips.Tips("修改失败\n最大7位数");
            return;
        }
        CUIMainManager._MainManager().NET_SetMainInfo("userName", nameSet.text);
    }
    #endregion

    #region 购买体力  让屏蔽掉了  UI按钮上的绑定方法删掉了
    public GameObject buyBar;
    //当前剩余次数
    public Text curNum;
    //获得多少体力
    public Text getTL;
    //花费多少USDT
    public TMP_Text USDT;
    public void RefTl(int _curNum,int _TL, float _USDT)
    {
        OorDownTl(true);
        curNum.text = _curNum.ToString();
        getTL.text = _TL.ToString();
        USDT.text = _USDT.ToString();
        OorDownTl(true);
    }
    //申请数据
    public void GetNetBuyTL()
    {
        CUIMainManager._MainManager().NET_brawnConfig();
        //OorDownTl(true);
    }
    public void OorDownTl(bool b)
    {
        buyBar.SetActive(b);
    }
    //购买
    public void BtnBuyTL()
    {
        CUIMainManager._MainManager().NET_PayReplenish();
    }
    #endregion
}

