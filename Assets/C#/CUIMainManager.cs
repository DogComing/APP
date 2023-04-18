using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using Spine.Unity;
using UnityEditor;
using System;

public class CUIMainManager : MonoBehaviour
{
    private static CUIMainManager _main;
    public static CUIMainManager _MainManager()
    {
        return _main;
    }

    void Start()
    {
        _main = this;
    }

    void Update()
    {
        计时器();
    }

    #region API

    //避免重复申请地图详情
    public bool isNetMapDate;
    public float cur_time = 0;
    public int cur_timeInt = 0;

    //总计时
    void 计时器()
    {
        if (!isNetMapDate) return;
        cur_time += Time.deltaTime;
        if (cur_timeInt != (int)cur_time)
        {
            比赛状态计时器();
            匹配计时器();
            奖金计时器();
            押注计时器();
            士气计时器();
            比赛计时器();
            结算计时器();
        }
    }
    void 奖金计时器()
    {
        if (!yaZhu.bar.activeSelf) return;
        //申请奖池
        //NET_JiangChi();
    }
    void 匹配计时器()
    {
        //匹配计时器
        cur_timeInt = (int)cur_time;
        biSai.SetTime(cur_timeInt);
    }
    void 比赛状态计时器()
    {
        //每秒申请比赛状态
        NET_FinBiSai();
    }
    void 押注计时器()
    {
        if (worlBiSaiData.type == 2)
        {
            if (yaZhu.cur_miao > 0)
            {
                //没有到最后3秒前端自己算
                yaZhu.cur_miao--;
                yaZhu.SetYaZhuTime(yaZhu.cur_miao);
            }
            else
            {
                yaZhu.SetYaZhuTime(0);
            }
        }
    }
    void 士气计时器()
    {
        if (worlBiSaiData.type == 3)
        {
            if (yaZhu.cur_ShiqiM > 0)
            {
                yaZhu.cur_ShiqiM--;
                yaZhu.SetShiQiM(yaZhu.cur_ShiqiM);
            }
            else
            {
                yaZhu.SetShiQiM(0);
            }
        }

    }
    void 比赛计时器()
    {
        if (!biSaiKaiShi.isBiSai) return;
        if (biSaiKaiShi.cur_BiSaiM > 6)
        {
            //没有到最后3秒前端自己算
            biSaiKaiShi.cur_BiSaiM--;
            //biSaiKaiShi.uiBiSai.text = biSaiKaiShi.cur_BiSaiM.ToString();
        }
        else
        {
            Debug.LogError("比赛-前端最后3秒开始同步");
            NET_GameType();//比赛-前端最后3秒开始同步
        }
    }
    void 结算计时器()
    {
        if (!biSaiKaiShi.isJieSuan) return;
        if (biSaiKaiShi.cur_JieSuanM > 3)
        {
            //没有到最后3秒前端自己算
            biSaiKaiShi.cur_JieSuanM--;
            biSaiKaiShi.uiJieSuan.text = biSaiKaiShi.cur_JieSuanM + "s";
        }
        else
        {
            Debug.LogError("结算-前端最后3秒开始同步");
            NET_GameType();//结算-前端最后3秒开始同步
        }
    }

    //全部关闭
    public void ALLDown(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(false);
        }
    }

    //换图
    Dictionary<string, Texture2D> AllTexture2D = new Dictionary<string, Texture2D>();
    public void HuanTu(Image image, string name)
    {
        if (AllTexture2D.ContainsKey(name))
        {

            Texture2D data = AllTexture2D[name];
            image.sprite = Sprite.Create(data, new Rect(0, 0, data.width, data.height), Vector2.zero);
        }
        else
        {
#if UNITY_ANDROID
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/md5", name));
#endif
#if UNITY_IPHONE
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/iosmd5", name));
#endif
            Texture2D data = asset.LoadAsset<Texture2D>(name);
            image.sprite = Sprite.Create(data, new Rect(0, 0, data.width, data.height), Vector2.zero);
            AllTexture2D.Add(name, data);
        }
    }

    //换图
    Dictionary<string, SkeletonDataAsset> AllDogA = new Dictionary<string, SkeletonDataAsset>();
    public void HuanDogA(SkeletonGraphic image, string name)
    {
        if (AllDogA.ContainsKey(name))
        {
            SkeletonDataAsset data = AllDogA[name];
            image.skeletonDataAsset = data;
            image.Initialize(true);
        }
        else
        {
#if UNITY_ANDROID
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/md5", name));
#endif
#if UNITY_IPHONE
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/iosmd5", name));
#endif

            SkeletonDataAsset data = asset.LoadAsset<SkeletonDataAsset>(name);
            image.skeletonDataAsset = data;
            image.Initialize(true);
            AllDogA.Add(name, data);
        }
    }
    //换品质框
    public void HuanBox(int grade, Image obj)
    {
        if (grade == 0)
        {
            grade = 1;
        }
        string kuang = "k_";
        kuang = kuang + grade;
        HuanTu(obj, kuang);
    }


    public void 关闭所有界面()
    {
        shezhi.OorDBar(false);
        qiandao.OorDBar(false);
        youjian.OorDBar(false);
        buZhuoDaoJu.OorDBar(false);
        siYang.OorDBar(false);
        canZhan.OorDBar(false);
        biSai.OorDBar(false);
        cangKu.OorDBar(false);
        duihuan.OorDBar(false);
        zhuaBu.OorDBar(false);
        shijie.OorDBar(false);
        yaZhu.OorDBar(false);
        daoHang.OorDBar(false);
        mainBar.OorDBar(false);
    }

    //数值转化
    public void SetNum(Text _num, double _value)
    {
        double zhuanhua = _value;
        _num.text = zhuanhua + "";
        zhuanhua = _value / 10000;
        if (zhuanhua >= 1)
        {
            _num.text = zhuanhua.ToString("F2") + "W";
        }
        //zhuanhua = _value / 1000000;
        //if (zhuanhua >= 1)
        //{
        //    _num.text = zhuanhua.ToString("F2") + "W";
        //}
        //zhuanhua = _value / 10000000;
        //if (zhuanhua >= 1)
        //{
        //    _num.text = zhuanhua.ToString("F2") + "W";
        //}
        zhuanhua = _value / 100000000;
        if (zhuanhua >= 1)
        {
            _num.text = zhuanhua.ToString("F2")
                + "亿";
        }
    }

    #endregion

    #region 所有UI脚本

    //所有界面
    public void Loading()
    {
#if UNITY_ANDROID
            string _path = Application.persistentDataPath + "/md5";
#endif
#if UNITY_IPHONE
        string _path = Application.persistentDataPath + "/iosmd5";
#endif
        //主页
        AssetBundle asset1 = AssetBundle.LoadFromFile(Path.Combine(_path, "zhuye_ui"));
        GameObject data1 = asset1.LoadAsset<GameObject>("zhuye_ui");
        GameObject obj1 = Instantiate(data1, transform);
        mainBar = obj1.GetComponent<CMianBar>();
        //设置
        AssetBundle asset2 = AssetBundle.LoadFromFile(Path.Combine(_path, "shezhi_ui"));
        GameObject data2 = asset2.LoadAsset<GameObject>("shezhi_ui");
        GameObject obj2 = Instantiate(data2, transform);
        shezhi = obj2.GetComponent<CSheZhi>();
        //签到
        AssetBundle asset3 = AssetBundle.LoadFromFile(Path.Combine(_path, "qiandao_ui"));
        GameObject data3 = asset3.LoadAsset<GameObject>("qiandao_ui");
        GameObject obj3 = Instantiate(data3, transform);
        qiandao = obj3.GetComponent<CQianDao>();
        //邮件
        AssetBundle asset4 = AssetBundle.LoadFromFile(Path.Combine(_path, "youjian_ui"));
        GameObject data4 = asset4.LoadAsset<GameObject>("youjian_ui");
        GameObject obj4 = Instantiate(data4, transform);
        youjian = obj4.GetComponent<CYouJian>();
        //捕捉道具
        AssetBundle asset5 = AssetBundle.LoadFromFile(Path.Combine(_path, "buzhuodaoju_ui"));
        GameObject data5 = asset5.LoadAsset<GameObject>("buzhuodaoju_ui");
        GameObject obj5 = Instantiate(data5, transform);
        buZhuoDaoJu = obj5.GetComponent<CBUZhuoDaoJu>();
        //仓库
        AssetBundle asset6 = AssetBundle.LoadFromFile(Path.Combine(_path, "cangku_ui"));
        GameObject data6 = asset6.LoadAsset<GameObject>("cangku_ui");
        GameObject obj6 = Instantiate(data6, transform);
        cangKu = obj6.GetComponent<CCangKu>();
        //饲养
        AssetBundle asset7 = AssetBundle.LoadFromFile(Path.Combine(_path, "siyang_ui"));
        GameObject data7 = asset7.LoadAsset<GameObject>("siyang_ui");
        GameObject obj7 = Instantiate(data7, transform);
        siYang = obj7.GetComponent<CSiYang>();
        //比赛
        AssetBundle asset8 = AssetBundle.LoadFromFile(Path.Combine(_path, "bisai_ui"));
        GameObject data8 = asset8.LoadAsset<GameObject>("bisai_ui");
        GameObject obj8 = Instantiate(data8, transform);
        biSai = obj8.GetComponent<CBiSai>();
        //参战
        AssetBundle asset9 = AssetBundle.LoadFromFile(Path.Combine(_path, "canzhan_ui"));
        GameObject data9 = asset9.LoadAsset<GameObject>("canzhan_ui");
        GameObject obj9 = Instantiate(data9, transform);
        canZhan = obj9.GetComponent<CCanZhan>();
        //兑换
        AssetBundle asset10 = AssetBundle.LoadFromFile(Path.Combine(_path, "duihuan_ui"));
        GameObject data10 = asset10.LoadAsset<GameObject>("duihuan_ui");
        GameObject obj10 = Instantiate(data10, transform);
        duihuan = obj10.GetComponent<CDuiHuan>();

        //比赛押注
        AssetBundle asset12 = AssetBundle.LoadFromFile(Path.Combine(_path, "bisaiyazhu_ui"));
        GameObject data12 = asset12.LoadAsset<GameObject>("bisaiyazhu_ui");
        GameObject obj12 = Instantiate(data12, transform);
        yaZhu = obj12.GetComponent<CYaZhu>();
        //导航
        AssetBundle asset11 = AssetBundle.LoadFromFile(Path.Combine(_path, "daohang_ui"));
        GameObject data11 = asset11.LoadAsset<GameObject>("daohang_ui");
        GameObject obj11 = Instantiate(data11, transform);
        daoHang = obj11.GetComponent<CDaoHang>();
        //世界
        AssetBundle asset13 = AssetBundle.LoadFromFile(Path.Combine(_path, "shijie_ui"));
        GameObject data13 = asset13.LoadAsset<GameObject>("shijie_ui");
        GameObject obj13 = Instantiate(data13, transform);
        shijie = obj13.GetComponent<CShiJie>();
        //抓捕结果
        AssetBundle asset14 = AssetBundle.LoadFromFile(Path.Combine(_path, "zhuanbujieguo_ui"));
        GameObject data14 = asset14.LoadAsset<GameObject>("zhuanbujieguo_ui");
        GameObject obj14 = Instantiate(data14, transform);
        zhuaBu = obj14.GetComponent<CZhuaBu>();
        //登录
        AssetBundle asset15 = AssetBundle.LoadFromFile(Path.Combine(_path, "denglu_ui"));
        GameObject data15 = asset15.LoadAsset<GameObject>("denglu_ui");
        GameObject obj15 = Instantiate(data15, transform);
        denglu = obj15.GetComponent<CDengLu>();
        //比赛开始
        AssetBundle asset16 = AssetBundle.LoadFromFile(Path.Combine(_path, "bisaikaishi_ui"));
        GameObject data16 = asset16.LoadAsset<GameObject>("bisaikaishi_ui");
        GameObject obj16 = Instantiate(data16, transform);
        biSaiKaiShi = obj16.GetComponent<CUIBiSai>();
        //提示框
        AssetBundle asset17 = AssetBundle.LoadFromFile(Path.Combine(_path, "tishi_ui"));
        GameObject data17 = asset17.LoadAsset<GameObject>("tishi_ui");
        GameObject obj17 = Instantiate(data17, transform);
        cUITips = obj17.GetComponent<CUITips>();
        //比赛跑到
        AssetBundle asset18 = AssetBundle.LoadFromFile(Path.Combine(_path, "bisaipaodao"));
        GameObject data18 = asset18.LoadAsset<GameObject>("bisaipaodao");
        GameObject obj18 = Instantiate(data18, null);
        biSaiManager = obj18.GetComponent<BiSaiManager>();

        loding.YanChi();
    }

    //主页
    public CMianBar mainBar;
    //设置
    public CSheZhi shezhi;
    //签到
    public CQianDao qiandao;
    //邮件
    public CYouJian youjian;
    //捕捉道具
    public CBUZhuoDaoJu buZhuoDaoJu;
    //仓库
    public CCangKu cangKu;
    //饲养
    public CSiYang siYang;
    //比赛
    public CBiSai biSai;
    //参战
    public CCanZhan canZhan;
    //兑换
    public CDuiHuan duihuan;
    //导航
    public CDaoHang daoHang;
    //比赛押注
    public CYaZhu yaZhu;
    //世界
    public CShiJie shijie;
    //抓捕结果
    public CZhuaBu zhuaBu;
    //登录
    public CDengLu denglu;
    //比赛开始
    public CUIBiSai biSaiKaiShi;
    //提示框
    public CUITips cUITips;
    //比赛管理器
    public BiSaiManager biSaiManager;


    //加载
    public CLoding loding;
    #endregion

    #region 音乐
    public AudioSource bj_music;
    public AudioSource yx_music;
    //音乐
    Dictionary<string, AudioClip> AllMusic = new Dictionary<string, AudioClip>();
    public void HuanBGMusic(string name)
    {
        if (AllMusic.ContainsKey(name))
        {
            AudioClip data = AllMusic[name];
            if (bj_music.clip != data)
            {
                bj_music.Stop();
                bj_music.clip = data;
                bj_music.Play();
                bj_music.loop = true;
            }
        }
        else
        {
#if UNITY_ANDROID
           AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/md5", name));
#endif
#if UNITY_IPHONE
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/iosmd5", name));
#endif

            AudioClip clip = asset.LoadAsset<AudioClip>(name);
            AllMusic.Add(name, clip);
            bj_music.clip = clip;
            bj_music.Play();
            bj_music.loop = true;
        }
    }
    public void HuanMusic(string name, bool b = true)
    {
        if (AllMusic.ContainsKey(name))
        {
            AudioClip data = AllMusic[name];
            yx_music.PlayOneShot(data);
        }
        else
        {
#if UNITY_ANDROID
           AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/md5", name));
#endif
#if UNITY_IPHONE
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/iosmd5", name));
#endif
             
            AudioClip data = asset.LoadAsset<AudioClip>(name);
            AllMusic.Add(name, data);
            if (b)
            {
                yx_music.PlayOneShot(data);
            }
        }
    }

    #endregion

    #region 登录
    //扫码授权登录
    public void NetSendLogin(string _id)
    {
        JsonData data = new JsonData();
        data["logonCredentials"] = _id;
        string id = PlayerPrefs.GetString("id");
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/auth", data));
    }
    //快速登录
    public void NetSpeedSendLogin()
    {
        JsonData data = new JsonData();
        string id = PlayerPrefs.GetString("id");
        data["openId"] = id;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/login", data));
        Debug.LogError(id);
    }
    //快速登录
    public void NetSpeedSendLogin(string _id)
    {
        JsonData data = new JsonData();
        data["openId"] = _id;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/login", data));
    }
    #endregion

    #region 服务器—个人信息
    public MainInfoData mainDataInfo = new MainInfoData();
    //获取个人信息
    public void NET_GetMainInfo()
    {
        Debug.LogError("获取个人信息");
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("user/info"));
    }
    //修改个人信息
    public void NET_SetMainInfo(string key, int value)
    {
        JsonData data = new JsonData();
        data[key] = value;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/update", data));
    }
    public void NET_SetMainInfo(string key, string value)
    {
        JsonData data = new JsonData();
        data[key] = value;
        if (key == "userName")
        {
            data["isFreeNameEdit"] = mainDataInfo.isFreeNameEdit;
        }
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/update", data));
    }
    #endregion

    #region 服务器—地图信息
    public List<MapData> allMapData = new List<MapData>();

    //获取所有地图信息
    public void NET_GetMapData()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("map/all"));
    }
    //地图详情-跳转
    public void NET_GetMapDate(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("map/detail", "/" + _id));
    }
    //地图详情
    public void NET_GetMapDeblock(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("map/deblock", "/" + _id));
    }
    //购买
    public void NET_Buy(int _id)
    {
        string str = "?id=" + _id;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("pay/catch/equip", str));
    }
    #endregion

    #region 获取服务器—捕捉道具所有属性
    public List<BuZhuoDaoJuData> buzhuodaojuList = new List<BuZhuoDaoJuData>();
    //根据ID获取
    public BuZhuoDaoJuData GetIDBuZhuoDaoJuData(int _id)
    {
        for (int i = 0; i < buzhuodaojuList.Count; i++)
        {
            if (buzhuodaojuList[i].id == _id)
            {
                return buzhuodaojuList[i];
            }
        }
        return null;
    }
    //获取捕捉道具列表
    public void NET_BuZhuoListDate(int _type)
    {
        JsonData data = new JsonData();
        data["catchType"] = _type;
        data["page"] = 1;
        data["size"] = 100;
        string str = "?catchType=" + _type + "&page=1&size=10";
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("catch/equip/all", str));
    }
    //获取捕捉道具某一个的详情
    public void NET_GetBZDate(int _id)
    {
        Debug.LogError(_id);
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("catch/equip/detail", "/" + _id));
    }
    //使用捕捉道具
    public void NET_CatchEquipUse(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("catch/equip/use", "/" + _id));
    }
    //卸下
    public void NET_CatchEquipUnsnatch(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("catch/equip/unsnatch", "/" + _id));
    }
    //当前穿戴装备
    public List<BuZhuoDaoJuData> curCathEquip = new List<BuZhuoDaoJuData>();
    public BuZhuoDaoJuData GetCurCathEquip(int _type)
    {
        for (int i = 0; i < curCathEquip.Count; i++)
        {
            if (curCathEquip[i].catchType == _type)
            {
                return curCathEquip[i];
            }
        }
        return null;
    }
    public BuZhuoDaoJuData GetCurCathEquip(string _name)
    {
        for (int i = 0; i < curCathEquip.Count; i++)
        {
            if (curCathEquip[i].equipName == _name)
            {
                return curCathEquip[i];
            }
        }
        return null;
    }
    //获取当前装备道具
    public void NET_CatchEquipCur()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("catch/equip/current"));
    }
    #endregion

    #region 获取服务器—宠物所有属性
    public List<ChongWuData> AllChongWuData = new List<ChongWuData>();
    //获取狗狗列表
    public void NET_GetChongWuDataList()
    {
        JsonData data = new JsonData();
        data["page"] = 1;
        data["size"] = 100;
        string str = "?page=" + 1 + "&size=" + 100;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/dog/all", str));
    }
    //使用狗狗
    public void NET_UpdateChonwuDate(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().PutRequest("user/dog/use", "/" + _id));
    }
    //根据ID获取
    public ChongWuData GetIDChongWuData(int _id)
    {
        for (int i = 0; i < AllChongWuData.Count; i++)
        {
            if (AllChongWuData[i].id == _id)
            {
                return AllChongWuData[i];
            }
        }
        return null;
    }
    //根据ID获取
    public ChongWuData GetIDChongWuData()
    {
        for (int i = 0; i < AllChongWuData.Count; i++)
        {
            if (AllChongWuData[i].isUse == 1)
            {
                return AllChongWuData[i];
            }
        }
        return null;
    }
    public int GetDogID()
    {
        for (int i = 0; i < AllChongWuData.Count; i++)
        {
            if (AllChongWuData[i].isUse == 1)
            {
                return AllChongWuData[i].id;
            }
        }
        return -1;
    }

    #endregion

    #region 饲养
    //吞噬
    public void NET_TunShi(int _dogID)
    {
        string str = "?dogId=" + _dogID;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("gobble", str));
    }
    //饲养
    public void NET_Raise(int _type, int _id, int _dogID)
    {
        if (_type == 0) { _type = 1; }
        string str = "?dogId=" + _dogID + "&id=" + _id + "&type=" + _type;
        Debug.LogError(str);
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("raise", str));
    }
    public int buzhuoID;
    public int discardType;
    //丢弃
    public void NET_DiuQi(int id, int _type)
    {
        string str = "?id=" + id + "&type=" + _type;
        Debug.LogError(str);
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("discard", str));
    }
    //使用
    public void NET_Use(int _dogId, int _id)
    {
        string str = "?dogId=" + _dogId + "&propId=" + _id;
        Debug.LogError(_id);
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("store/use/prop", str));
    }
    #endregion

    #region 获得服务器-珍宝 饲料 野生所有属性
    //捕捉所有道具属性
    public List<ItemData> allItemData = new List<ItemData>();
    //根据类型获取
    public List<ItemData> GetItemData(int _type)
    {
        List<ItemData> list = new List<ItemData>();
        for (int i = 0; i < allItemData.Count; i++)
        {
            list.Add(allItemData[i]);
        }
        return list;
    }
    //根据ID获取
    public ItemData GetIDItemData(int _id)
    {
        for (int i = 0; i < allItemData.Count; i++)
        {
            if (allItemData[i].id == _id)
            {
                return allItemData[i];
            }
        }
        return null;
    }
    public int tagType;
    //获取物品
    public void NET_GetItemData(int _type)
    {
        tagType = _type;
        JsonData data = new JsonData();
        data["page"] = 1;
        data["size"] = 100;
        data["type"] = _type;
        string str = "?page=" + 1 + "&size=" + 100 + "&type=" + _type;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("store/all", str));
    }
    #endregion

    #region 获取服务器-装备所有属性
    //所有装备属性
    public List<ZhuangBeiData> allZhuangBeiData = new List<ZhuangBeiData>();
    //根据ID获取
    public ZhuangBeiData GetIDZBData(int _id)
    {
        for (int i = 0; i < allZhuangBeiData.Count; i++)
        {
            if (allZhuangBeiData[i].id == _id)
            {
                return allZhuangBeiData[i];
            }
        }
        return null;
    }

    //当前战斗物品
    public List<FightData> allFightItem = new List<FightData>();
    //战斗道具
    public void NET_Fight_DaoJu()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("store/all/Prop"));
    }

    #endregion

    #region 获取服务器—宠物战斗属性
    public List<ZhanDouChongWuData> allZhanDouChongWuData = new List<ZhanDouChongWuData>();
    public ZhanDouChongWuData GetZhanDouChongWuID(int _id)
    {
        for (int i = 0; i < allZhanDouChongWuData.Count; i++)
        {
            if (allZhanDouChongWuData[i].id == _id)
            {
                return allZhanDouChongWuData[i];
            }
        }
        return null;
    }
    public void NET_FightlDog()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("user/dog/fight"));
    }
    #endregion

    #region 获取服务器—抓捕消息

    //发送捕捉消息
    public void NET_SendCapture(int type, int id)
    {
        JsonData data = new JsonData();
        data["type"] = type + 1;
        data["id"] = id;
        string str = "?id=" + id + "&type=" + type;
        if (id == -1)
        {
            str = "?id=" + id;
            data["type"] = type + 1;
            data["id"] = id;
        }
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("catch", str));
    }
    #endregion

    #region 服务器—签到
    public List<SignInData> allSignInData = new List<SignInData>();
    //获取签到列表
    public void NET_GetSingInList()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("signIn/all"));
    }
    //签到
    public void NET_SingIn()
    {
        StartCoroutine(HttpManager.GetHttpManager().PutRequest("signIn/check"));
    }
    //购买签到
    public void NET_PaySign()
    {
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("pay/sign/in"));
    }
    #endregion

    #region 服务器—邮件
    public List<MailData> allMailData = new List<MailData>();
    //获取签到列表
    public void NET_GetMailList()
    {
        JsonData data = new JsonData();
        data["page"] = 1;
        data["size"] = 100;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("mail/all", data));
    }
    //获取邮件详情
    public void NET_GetMailDate(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("mail/detail", "/" + _id));
    }
    //领取邮件
    public void NET_ReceiveMailDate(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().PutRequest("mail/receive", "/" + _id));
    }
    //一键删除
    public void NET_AllShanChu()
    {
        StartCoroutine(HttpManager.GetHttpManager().PutDelete("mail/delete", ""));
    }
    //一键领取
    public void NET_AllLingQu()
    {
        StartCoroutine(HttpManager.GetHttpManager().PutRequest("mail/receive/all", ""));
    }
    #endregion

    #region 服务器—对战装备接口
    //捕捉所有道具属性
    public List<ZhuangBeiData> allZhuangDZBeiData = new List<ZhuangBeiData>();
    //根据ID获取
    public ZhuangBeiData GetIDZDZBData(int _id)
    {
        for (int i = 0; i < allZhuangDZBeiData.Count; i++)
        {
            if (allZhuangDZBeiData[i].id == _id)
            {
                return allZhuangDZBeiData[i];
            }
        }
        return null;
    }
    public void NET_EquipAll()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("fight/equip/all"));
    }
    #endregion

    #region 报名比赛
    //报名比赛
    public void NET_BaoMing(int _dogID, int[] _zhuangbeiID, List<DataUsing> list)
    {
        JsonData data = new JsonData();
        data["dogId"] = _dogID;
        string str = "?dogId=" + _dogID;
        for (int i = 0; i < _zhuangbeiID.Length; i++)
        {
            if (_zhuangbeiID[i] != 0)
            {
                ZhuangBeiData _data= GetIDZDZBData(_zhuangbeiID[i]);
                str = str + "&equip0" + (i + 1) + "=" + _zhuangbeiID[i];
                if (_data.ratio == 0)
                {
                    str = str + "&equipType0" + (i + 1) + "=" + 2;
                }
                else
                {
                    str = str + "&equipType0" + (i + 1) + "=" + 1;
                }
                
            }
        }
        if (list[0].isUse) { str = str + "&brawn" + "=" + list[0].id; }
        if (list[1].isUse) { str = str + "&gameCard" + "=" + list[1].id; }
        if (list[2].isUse) { str = str + "&fight" + "=" + list[2].id; }
        if (list[3].isUse) { str = str + "&cool" + "=" + list[3].id; }
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("mate/enroll", str));
    }
    //取消报名
    public void NET_CancelBaoMing(int _dogID)
    {
        JsonData data = new JsonData();
        data["dogId"] = -1;
        string str = "?dogId=" + _dogID;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("mate/cancel", str));
    }

    public BiSaiData myBiSaiData = new BiSaiData();
    public BiSaiData worlBiSaiData = new BiSaiData();
    //查询报名情况/场次序号
    public void NET_FinBiSai()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("matcher/mate"));
    }
    #endregion

    #region 押注

    //宠物标
    public List<ZhanDouChongWuData> allUseDog = new List<ZhanDouChongWuData>();
    public List<BetData> allBet = new List<BetData>();
    //申请押注信息
    public void NET_BetData()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("game/record"));
    }
    //获得比赛狗狗
    public void NET_BiSaiDog()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("game/all/dog"));
    }
    //获取倍率
    public void NET_GetBeiLv(int type, List<List<int>> hao = null)
    {
        JsonData data = new JsonData();
        if (type == 1)
        {
            data["type"] = type;
        }
        //名次
        if (type != 1)
        {
            data["type"] = type;

            //前二
            if (hao[0][0] != 0 && hao[0][1] != 0)
            {
                JsonData qianer = new JsonData();
                qianer["one"] = hao[0][0];
                qianer["oneDogId"] = allUseDog[hao[0][0] - 1].id;
                qianer["two"] = hao[0][1];
                qianer["twoDogId"] = allUseDog[hao[0][1] - 1].id;
                data["oneMap"] = qianer;
            }
            //前三
            if (hao[1][0] != 0 && hao[1][1] != 0 && hao[1][2] != 0)
            {
                JsonData qiansan = new JsonData();
                qiansan["one"] = hao[1][0];
                qiansan["oneDogId"] = allUseDog[hao[1][0] - 1].id;
                qiansan["two"] = hao[1][1];
                qiansan["twoDogId"] = allUseDog[hao[1][1] - 1].id;
                qiansan["three"] = hao[1][2];
                qiansan["threeDogId"] = allUseDog[hao[1][2] - 1].id;
                data["twoMap"] = qiansan;
            }
            //前四
            if (hao[2][0] != 0 && hao[2][1] != 0 && hao[2][2] != 0 && hao[2][3] != 0)
            {
                JsonData qiansi = new JsonData();
                qiansi["one"] = hao[2][0];
                qiansi["oneDogId"] = allUseDog[hao[2][0] - 1].id;

                qiansi["two"] = hao[2][1];
                qiansi["twoDogId"] = allUseDog[hao[2][1] - 1].id;

                qiansi["three"] = hao[2][2];
                qiansi["threeDogId"] = allUseDog[hao[2][2] - 1].id;

                qiansi["four"] = hao[2][3];
                qiansi["fourDogId"] = allUseDog[hao[2][3] - 1].id;

                data["threeMap"] = qiansi;
            }
        }
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("game/win/ratio", data));
    }
    //独赢押注
    public void NET_DuYi(int _hao, int _zhu)
    {
        JsonData data = new JsonData();
        data["dogNumber"] = _hao;
        data["stakeNumber"] = _zhu;
        data["dogId"] = allUseDog[_hao - 1].id;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("game/stake/one", data));
    }
    //名次押注
    public void NET_MingCi(List<List<int>> _hao, List<int> zhu)
    {
        int vualue = 0;
        for (int i = 0; i < zhu.Count; i++)
        {
            if (zhu[i] == 0)
            {
                vualue++;
            }
        }
        if (vualue == 3)
        {
            cUITips.Tips("请选择您心仪的狗进行赞助"); return;
        }

        if (_hao.Count == 0) { cUITips.Tips("请选择您心仪的狗进行赞助"); return; }
        JsonData data = new JsonData();
        for (int i = 0; i < _hao.Count; i++)
        {
            switch (_hao[i].Count)
            {
                case 2:
                    //前二
                    JsonData qianer = new JsonData();
                    qianer["one"] = _hao[i][0];
                    qianer["oneDogId"] = allUseDog[_hao[i][0] - 1].id;
                    qianer["two"] = _hao[i][1];
                    qianer["twoDogId"] = allUseDog[_hao[i][1] - 1].id;
                    qianer["stakeNumber"] = zhu[0];
                    data["oneMap"] = qianer;
                    break;
                case 3:
                    //前三
                    JsonData qiansan = new JsonData();
                    qiansan["one"] = _hao[i][0];
                    qiansan["oneDogId"] = allUseDog[_hao[i][0] - 1].id;
                    qiansan["two"] = _hao[i][1];
                    qiansan["twoDogId"] = allUseDog[_hao[i][1] - 1].id;
                    qiansan["three"] = _hao[i][2];
                    qiansan["threeDogId"] = allUseDog[_hao[i][2] - 1].id;
                    qiansan["stakeNumber"] = zhu[1];
                    data["twoMap"] = qiansan;
                    break;
                case 4:
                    //前四
                    JsonData qiansi = new JsonData();
                    qiansi["one"] = _hao[i][0];
                    qiansi["oneDogId"] = allUseDog[_hao[i][0] - 1].id;
                    qiansi["two"] = _hao[i][1];
                    qiansi["twoDogId"] = allUseDog[_hao[i][1] - 1].id;
                    qiansi["three"] = _hao[i][2];
                    qiansi["threeDogId"] = allUseDog[_hao[i][2] - 1].id;
                    qiansi["four"] = _hao[i][3];
                    qiansi["fourDogId"] = allUseDog[_hao[i][3] - 1].id;
                    qiansi["stakeNumber"] = zhu[2];
                    data["threeMap"] = qiansi;
                    break;
            }

        }
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("game/stake/more", data));
    }
    //包围
    public void NET_BaoWei(List<List<int>> _hao, List<int> zhu)
    {
        int vualue = 0;
        for (int i = 0; i < zhu.Count; i++)
        {
            if (zhu[i] == 0)
            {
                vualue++;
            }
        }
        if (vualue == 3)
        {
            cUITips.Tips("请选择您心仪的狗进行赞助"); return;
        }
        if (_hao.Count == 0) { cUITips.Tips("请选择您心仪的狗进行赞助"); return; }
        JsonData data = new JsonData();
        for (int i = 0; i < _hao.Count; i++)
        {
            switch (_hao[i].Count)
            {
                case 2:
                    //前二
                    JsonData qianer = new JsonData();
                    qianer["one"] = _hao[i][0];
                    qianer["oneDogId"] = allUseDog[_hao[i][0] - 1].id;
                    qianer["two"] = _hao[i][1];
                    qianer["twoDogId"] = allUseDog[_hao[i][1] - 1].id;
                    qianer["stakeNumber"] = zhu[0];
                    data["oneMap"] = qianer;
                    break;
                case 3:
                    //前三
                    JsonData qiansan = new JsonData();
                    qiansan["one"] = _hao[i][0];
                    qiansan["oneDogId"] = allUseDog[_hao[i][0] - 1].id;
                    qiansan["two"] = _hao[i][1];
                    qiansan["twoDogId"] = allUseDog[_hao[i][1] - 1].id;
                    qiansan["three"] = _hao[i][2];
                    qiansan["threeDogId"] = allUseDog[_hao[i][2] - 1].id;
                    qiansan["stakeNumber"] = zhu[1];
                    data["twoMap"] = qiansan;
                    break;
                case 4:
                    //前四
                    JsonData qiansi = new JsonData();
                    qiansi["one"] = _hao[i][0];
                    qiansi["oneDogId"] = allUseDog[_hao[i][0] - 1].id;
                    qiansi["two"] = _hao[i][1];
                    qiansi["twoDogId"] = allUseDog[_hao[i][1] - 1].id;
                    qiansi["three"] = _hao[i][2];
                    qiansi["threeDogId"] = allUseDog[_hao[i][2] - 1].id;
                    qiansi["four"] = _hao[i][3];
                    qiansi["fourDogId"] = allUseDog[_hao[i][3] - 1].id;
                    qiansi["stakeNumber"] = zhu[2];
                    data["threeMap"] = qiansi;
                    break;
            }
        }
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("game/stake/surround", data));
    }
    //奖池
    public void NET_JiangChi()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("game/jackpot"));
    }
    //游戏状态
    public void NET_GameType()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("game/status"));
    }
    public List<GameOverData> gameOverData = new List<GameOverData>();
    //游戏结果
    public void NET_GameResult()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("game/result"));
    }
    #endregion

    #region 广告
    public List<ADData> allAD = new List<ADData>();
    public ADData GetAD(int _id)
    {
        for (int i = 0; i < allAD.Count; i++)
        {
            if (allAD[i].id == _id)
            {
                return allAD[i];
            }
        }
        return null;
    }
    //获取广告列表
    public void NET_GetAD()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("ad/all"));
    }
    //观看广告开始/广告详情
    public void NET_GetADData(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("ad/detail", "/" + _id));
    }
    //观看完毕
    public void NET_ADOver(int _id)
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("ad/look", "/" + _id));
    }
    #endregion

    #region 充值提现
    //总金额
    public void NET_AsgTotal()
    {
        StartCoroutine(HttpManager.GetHttpManager().GetRequest("user/asg/total"));
    }
    //充值
    public void NET_Recharge(int _value)
    {
        string str = "?money=" + _value;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/recharge", str));
    }
    //提现
    public void NET_withdraw(int _value)
    {
        string str = "?money=" + _value;
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/withdraw", str));
    }
    #endregion

    #region NFT 
    public void NET_NFT(int _type, int _id)
    {
        string str = "?id=" + _id + "&type=" + _type;
        Debug.LogError(str);
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("nft/cast", str));
    }
    public void NET_NFTOut(int _type, int _id)
    {
        string str = "?id=" + _id + "&type=" + _type;
        Debug.LogError(str);
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("nft/out", str));
    }
    #endregion

    #region 购买体力
    //购买体力配置
    public void NET_brawnConfig()
    {
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("user/brawn/config"));
    }
    //购买体力
    public void NET_PayReplenish()
    {
        StartCoroutine(HttpManager.GetHttpManager().PostRequest("pay/replenish"));
    }
    #endregion
}
//捕捉道具属性
public class BuZhuoDaoJuData
{
    public int isDraw;
    public int durabilityResidue;
    public int durabilityMax;
    //额外效果
    public List<string> allXiaoGuo = new List<string>();
    //是否使用
    public int isUse;
    public int isNft;

    public int userId;
    //id
    public int equipId;
    public int id;
    //名字
    public string equipName;
    //装备说明
    public string equipDesc;
    //图片
    public string imgName;
    //装备等级
    public int equipGrade;
    //捕捉类型
    public int catchType;
    //行为类型
    public int deedType;
    //额外一个
    public int extraOne;
    public string extraOneDesc = "";
    //额外二个
    public int extraTwo;
    public string extraTwoDesc = "";
    //是不是珍宝
    public int isGem;
    //数量
    public int equipNum;
    public long createTime;
    public long updateTime;
    public int grade;

}
//个人信息
public class MainInfoData
{

    //ID
    public int userId;
    //名字
    public string userName;
    //头像地址
    public string userAvatar;
    //狗币
    public int dogCoin;
    //总体力值
    public int totalMuscleNum;
    //当前体力值
    public int residueMuscleNum;
    //背景音乐 【0:未开启 1:已开启】
    public int isMusic;
    //音效
    public int isEffect = 1;
    //用户状态 【1:正常 2:禁用】
    public int status;
    //语言 zh en
    public string language;
    //签到天数
    public int signInDayNum;
    //当前是否签到 1:签到 0:未签到
    public int isTodayCheck;
    //是否有新邮件
    public int isHaveUnread;
    //地图ID
    public int mapId;
    //钱包登录凭证
    public string logonCredentials;
    //打开地图数量
    public int openMapNum;
    //"是否免费编辑名字（0:否 1:是）"
    public int isFreeNameEdit = 0;
    public string registerIp;
    public long registerTime;
    public string lastLoginIp;
    public long lastLoginTime;
    public int equipTossGrade;
    public int equipHideGrade;
    public int equipGnawGrade;
    public int equipSoundGrade;
    /// <summary>
    /// 胜利场次
    /// </summary>
    public int winNum = 0;
    /// <summary>
    /// 宠物数量
    /// </summary>
    public int dogNum = 0;
    /// <summary>
    /// 珍宝数量
    /// </summary>
    public int gemNum = 0;
    public UseData dogMap;
    public int isSignIn;
}
//当前正在使用的地图信息
public class UseData
{
    public int id;
    public string dogImg;
    public string dogName;
}
//地图属性
public class MapData
{
    public int id;
    public string mapName;
    //宠物率
    public double petRatio;
    //装备率
    public double equipRatio;
    //珍宝率
    public double gemRatio;
    //饲料率
    public double forageRatio;
    //野生率
    public double wildRatio;
    //是否打开
    public bool isOpen;
    public string imgName;
    //宠物产出概率
    public GaiLv[] petRatioMap;
    //装备产出概率
    public GaiLv[] equipRatioMap;
    //饲料产出概率
    public GaiLv[] forageRatioMap;
    //珍宝产出概率
    public GaiLv[] gemRatioMap;
    //野生产出概率
    public GaiLv[] wildRatioMap;

    //消耗体力
    public int useBrawn;
    public long createTime;
    public long updateTime;
    public int useAgs;
    //开启条件
    public ConditionsData[] catchPropMap;
}
public class GaiLv
{
    public string typeDesc;
    //等级
    public int grade;
    //概率
    public float odds;
    public int type;
}
public class ConditionsData
{
    //物品图片名字
    public string image;
    //物品名字
    public string name;
    //是否拥有
    public bool isHave;
    public int id;
    public int type = 1;
    public int Unum;
}
//签到列表
public class SignInData
{
    public int id;
    public string day;
    public string content;
    public int awardNum;
    public int isAttribute;
    public int awardType;
    public int kindId;
    public long createTime;
    public long updateTime;
    public bool isCheck;
    public bool isToday;
    public string imgName;
    public string name;
}
//邮件列表
public class MailData
{
    public int id;
    //说明
    public string mailTitle;
    //内容
    public string mailContent;
    //数量
    public int awardNum;
    //是否已读 0未读
    public int isRead;
    //是否领取 0未领取
    public int isReceive;
    //物品图片 多个会,分割
    public string imgName = "";
    //物品数量 多个会,分割
    public string gameAward = "";

    public int isAttribute;
    public int type;
    public int awardType;
    public int kindId;
    public int userId;
    public long createTime;
    public long updateTime;

}
//通用数据
public class AllData
{
    public int isDraw;
    public int nftType;
    public int isNft;
    public int id;
    public double ratio = 0;
    public int isIgnoreTalent = 0;
    public int discardType;
    //狗狗的~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //等级 品质
    public int dogGrade = 1;
    //名字
    public string dogName;
    //说明
    public string dogDesc;
    //图片
    public string imgName;
    //品种
    public string dogBreed;
    //战斗力
    public double fightingNum;
    //最大成长
    public int maxGrowUpNum;
    //当前成长
    public int currentGrowUpNum;
    //天赋
    public double inbornNum;
    //主人
    public int userId;
    //是否使用
    public int isUse;
    //锁定
    public int isCool;
    //动画名字
    public string animationName;
    public int isGame;
    public long coolTime;
    public int morale;
    public int dogId;
    //速度
    public int speed;
    //心情
    public int mood;
    //耐力
    public int endurance;
    //运气 幸运
    public int luck;

    //物品的~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //数量
    public int num;
    public string img;
    public string name;
    public string desc;
    //1饲养珍宝2使用道具   装备得时候就是装备槽
    public int type;
    public int grade = 0;
}
//宠物数据
public class ChongWuData : AllData
{

}
//珍宝 饲料 野生
public class ItemData : AllData
{

}
//宠物战斗属性
public class ZhanDouChongWuData : AllData
{
    public int[] zhuangbei = { 0, 0, 0 };
    //冲刺数量
    public int spurtNum;
    //主人名字
    public string masterName;
    //跑完秒数
    public double finishSec;
    //名次
    public int ranking;

}
//装备属性
public class ZhuangBeiData : AllData
{
    public string typeName = "";
    public int durabilityResidue;
    public int durabilityMax;
    //额外一个
    public int extraOne;
    public string extraOneDesc = "";
    //额外二个
    public int extraTwo;
    public string extraTwoDesc = "";
}
//比赛状态
public class BiSaiData
{
    public int etcNum;
    public int type = -1;
    public string orderNum;
}
//押注信息
public class BetData
{
    public int id;
    public string gameNum;
    public string userName;
    public int stakeType;
    public int selectType;
    public string dogNum;
    public int pourNum;
    public long createTime;
    public int userId;
}

//游戏结果
public class GameOverData
{
    public int ranking;
    public string masterName;
    public string dogName;
    public int brawn;
    public int dogCoin;
}

//广告
public class ADData
{
    public int id;
    //广告地址
    public string adUrl;
    //跳转地址
    public string jumpUrl;
    public string awardName;
    //图片名字
    public string imgName;
    public int awardType;
    //数量
    public int awardNum;
    public bool isLook;
}

//战斗物品
public class FightData
{
    public int isDraw;
    public int grade;
    public int id;
    public int userId;
    public int propId;
    public string propName;
    public string propDesc;
    public string imgName;
    public int propType;
    public int useType;
    public int attributeType;
    public int propNum;
    public int isNft;
}