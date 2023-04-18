using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CBiSai : MonoBehaviour
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
            if (!CUIMainManager._MainManager().canZhan.bar.activeSelf)
            {
                CUIMainManager._MainManager().HuanBGMusic("mainbj");
            }
            
        }
    }

    public void Btn_ChuZhan()
    {

        switch (CUIMainManager._MainManager().myBiSaiData.type)
        {
          
            case 1:
                //匹配中
                CUIMainManager._MainManager().cUITips.Tips("正在匹配对手请稍后再试");
                return;
            case 2:
                //匹配成功
                return;
            case 3:
                //匹配失败
                return;
            case 4:
            //等待中
            case 5:
                CUIMainManager._MainManager().cUITips.Tips("距离比赛次还有" + CUIMainManager._MainManager().myBiSaiData.etcNum + "场");
                return;
        }
        //未匹配
        CUIMainManager._MainManager().NET_FightlDog();
        CUIMainManager._MainManager().NET_Fight_DaoJu();
    }
    public void Btn_GuanZhan()
    {

        switch (CUIMainManager._MainManager().worlBiSaiData.type)
        {
            case 1:
                //没有比赛
                CUIMainManager._MainManager().cUITips.Tips("当前没有可观看的比赛");
                return;
            case 2:
                //押注中
                OorDBar(false);
                CUIMainManager._MainManager().yaZhu.OorDBar(true);
                return;
            case 3:
                //比赛中
                CUIMainManager._MainManager().cUITips.Tips("比赛中-不可中途观看");
                return;
            case 4:
                //结算中
                CUIMainManager._MainManager().cUITips.Tips("结算中");
                return;
        }

    }
    #region 匹配中
    public GameObject pipei;
    public Text time;
    public void SetTime(int _time)
    {
        time.text = _time + "秒";
    }
    public void CancelBtn()
    {
        CUIMainManager._MainManager().NET_CancelBaoMing(CUIMainManager._MainManager().canZhan.curID);
    }
    #endregion

    #region 排队中
    public GameObject paidui;
    public Text chang;
    public void SetChang(int _chang)
    {
        chang.text = _chang.ToString();
    }
    #endregion

    #region 世界比赛状态
    public GameObject youbisai;
    public GameObject meiyoubisai;

    public Text xuhao;
    public Text zhuangtai;
    public Text canyu;
    public void SetBiSai(string _xuhao, string _zhuangtai, bool b)
    {
        if (!youbisai.activeSelf) { youbisai.SetActive(true); }
        if (meiyoubisai.activeSelf) { meiyoubisai.SetActive(false); }
        //xuhao.text = _xuhao;
        zhuangtai.text = _zhuangtai;
        if (b) { canyu.text = "可以参与"; }
        else { canyu.text = "不可参与"; }
    }
    #endregion

    public GameObject shuoming;
    public void OorDShuoMing(bool b)
    {
        shuoming.SetActive(b);
    }
}
