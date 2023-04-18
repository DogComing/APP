using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CDuiHuan : MonoBehaviour
{
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpDateVideo();
    }
    public void OorDBar(bool b)
    {
        bar.SetActive(b);
        if (b)
        {
            CUIMainManager._MainManager().NET_GetAD();
            CUIMainManager._MainManager().NET_AsgTotal();
        }
    }

    #region 充值
    public Text usdt;
    public void SetUSDT(float _usdt)
    {
        usdt.text = _usdt.ToString();
    }
    /// <summary>
    /// 充值数量
    /// </summary>
    public InputField topUpValue;
    /// <summary>
    /// 充值按钮
    /// </summary>
    public void Btn_TopUp()
    {
        if (topUpValue.text == "")
        {
            CUIMainManager._MainManager().cUITips.Tips("请填写充值数量");
        }
        else
        {
            int _topUpValue = int.Parse(topUpValue.text);
            CUIMainManager._MainManager().NET_Recharge(_topUpValue);
        }
    }
    public void RefTopUp()
    {
        CUIMainManager._MainManager().NET_AsgTotal();
        topUpValue.text = "";
        CUIMainManager._MainManager().cUITips.Tips1("充值成功");
        CUIMainManager._MainManager().NET_GetMainInfo();
    }
    #endregion

    #region 提现
    /// <summary>
    /// 提现数量
    /// </summary>
    public InputField tiXianValue;
    /// <summary>
    /// 提现按钮
    /// </summary>
    public void Btn_TiXian()
    {
        if (tiXianValue.text == "")
        {
            CUIMainManager._MainManager().cUITips.Tips("请填写提现数量");
            return;
        }
        int _topUpValue = int.Parse(tiXianValue.text);
        if (CUIMainManager._MainManager().mainDataInfo.dogCoin >= _topUpValue)
        {
            //发送提现
            CUIMainManager._MainManager().NET_withdraw(_topUpValue);
        }
        else
        {
            CUIMainManager._MainManager().cUITips.Tips("提现失败\nASG不足");
        }
    }
    public void RefTiXian()
    {
        CUIMainManager._MainManager().NET_AsgTotal();
        tiXianValue.text = "";
        CUIMainManager._MainManager().cUITips.Tips1("提现成功");
        CUIMainManager._MainManager().NET_GetMainInfo();
    }
    #endregion

    #region 广告

    public List<TMP_Text> allValue = new List<TMP_Text>();
    public List<Image> allImage = new List<Image>();
    public List<GameObject> allLook = new List<GameObject>();
    public void RefAdBar()
    {
        for (int i = 0; i < CUIMainManager._MainManager().allAD.Count; i++)
        {
            allLook[i].SetActive(CUIMainManager._MainManager().allAD[i].isLook);
            //文字
            allValue[i].text = CUIMainManager._MainManager().allAD[i].awardNum.ToString();
            //换图
            CUIMainManager._MainManager().HuanTu(allImage[i], CUIMainManager._MainManager().allAD[i].imgName);
        }
    }
    public GameObject advertisingBar;
    public MediaPlayer videoPlayer;
    bool isPlayer = false;
    ADData cur_data;
    //观看广告
    public void BtnOpenGuangGao(int key)
    {
        cur_data = CUIMainManager._MainManager().allAD[key - 1];
        videoPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, cur_data.adUrl, false);
        if (cur_data.isLook) { CUIMainManager._MainManager().cUITips.Tips("今日没有播放次数"); return; }
        CUIMainManager._MainManager().NET_GetADData(cur_data.id);
    }
    public void StartGuangGao()
    {
        CUIMainManager._MainManager().bj_music.volume = 0;
        //发送观看广告
        advertisingBar.SetActive(true);
        videoPlayer.m_VideoPath = cur_data.adUrl;
        jindutiao.fillAmount = 0;
        videoPlayer.Control.Play();
        Invoke("YCIsPlayer",0.5f);
        CUIMainManager._MainManager().daoHang.OorDBar(false);
    }
    //临时解决第二次观看广告有几率出现直接播放完毕的情况 延迟执行
    void YCIsPlayer()
    {
        isPlayer = true;
    }
    public Image jindutiao;
    //检测广告
    void UpDateVideo()
    {
        if (isPlayer)
        {
            if (videoPlayer.Control.GetCurrentTimeMs() != 0)
            {
                jindutiao.fillAmount = videoPlayer.Control.GetCurrentTimeMs() / videoPlayer.Info.GetDurationMs();
                if (videoPlayer.Control.GetCurrentTimeMs() >= videoPlayer.Info.GetDurationMs())
                {
                    isPlayer = false;
                    //发送播放完毕消息
                    CUIMainManager._MainManager().NET_ADOver(cur_data.id);
                
                }
            }
        }
    }
    //播放完毕
    public void NetPlayOver(string tipsStr)
    {
        OorDBar(true);
        if (CUIMainManager._MainManager().mainDataInfo.isMusic == 1)
            CUIMainManager._MainManager().bj_music.volume = 1;
        videoPlayer.Stop();
        advertisingBar.SetActive(false);
        CUIMainManager._MainManager().daoHang.OorDBar(true);
        CUIMainManager._MainManager().cUITips.Tips1(tipsStr);
        CUIMainManager._MainManager().NET_GetMainInfo();
        
    }
    //取消播放
    public void BtnCancelPlay()
    {
        if (CUIMainManager._MainManager().mainDataInfo.isMusic == 1)
            CUIMainManager._MainManager().bj_music.volume = 1;
        
        videoPlayer.Stop();
        
        isPlayer = false;
        advertisingBar.SetActive(false);
        CUIMainManager._MainManager().daoHang.OorDBar(true);
        CUIMainManager._MainManager().cUITips.Tips("提前关闭视频\n本次不获得奖励");
        
    }
    //跳转
    public void BtnTiaoZhuan()
    {
        Application.OpenURL(cur_data.jumpUrl);
    }
    #endregion
}
