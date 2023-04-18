using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class HttpManager
{
    //string httpAddress = "http://192.168.0.101:1100/api/";
    string httpAddress = "https://1100.pamperdog.club/api/";
    //string httpAddress = "https://2200.pamperdog.club/api/";
    private static HttpManager httpManager = null;
    public static HttpManager GetHttpManager()
    {
        if (httpManager == null)
        {
            httpManager = new HttpManager();
        }
        return httpManager;
    }
    #region 不带参数方法
    //Get请求
    public IEnumerator GetRequest(string url, string str = "")
    {
        if (CUIMainManager._MainManager().denglu.ip.text != "")
        {
            httpAddress = "http://" + CUIMainManager._MainManager().denglu.ip.text + ":1100/api/";
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Get(httpAddress + url + str))
        {
            if (url != "user/login")
            {
                webRequest.SetRequestHeader("authorization", token);//请求头文件内容
            }
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);

            }
            else
            {
                // if (url != "matcher/mate")
                //{
                Debug.Log(webRequest.downloadHandler.text + ":::::" + url);
                // }
                JsonData allcur_data = JsonMapper.ToObject(webRequest.downloadHandler.text);
                cur_data = webRequest.downloadHandler.text;
                CUIMainManager._MainManager().cUITips.xiaoxi.text = webRequest.downloadHandler.text;

                if (int.Parse(allcur_data["errno"] + "") == 0)
                {
                    ReturnFangFa(url, allcur_data);
                }
                else
                {
                    if (allcur_data["errno"] + "" == "401")
                    {
                        if (!CUIMainManager._MainManager().cUITips.登录提醒页.activeSelf)
                        {
                            CUIMainManager._MainManager().cUITips.OpeDLTips(true);
                            CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                        }
                    }
                    else
                    {
                        CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                    }
                }
            }
        }
    }
    //Post请求
    public IEnumerator PostRequest(string url, JsonData form)
    {
        if (CUIMainManager._MainManager().denglu.ip.text != "")
        {
            httpAddress = "http://" + CUIMainManager._MainManager().denglu.ip.text + ":1100/api/";
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Post(httpAddress + url, form.ToJson()))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(form.ToJson());
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            if (url != "user/login")
            {
                webRequest.SetRequestHeader("authorization", token);//请求头文件内容
            }
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);
                if (url == "user/login")
                {
                    CUIMainManager._MainManager().denglu.isLogin = false;
                    CUIMainManager._MainManager().cUITips.Tips("登录服务器不返回！！");
                }
                if (url == "user/auth")
                {
                    CUIMainManager._MainManager().denglu.isLogin = false;
                    CUIMainManager._MainManager().cUITips.Tips("登录服务器不返回！！");
                }
            }
            else
            {
                JsonData allcur_data = JsonMapper.ToObject(webRequest.downloadHandler.text);
                cur_data = webRequest.downloadHandler.text;
                CUIMainManager._MainManager().cUITips.xiaoxi.text = webRequest.downloadHandler.text;

                Debug.Log(webRequest.downloadHandler.text);

                if (int.Parse(allcur_data["errno"] + "") == 0)
                {

                    ReturnFangFa(url, allcur_data);
                }
                else
                {
                    CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                }
            }
        }
    }
    public IEnumerator PostRequest(string url)
    {
        if (CUIMainManager._MainManager().denglu.ip.text != "")
        {
            httpAddress = "http://" + CUIMainManager._MainManager().denglu.ip.text + ":1100/api/";
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Post(httpAddress + url, ""))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            //byte[] bodyRaw = Encoding.UTF8.GetBytes(form.ToJson());
            //webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            if (url != "user/login")
            {
                webRequest.SetRequestHeader("authorization", token);//请求头文件内容
            }
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);

            }
            else
            {
                JsonData allcur_data = JsonMapper.ToObject(webRequest.downloadHandler.text);
                cur_data = webRequest.downloadHandler.text;
                CUIMainManager._MainManager().cUITips.xiaoxi.text = webRequest.downloadHandler.text;

                Debug.Log(webRequest.downloadHandler.text);

                if (int.Parse(allcur_data["errno"] + "") == 0)
                {

                    ReturnFangFa(url, allcur_data);
                }
                else
                {
                    CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                }
            }
        }
    }
    //Post请求
    public IEnumerator PostRequest(string url, string str)
    {
        if (CUIMainManager._MainManager().denglu.ip.text != "")
        {
            httpAddress = "http://" + CUIMainManager._MainManager().denglu.ip.text + ":1100/api/";
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Post(httpAddress + url + str, ""))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            if (url != "user/login")
            {
                webRequest.SetRequestHeader("authorization", token);//请求头文件内容
            }
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);
                if (url == "catch")
                {
                    CUIMainManager._MainManager().mainBar.isbuZhuo = false;
                }
                if (url == "mate/enroll")
                {
                    CUIMainManager._MainManager().canZhan.isCanZhan = false;
                }
            }
            else
            {
                JsonData allcur_data = JsonMapper.ToObject(webRequest.downloadHandler.text);
                cur_data = webRequest.downloadHandler.text;
                CUIMainManager._MainManager().cUITips.xiaoxi.text = webRequest.downloadHandler.text;

                Debug.Log(webRequest.downloadHandler.text);

                if (int.Parse(allcur_data["errno"] + "") == 0)
                {

                    ReturnFangFa(url, allcur_data);
                }
                else
                {
                    if (allcur_data["errmsg"].ToString() == "捕捉失败！")
                    {
                        CUIMainManager._MainManager().mainBar.AniBuZhuoDown();
                        CUIMainManager._MainManager().mainBar.isbuZhuo = false;
                        if (CUIMainManager._MainManager().mainDataInfo.dogMap.id != 0)
                        {
                            CUIMainManager._MainManager().mainBar.dog.SetActive(true);
                        }
                    }
                    if (url == "catch")
                    {
                        CUIMainManager._MainManager().mainBar.isbuZhuo = false;
                    }
                    if (url == "mate/enroll")
                    {
                        CUIMainManager._MainManager().canZhan.isCanZhan = false;
                    }
                    CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                }
            }
        }
    }
    //Put请求
    public IEnumerator PutRequest(string url, string str = "")
    {
        if (CUIMainManager._MainManager().denglu.ip.text != "")
        {
            httpAddress = "http://" + CUIMainManager._MainManager().denglu.ip.text + ":1100/api/";
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Put(httpAddress + url + str, "1"))
        {
            if (url != "user/login")
            {
                webRequest.SetRequestHeader("authorization", token);//请求头文件内容
            }
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);

            }
            else
            {
                JsonData allcur_data = JsonMapper.ToObject(webRequest.downloadHandler.text);
                cur_data = webRequest.downloadHandler.text;
                Debug.Log(webRequest.downloadHandler.text);
                CUIMainManager._MainManager().cUITips.xiaoxi.text = webRequest.downloadHandler.text;
                if (int.Parse(allcur_data["errno"] + "") == 0)
                {
                    ReturnFangFa(url, allcur_data);
                }
                else
                {
                    CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                }

            }
        }
    }
    public IEnumerator PutDelete(string url, string str = "")
    {
        if (CUIMainManager._MainManager().denglu.ip.text != "")
        {
            httpAddress = "http://" + CUIMainManager._MainManager().denglu.ip.text + ":1100/api/";
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(httpAddress + url + str))
        {
            if (url != "user/login")
            {
                webRequest.SetRequestHeader("authorization", token);//请求头文件内容
            }
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            webRequest.downloadHandler = dH;
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                Debug.LogError(webRequest.error);

            }
            else
            {
                JsonData allcur_data = JsonMapper.ToObject(webRequest.downloadHandler.text);
                cur_data = webRequest.downloadHandler.text;
                Debug.Log(webRequest.downloadHandler.text);
                CUIMainManager._MainManager().cUITips.xiaoxi.text = webRequest.downloadHandler.text;
                if (int.Parse(allcur_data["errno"] + "") == 0)
                {
                    ReturnFangFa(url, allcur_data);
                }
                else
                {
                    CUIMainManager._MainManager().cUITips.Tips(allcur_data["errmsg"].ToString());
                }

            }
        }
    }
    #endregion

    string cur_data;
    string token = "";

    public bool JSONObject { get; private set; }

    void ReturnFangFa(string str, JsonData allcur_data)
    {
        switch (str)
        {
            //快速登录
            case "user/login":
                token = allcur_data["data"] + "";
                CUIMainManager._MainManager().denglu.isLogin = false;
                //储存
                登录后执行();
                CUIMainManager._MainManager().cUITips.Tips1("登录成功");
                return;
            //扫码登录
            case "user/auth":
                CUIMainManager._MainManager().denglu.isLogin = false;
                token = allcur_data["data"]["token"] + "";
                string userid = allcur_data["data"]["openId"] + "";
                //储存
                PlayerPrefs.SetString("id", userid);
                登录后执行();
                CUIMainManager._MainManager().cUITips.Tips1("登录成功");
                return;
            //个人信息
            case "user/info":
                CUIMainManager._MainManager().mainDataInfo = JsonMapper.ToObject<MainInfoData>(allcur_data["data"].ToJson());
                申请个人信息后执行();
                return;
            //修改个人信息
            case "user/update":
                CUIMainManager._MainManager().NET_GetMainInfo();
                if (CUIMainManager._MainManager().daoHang.setNameBar.activeSelf)
                {
                    CUIMainManager._MainManager().cUITips.Tips1("修改成功");
                    CUIMainManager._MainManager().daoHang.OorDownSetNameBar(false);
                }
                return;
            //所有地图
            case "map/all":
                CUIMainManager._MainManager().allMapData.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    MapData data = JsonMapper.ToObject<MapData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allMapData.Add(data);
                }
                CUIMainManager._MainManager().shijie.OorDBar(true);
                return;
            //地图详情-跳转
            case "map/detail":
                MapData mapData = JsonMapper.ToObject<MapData>(allcur_data["data"].ToJson());
                CUIMainManager._MainManager().mainBar.SetMapData(mapData);
                CUIMainManager._MainManager().shijie.OorDBar(false);
                CUIMainManager._MainManager().mainBar.OorDBar(true);
                CUIMainManager._MainManager().NET_GetMainInfo();
                return;
            case "map/deblock":
                //地图解锁
                CUIMainManager._MainManager().cUITips.Tips1("解锁成功");
                CUIMainManager._MainManager().shijie.cur_key = -1;
                CUIMainManager._MainManager().NET_GetMapData();
                CUIMainManager._MainManager().shijie.obj.SetActive(false);
                CUIMainManager._MainManager().NET_CatchEquipCur();
                return;
            case "pay/catch/equip":
                //购买道具
                CUIMainManager._MainManager().cUITips.Tips1("购买成功");
                CUIMainManager._MainManager().NET_GetMapData();
                CUIMainManager._MainManager().shijie.OorDownGouMaiBar(false);
                CUIMainManager._MainManager().shijie.OorDBar(true);
                return;
            //捕捉
            case "catch":  
                捕捉结果(allcur_data);
                return;
            //签到列表
            case "signIn/all":
                CUIMainManager._MainManager().allSignInData.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    SignInData data = JsonMapper.ToObject<SignInData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allSignInData.Add(data);
                }
                CUIMainManager._MainManager().qiandao.RefQD();
                return;
            //签到返回
            case "signIn/check":
                CUIMainManager._MainManager().cUITips.Tips1("签到成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().NET_GetSingInList();
                return;
            case "pay/sign/in":
                CUIMainManager._MainManager().cUITips.Tips1("购买成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().NET_GetSingInList();
                CUIMainManager._MainManager().qiandao.buyBtn.SetActive(false);
                CUIMainManager._MainManager().qiandao.qiandaoBtn.SetActive(true);
                return;
            //所有邮件列表
            case "mail/all":
                CUIMainManager._MainManager().allMailData.Clear();
                for (int i = 0; i < allcur_data["data"]["data"].Count; i++)
                {
                    MailData data = JsonMapper.ToObject<MailData>(allcur_data["data"]["data"][i].ToJson());
                    CUIMainManager._MainManager().allMailData.Add(data);
                }
                CUIMainManager._MainManager().youjian.RefMail();
                return;
            //所有邮件列表
            case "mail/detail":
                MailData mailData = JsonMapper.ToObject<MailData>(allcur_data["data"].ToJson());
                CUIMainManager._MainManager().youjian.RefMailXQ(mailData);
                return;
            //领取邮件
            case "mail/receive":
                CUIMainManager._MainManager().youjian.OorDBar(true);
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().cUITips.Tips1("领取成功");
                return;
            case "mail/delete":
                CUIMainManager._MainManager().youjian.OorDBar(true);
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().cUITips.Tips1("已读删除成功");
                return;
            case "mail/receive/all":
                CUIMainManager._MainManager().youjian.OorDBar(true);
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().cUITips.Tips1("全部领取成功");
                return;
            //捕捉道具列表
            case "catch/equip/all":
                //清除当前页签内容
                CUIMainManager._MainManager().buzhuodaojuList.Clear();
                for (int i = 0; i < allcur_data["data"]["data"].Count; i++)
                {
                    BuZhuoDaoJuData data = JsonMapper.ToObject<BuZhuoDaoJuData>(allcur_data["data"]["data"][i].ToJson());
                    CUIMainManager._MainManager().buzhuodaojuList.Add(data);
                }
                CUIMainManager._MainManager().buZhuoDaoJu.RefItemShow();
                return;
            //捕捉道具详情
            case "catch/equip/detail":
                BuZhuoDaoJuData buZhuoDaoJuData = JsonMapper.ToObject<BuZhuoDaoJuData>(allcur_data["data"].ToJson());
                return;
            case "catch/equip/use":
                //穿戴捕捉装备
                CUIMainManager._MainManager().cUITips.Tips1("装备成功");
                CUIMainManager._MainManager().NET_CatchEquipCur();
                CUIMainManager._MainManager().mainBar.OorDBar(true);
                CUIMainManager._MainManager().buZhuoDaoJu.OorDBar(false);
                return;
            case "catch/equip/unsnatch":
                //卸下
                CUIMainManager._MainManager().cUITips.Tips1("卸下成功");
                CUIMainManager._MainManager().NET_CatchEquipCur();
                CUIMainManager._MainManager().mainBar.OorDBar(true);
                CUIMainManager._MainManager().buZhuoDaoJu.OorDBar(false);
                return;
            case "catch/equip/current":
                //当前穿戴装备
                CUIMainManager._MainManager().curCathEquip.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    BuZhuoDaoJuData data = JsonMapper.ToObject<BuZhuoDaoJuData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().curCathEquip.Add(data);
                }
                CUIMainManager._MainManager().mainBar.RefShow();

                return;
            //获取宠物列表
            case "user/dog/all":
                //跟新宠物列表
                CUIMainManager._MainManager().AllChongWuData.Clear();
                for (int i = 0; i < allcur_data["data"]["data"].Count; i++)
                {
                    ChongWuData data = JsonMapper.ToObject<ChongWuData>(allcur_data["data"]["data"][i].ToJson());
                    CUIMainManager._MainManager().AllChongWuData.Add(data);
                }
                CUIMainManager._MainManager().siYang.RefMCW();
                return;
            //更新狗狗出战
            case "user/dog/use":
                CUIMainManager._MainManager().siYang.SetTagSelect(0);
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().cUITips.Tips1("出战成功");
                return;
            //获取物品列表
            case "store/all":
                申请列表后执行(allcur_data);
                return;
            //饲养
            case "raise":
                CUIMainManager._MainManager().cUITips.Tips1("使用道具成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().siYang.SetTagSelect(CUIMainManager._MainManager().siYang.tagManager.Cur_SelectedTab);
                CUIMainManager._MainManager().NET_GetChongWuDataList();
                return;
            case "nft/cast":
                //nft
                CUIMainManager._MainManager().cUITips.Tips1("铸造NFT成功");
                CUIMainManager._MainManager().cangKu.SetTagSelect(CUIMainManager._MainManager().cangKu.tagManager.Cur_SelectedTab);
                return;
            case "nft/out":
                //nft提取
                CUIMainManager._MainManager().cUITips.Tips1("提取成功");
                CUIMainManager._MainManager().cangKu.SetTagSelect(CUIMainManager._MainManager().cangKu.tagManager.Cur_SelectedTab);
                return;                
            case "gobble":
                CUIMainManager._MainManager().cUITips.Tips1("吞噬成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().siYang.SetTagSelect(CUIMainManager._MainManager().siYang.tagManager.Cur_SelectedTab);
                CUIMainManager._MainManager().NET_GetChongWuDataList();
                return;
            case "discard":
                //丢弃
                CUIMainManager._MainManager().cUITips.Tips1("丢弃成功\n返还奖励请去邮箱领取");
                CUIMainManager._MainManager().NET_GetMainInfo();
                if (CUIMainManager._MainManager().cangKu.bar.activeSelf)
                {
                    CUIMainManager._MainManager().cangKu.SetTagSelect(CUIMainManager._MainManager().cangKu.tagManager.Cur_SelectedTab);
                }
                return;
            case "store/use/prop":
                //道具使用
                CUIMainManager._MainManager().siYang.SetTagSelect(CUIMainManager._MainManager().siYang.tagManager.Cur_SelectedTab);
                CUIMainManager._MainManager().NET_GetChongWuDataList();
                CUIMainManager._MainManager().cUITips.Tips1("使用成功");
                return;
            //对战装备
            case "fight/equip/all":
                CUIMainManager._MainManager().allZhuangDZBeiData.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    ZhuangBeiData data = JsonMapper.ToObject<ZhuangBeiData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allZhuangDZBeiData.Add(data);
                }
                //参战
                CUIMainManager._MainManager().canZhan.RefEquipPage();
                return;
            //参赛宠物列表
            case "user/dog/fight":
                CUIMainManager._MainManager().allZhanDouChongWuData.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    ZhanDouChongWuData data = JsonMapper.ToObject<ZhanDouChongWuData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allZhanDouChongWuData.Add(data);
                }
                CUIMainManager._MainManager().canZhan.RefList();
                CUIMainManager._MainManager().canZhan.OorDBar(true);
                CUIMainManager._MainManager().biSai.OorDBar(false);
                return;
            case "store/all/Prop":
                CUIMainManager._MainManager().allFightItem.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    FightData data = JsonMapper.ToObject<FightData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allFightItem.Add(data);
                }
                CUIMainManager._MainManager().canZhan.RefDatJu();
                return;
            //报名比赛
            case "mate/enroll":
               
                CUIMainManager._MainManager().canZhan.OorDBar(false);
                CUIMainManager._MainManager().biSai.OorDBar(true);
                CUIMainManager._MainManager().biSai.pipei.SetActive(true);
                CUIMainManager._MainManager().cur_time = 0;
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().canZhan.isCanZhan = false;
                CUIMainManager._MainManager().cUITips.Tips1("报名成功");
                return;
            //取消报名
            case "mate/cancel":
                CUIMainManager._MainManager().biSai.pipei.SetActive(false);
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().cUITips.Tips1("取消报名成功");
                return;
            case "matcher/mate":
                比赛状态刷新(allcur_data);
                return;
            case "game/all/dog":
                CUIMainManager._MainManager().allUseDog.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    ZhanDouChongWuData data = JsonMapper.ToObject<ZhanDouChongWuData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allUseDog.Add(data);
                }
                if (CUIMainManager._MainManager().worlBiSaiData.type == 2)
                {
                    CUIMainManager._MainManager().yaZhu.RefDogData();
                }
                if (CUIMainManager._MainManager().worlBiSaiData.type == 4)
                {
                    CUIMainManager._MainManager().biSaiManager.AddDog5();
                }
                return;
            case "game/stake/one":
                CUIMainManager._MainManager().cUITips.Tips1("独赢-赞助成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().yaZhu.RefDY();
                CUIMainManager._MainManager().yaZhu.ChongZhiXiaZhu(false);
                CUIMainManager._MainManager().yaZhu.chongZhiXiaZhu();
                return;
            case "game/stake/more":
                CUIMainManager._MainManager().cUITips.Tips1("名次-赞助成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().yaZhu.RefYZ();
                CUIMainManager._MainManager().yaZhu.ChongZhiXiaZhu(false);
                CUIMainManager._MainManager().yaZhu.chongZhiXiaZhu();
                return;
            case "game/stake/surround":
                CUIMainManager._MainManager().cUITips.Tips1("包围-赞助成功");
                CUIMainManager._MainManager().NET_GetMainInfo();
                CUIMainManager._MainManager().yaZhu.RefBY();
                CUIMainManager._MainManager().yaZhu.ChongZhiXiaZhu(false);
                CUIMainManager._MainManager().yaZhu.chongZhiXiaZhu();
                return;
            case "game/jackpot":
                //奖池
                CUIMainManager._MainManager().yaZhu.IncreaseAnim(CUIMainManager._MainManager().yaZhu.cur_JiangChi, int.Parse(allcur_data["data"]["jackpot"] + ""));
                return;
            case "game/status":
                游戏状态(allcur_data);
                return;
            case "game/record":
                CUIMainManager._MainManager().allBet.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    BetData data = JsonMapper.ToObject<BetData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allBet.Add(data);
                }
                CUIMainManager._MainManager().yaZhu.RefZhanZhu();
                return;
            case "game/result":
                CUIMainManager._MainManager().gameOverData.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    GameOverData data = JsonMapper.ToObject<GameOverData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().gameOverData.Add(data);
                }
                //Debug.LogError("结算申请秒");
                CUIMainManager._MainManager().NET_GameType();//结算申请秒
                CUIMainManager._MainManager().biSaiKaiShi.GameOverBar();
                return;
            case "game/win/ratio":
                //押注倍率
                if (allcur_data["data"]["type"] + "" == "1")
                {
                    for (int i = 0; i < 6; i++)
                    {
                        CUIMainManager._MainManager().yaZhu.selected_DY.SetPeiL(i, allcur_data["data"]["result"][i].ToString());
                    }
                }
                if (allcur_data["data"]["type"] + "" == "2")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        CUIMainManager._MainManager().yaZhu.listBetManager_MC[i].SetPeiL(allcur_data["data"]["result"][i].ToString());
                    }
                }
                if (allcur_data["data"]["type"] + "" == "3")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        CUIMainManager._MainManager().yaZhu.listBetManager_BW[i].SetPeiL(allcur_data["data"]["result"][i].ToString());
                    }
                }
                return;
            case "ad/all":
                //广告列表
                CUIMainManager._MainManager().allAD.Clear();
                for (int i = 0; i < allcur_data["data"].Count; i++)
                {
                    ADData data = JsonMapper.ToObject<ADData>(allcur_data["data"][i].ToJson());
                    CUIMainManager._MainManager().allAD.Add(data);
                }
                CUIMainManager._MainManager().duihuan.RefAdBar();
                return;
            case "ad/detail":
                CUIMainManager._MainManager().duihuan.StartGuangGao();
                return;
            case "ad/look":
                CUIMainManager._MainManager().duihuan.NetPlayOver("观看成功\n请去邮件领取奖励");
                return;
            case "user/recharge":
                //充值
                CUIMainManager._MainManager().duihuan.RefTopUp();
                return;
            case "user/asg/total":
                //总金额
                CUIMainManager._MainManager().duihuan.SetUSDT(float.Parse(allcur_data["data"]["asg"].ToString()));
                return;
            case "user/withdraw":
                //提现
                CUIMainManager._MainManager().duihuan.RefTiXian();
                return;
            case "user/brawn/config":
                //购买体力列表
                CUIMainManager._MainManager().daoHang.RefTl(int.Parse(allcur_data["data"]["buyBrawnNum"].ToString()),
                                                            int.Parse(allcur_data["data"]["brawn"].ToString()),
                                                            float.Parse(allcur_data["data"]["usdt"].ToString()));
                CUIMainManager._MainManager().daoHang.OorDownTl(true);
                return;
            case "pay/replenish":
                //购买体力
                CUIMainManager._MainManager().cUITips.Tips1("购买体力成功");
                CUIMainManager._MainManager().daoHang.OorDownTl(false);
                CUIMainManager._MainManager().NET_GetMainInfo();
                return;
        }
    }

    #region 申请登录后执行

    void 登录后执行()
    {
        CUIMainManager._MainManager().NET_GetMainInfo();

    }

    #endregion

    #region 申请个人信息后执行

    void 申请个人信息后执行()
    {
        CUIMainManager._MainManager().mainBar.SetDog();
        if (CUIMainManager._MainManager().mainDataInfo.isSignIn == 0)
        {
            //得购买
            CUIMainManager._MainManager().qiandao.buyBtn.SetActive(true);
            CUIMainManager._MainManager().qiandao.qiandaoBtn.SetActive(false);
        }
        else
        {
            //可以签到
            CUIMainManager._MainManager().qiandao.buyBtn.SetActive(false);
            CUIMainManager._MainManager().qiandao.qiandaoBtn.SetActive(true);
        }
        签到提醒();
        导航栏刷新();
        设置刷新();
        地图详情刷新();
    }
    void 设置刷新()
    {
        //音乐
        if (CUIMainManager._MainManager().mainDataInfo.isMusic == 1)
        {
            CUIMainManager._MainManager().shezhi.NetOorDYY(true);
        }
        else
        {
            CUIMainManager._MainManager().shezhi.NetOorDYY(false);
        }
        //音效
        if (CUIMainManager._MainManager().mainDataInfo.isEffect == 1)
        {
            CUIMainManager._MainManager().shezhi.NetOorDYX(true);
        }
        else
        {
            CUIMainManager._MainManager().shezhi.NetOorDYX(false);
        }
        //中英切换
        if (CUIMainManager._MainManager().shezhi.zhEn != CUIMainManager._MainManager().mainDataInfo.language)
        {
            CUIMainManager._MainManager().shezhi.NetEnCh(CUIMainManager._MainManager().mainDataInfo.language);
        }
    }
    void 地图详情刷新()
    {
        if (!CUIMainManager._MainManager().isNetMapDate)
        {
            CUIMainManager._MainManager().NET_CatchEquipCur();
            CUIMainManager._MainManager().NET_GetMapDate(CUIMainManager._MainManager().mainDataInfo.mapId);
            CUIMainManager._MainManager().isNetMapDate = true;
            CUIMainManager._MainManager().cur_time = 0;
            CUIMainManager._MainManager().HuanBGMusic("mainbj");
            CUIMainManager._MainManager().denglu.OorDBar(false);
            CUIMainManager._MainManager().mainBar.OorDBar(true);
            CUIMainManager._MainManager().daoHang.OorDBar(true);
        }
    }
    void 导航栏刷新()
    {
        CUIMainManager._MainManager().daoHang.RefShow();

    }
    void 签到提醒()
    {
        if (CUIMainManager._MainManager().mainDataInfo.isTodayCheck == 0)
        {
            CUIMainManager._MainManager().mainBar.OOrDTips(TipsType.E_qiandao, true);
        }
        if (CUIMainManager._MainManager().mainDataInfo.isHaveUnread == 1)
        {
            CUIMainManager._MainManager().mainBar.OOrDTips(TipsType.E_youjian, true);
        }

    }

    #endregion

    #region 申请捕捉后执行

    void 捕捉结果(JsonData allcur_data)
    {
        //狗关掉
        CUIMainManager._MainManager().mainBar.dog.SetActive(false);
        //捕捉动画打开
        CUIMainManager._MainManager().mainBar.buzhuo.SetActive(true);
        //开启动画
        CUIMainManager._MainManager().mainBar.buzhuo.transform.GetChild(0).GetComponent<Animator>().SetBool("isPlay", true);
        CUIMainManager._MainManager().mainBar.isbuZhuo = false;
        switch (int.Parse(allcur_data["data"]["type"].ToString()))
        {
            //宠物
            case 1:
                CUIMainManager._MainManager().zhuaBu.ChengGong(allcur_data["data"]["dogName"].ToString(),
                                                               allcur_data["data"]["imgName"].ToString(),
                                                               allcur_data["data"]["inbornNum"].ToString(),
                                                               allcur_data["data"]["fightingNum"].ToString(),
                                                               allcur_data["data"]["maxGrowUpNum"].ToString(),
                                                               allcur_data["data"]["maxGrowUpNum"].ToString(),
                                                               allcur_data["data"]["dogBreed"].ToString());
                break;
            //珍宝
            case 2:
                CUIMainManager._MainManager().zhuaBu.ChengGong(allcur_data["data"]["name"].ToString(),
                                                               allcur_data["data"]["image"].ToString(),
                                                               allcur_data["data"]["desc"].ToString(),
                                                               allcur_data["data"]["number"].ToString());
                break;
            case 3:
                //饲料
                CUIMainManager._MainManager().zhuaBu.ChengGong(allcur_data["data"]["forageName"].ToString(),
                                                               allcur_data["data"]["imgName"].ToString(),
                                                               allcur_data["data"]["forageDesc"].ToString(),
                                                               allcur_data["data"]["forageNum"].ToString());
                break;
            case 4:
                //野生
                CUIMainManager._MainManager().zhuaBu.ChengGong(allcur_data["data"]["wildName"].ToString(),
                                                               allcur_data["data"]["imgName"].ToString(),
                                                               allcur_data["data"]["wildDesc"].ToString(),
                                                               allcur_data["data"]["wildNum"].ToString());
                break;
            case 5:
                //捕捉装备
                CUIMainManager._MainManager().zhuaBu.ChengGong(allcur_data["data"]["equipName"].ToString(),
                                                               allcur_data["data"]["imgName"].ToString(),
                                                               allcur_data["data"]["equipDesc"].ToString(),
                                                               allcur_data["data"]["equipNum"].ToString());
                break;
            //失败
            case 6:
                //对战装备
                CUIMainManager._MainManager().zhuaBu.ChengGong(allcur_data["data"]["fightName"].ToString(),
                                                               allcur_data["data"]["imgName"].ToString(),
                                                               allcur_data["data"]["fightDesc"].ToString(),
                                                               allcur_data["data"]["fightNum"].ToString());
                break;
            case 7:
                CUIMainManager._MainManager().zhuaBu.ShiBai();
                break;
        }
        if (int.Parse(allcur_data["data"]["type"].ToString()) != 7)
        {
            CUIMainManager._MainManager().buzhuoID = int.Parse(allcur_data["data"]["id"].ToJson());
            CUIMainManager._MainManager().discardType = int.Parse(allcur_data["data"]["discardType"].ToJson());
        }
    }

    #endregion

    #region 申请列表后执行

    void 申请列表后执行(JsonData allcur_data)
    {
        switch (CUIMainManager._MainManager().tagType)
        {
            case 1:
                宠物列表刷新(allcur_data);
                return;
            case 2:
            case 3:
            case 4:
                物品列表刷新(allcur_data);
                return;
            case 5:
                装备列表刷新(allcur_data);
                return;
        }
    }
    void 宠物列表刷新(JsonData allcur_data)
    {
        CUIMainManager._MainManager().AllChongWuData.Clear();
        for (int i = 0; i < allcur_data["data"].Count; i++)
        {
            ChongWuData data = JsonMapper.ToObject<ChongWuData>(allcur_data["data"][i].ToJson());
            CUIMainManager._MainManager().AllChongWuData.Add(data);
        }
        if (CUIMainManager._MainManager().siYang.bar.activeSelf)
        {
            //饲养
            CUIMainManager._MainManager().siYang.RefItemShow();
        }
        else
        {
            //仓库
            CUIMainManager._MainManager().cangKu.RefItemShow();
        }
    }
    void 物品列表刷新(JsonData allcur_data)
    {
        CUIMainManager._MainManager().allItemData.Clear();
        for (int i = 0; i < allcur_data["data"].Count; i++)
        {
            ItemData data = JsonMapper.ToObject<ItemData>(allcur_data["data"][i].ToJson());
            CUIMainManager._MainManager().allItemData.Add(data);
        }
        if (CUIMainManager._MainManager().siYang.bar.activeSelf)
        {
            //饲养
            CUIMainManager._MainManager().siYang.RefItemShow();
        }
        if (CUIMainManager._MainManager().cangKu.bar.activeSelf)
        {
            //仓库
            CUIMainManager._MainManager().cangKu.RefItemShow();
        }

    }
    void 装备列表刷新(JsonData allcur_data)
    {
        CUIMainManager._MainManager().allZhuangBeiData.Clear();
        for (int i = 0; i < allcur_data["data"].Count; i++)
        {
            ZhuangBeiData data = JsonMapper.ToObject<ZhuangBeiData>(allcur_data["data"][i].ToJson());
            CUIMainManager._MainManager().allZhuangBeiData.Add(data);
        }
        if (CUIMainManager._MainManager().siYang.bar.activeSelf)
        {
            //饲养
            CUIMainManager._MainManager().siYang.RefItemShow();
        }
        if (CUIMainManager._MainManager().cangKu.bar.activeSelf)
        {
            //仓库
            CUIMainManager._MainManager().cangKu.RefItemShow();
        }
        //if (CUIMainManager._MainManager().canZhan.bar.activeSelf)
        //{
        //    //参战
        //    CUIMainManager._MainManager().canZhan.RefEquipPage();
        //}
    }

    #endregion

    //比赛状态刷新1/秒
    public void 比赛状态刷新(JsonData allcur_data)
    {
        世界比赛状态变化刷新(allcur_data);
        我的比赛状态变化刷新(allcur_data);
        // Debug.LogError("我的匹配状态" + allcur_data["data"]["myMap"]["type"] + "----------" +
        //"世界比赛状态" + allcur_data["data"]["worldMap"]["type"]);
    }

    void 我的比赛状态变化刷新(JsonData allcur_data)
    {
        BiSaiData data = JsonMapper.ToObject<BiSaiData>(allcur_data["data"]["myMap"].ToJson());
        if (CUIMainManager._MainManager().myBiSaiData.type != data.type)
        {
            switch (data.type)
            {
                case 0:
                    //未匹配
                    //刷新邮箱
                    CUIMainManager._MainManager().NET_GetMainInfo();
                    break;
                case 1:
                    //匹配中
                    //匹配倒计时代开
                    CUIMainManager._MainManager().cur_time = 0;
                    CUIMainManager._MainManager().biSai.pipei.SetActive(true);
                    break;
                case 2:
                    //匹配成功
                    break;
                case 3:
                    //匹配失败
                    CUIMainManager._MainManager().cUITips.OorDTispPiPei(0);
                    CUIMainManager._MainManager().biSai.pipei.SetActive(false);
                    break;
                case 4:
                    //等待中
                    //匹配成功提示
                    CUIMainManager._MainManager().cUITips.Tips1("匹配成功,正在排队请等待");
                    CUIMainManager._MainManager().biSai.pipei.SetActive(false);
                    CUIMainManager._MainManager().biSai.paidui.SetActive(true);
                    CUIMainManager._MainManager().biSai.SetChang(data.etcNum);
                    break;
                case 5:
                    //进场             
                    break;
                case 6:
                    //押注中
                    if (CUIMainManager._MainManager().myBiSaiData.type != 4)
                    {
                        CUIMainManager._MainManager().cUITips.Tips1("匹配成功,请进行赞助");
                    }
                    CUIMainManager._MainManager().biSai.paidui.SetActive(false);
                    CUIMainManager._MainManager().biSai.pipei.SetActive(false);
                    CUIMainManager._MainManager().cUITips.OpeYaZhuTips();
                    break;
            }
        }
        //排队刷新
        if (data.type == 4 && CUIMainManager._MainManager().myBiSaiData.etcNum != data.etcNum)
        {
            //刷新显示
            if (CUIMainManager._MainManager().biSai.pipei.activeSelf) { CUIMainManager._MainManager().biSai.pipei.SetActive(false); }
            if (!CUIMainManager._MainManager().biSai.paidui.activeSelf) { CUIMainManager._MainManager().biSai.paidui.SetActive(true); }
            CUIMainManager._MainManager().biSai.SetChang(data.etcNum);
        }
        CUIMainManager._MainManager().myBiSaiData = data;
    }

    void 世界比赛状态变化刷新(JsonData allcur_data)
    {
        BiSaiData data = JsonMapper.ToObject<BiSaiData>(allcur_data["data"]["worldMap"].ToJson());
        if (CUIMainManager._MainManager().worlBiSaiData.type != data.type)
        {
            switch (data.type)
            {
                case 0:
                    CUIMainManager._MainManager().biSaiKaiShi.isJieSuan = false;
                    if (CUIMainManager._MainManager().biSaiKaiShi.bar.activeSelf)
                    {
                        CUIMainManager._MainManager().cUITips.Tips1("下组狗狗正在入场...");
                        CUIMainManager._MainManager().worlBiSaiData.type = 0;
                    }
                    return;
                case 1:
                    CUIMainManager._MainManager().biSaiKaiShi.isJieSuan = false;
                    //没有比赛
                    CUIMainManager._MainManager().NET_GetMainInfo();
                    CUIMainManager._MainManager().biSai.meiyoubisai.SetActive(true);
                    CUIMainManager._MainManager().biSai.youbisai.SetActive(false);
                    if (!CUIMainManager._MainManager().biSaiKaiShi.bar.activeSelf) break;
                    //关闭比赛开始界面
                    CUIMainManager._MainManager().biSaiKaiShi.OorDBar(false);
                    CUIMainManager._MainManager().HuanBGMusic("mainbj");
                    break;
                case 2:
                    CUIMainManager._MainManager().yaZhu.ChongZhiXiaZhu();
                    CUIMainManager._MainManager().biSaiKaiShi.isJieSuan = false;
                    CUIMainManager._MainManager().yaZhu.chongZhiXiaZhu();
                    CUIMainManager._MainManager().biSai.SetBiSai(data.orderNum, "赞助中", true);
                    CUIMainManager._MainManager().yaZhu.xuhao.text = "NO." + data.orderNum.Substring(data.orderNum.Length - 7);
                    if (CUIMainManager._MainManager().worlBiSaiData.type == 0)
                    {
                        CUIMainManager._MainManager().关闭所有界面();
                        CUIMainManager._MainManager().biSaiKaiShi.OorDBar(false);  //关闭 比赛开始界面
                        CUIMainManager._MainManager().yaZhu.OorDBar(true);         //打开 押注界面     
                        CUIMainManager._MainManager().HuanBGMusic("zbbj");
                    }
                    break;
                case 3:
                    CUIMainManager._MainManager().biSai.SetBiSai(data.orderNum, "士气调整", false);
                    if (CUIMainManager._MainManager().yaZhu.bar.activeSelf)
                    {
                        //士气
                        CUIMainManager._MainManager().cUITips.Tips1("士气调整开始");
                        //Debug.LogError("申请士气");
                        CUIMainManager._MainManager().NET_GameType();//申请士气
                    }
                    break;
                case 4:
                    CUIMainManager._MainManager().biSai.SetBiSai(data.orderNum, "比赛中", false);
                    if (!CUIMainManager._MainManager().yaZhu.bar.activeSelf) break;
                    //进入战斗页面
                    CUIMainManager._MainManager().关闭所有界面();
                    CUIMainManager._MainManager().worlBiSaiData = data;
                    CUIMainManager._MainManager().biSaiKaiShi.OorDBar(true);
                    break;
                case 5:
                    //结算中
                    CUIMainManager._MainManager().biSai.SetBiSai(data.orderNum, "结算中", false);
                    if (!CUIMainManager._MainManager().biSaiKaiShi.bar.activeSelf) break;
                    //申请结算
                    CUIMainManager._MainManager().NET_GameResult();
                    break;
            }
        }
        CUIMainManager._MainManager().worlBiSaiData = data;
    }
    void 游戏状态(JsonData allcur_data)
    {
        switch (allcur_data["data"]["type"] + "")
        {
            case "1":
                //没有比赛
                break;
            case "2":
                //押注中
                int miao = int.Parse(allcur_data["data"]["second"] + "");
                CUIMainManager._MainManager().cur_time = 0;
                CUIMainManager._MainManager().yaZhu.SetYaZhuTime(miao);
                Debug.LogError(miao);
                CUIMainManager._MainManager().yaZhu.yazhuTime.gameObject.SetActive(true);
                CUIMainManager._MainManager().yaZhu.uiShiqi.gameObject.SetActive(false);
                break;
            case "3":
                //士气
                int shiqi = int.Parse(allcur_data["data"]["second"] + "");
                CUIMainManager._MainManager().cur_time = 0;
                CUIMainManager._MainManager().yaZhu.SetShiQiM(shiqi);
                CUIMainManager._MainManager().yaZhu.yazhuTime.gameObject.SetActive(false);
                CUIMainManager._MainManager().yaZhu.uiShiqi.gameObject.SetActive(true);
                break;
            case "4":
                //比赛
                int bisai = int.Parse(allcur_data["data"]["second"] + "");
                CUIMainManager._MainManager().cur_time = 0;
                CUIMainManager._MainManager().biSaiKaiShi.SetBiSaiM(bisai);
                break;
            case "5":
                //结算
                int jiesuan = int.Parse(allcur_data["data"]["second"] + "");
                CUIMainManager._MainManager().cur_time = 0;
                CUIMainManager._MainManager().biSaiKaiShi.SetJieSuanM(jiesuan);
                break;
        }
    }
}
