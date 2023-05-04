using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class BiSaiManager : MonoBehaviour
{
    //所有狗狗资源  0-161
    public Dictionary<string, GameObject> allRDogData = new Dictionary<string, GameObject>();
    //创建狗狗
    public GameObject CreateDog(string _name)
    {
        if (allRDogData.ContainsKey(_name))
        {

            return allRDogData[_name];
        }
        else
        {
#if UNITY_ANDROID
           AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/md5", _name));
#endif
#if UNITY_IPHONE
            AssetBundle asset = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath + "/iosmd5", _name));
#endif

            if (asset != null)
            {
                GameObject data = asset.LoadAsset<GameObject>(_name);
#if UNITY_IPHONE
                //ios 材质丢失  
                //材质球        格式：gou_1_Material 2 3 4。。。  gou_1_Material
                //_name =       格式：AniDog_1
                string[] info = _name.Split('_');
                Debug.LogError("gou_" + info[1] + "_Material");
                Material m = asset.LoadAsset<Material>("gou_" + info[1] + "_Material");
                Debug.LogError(m);
                if (m)
                {
                    m.shader = Shader.Find("Spine/Skeleton");
                    data.GetComponent<MeshRenderer>().material = m;
                }
#endif

                allRDogData.Add(_name, data);
                return allRDogData[_name];
            }
        }
        return null;
    }
    //0=比赛准备阶段   1=比赛进行种  2=比赛结束
    public int gameType = 0;
    //比赛是否开始
    Transform _MainCamera;
    void Start()
    {
        _MainCamera = GameObject.Find("Main Camera").transform;
        FinBtn();
        for (int i = 0; i < startTra.Count; i++)
        {
            startTra[i].position = startTra1[i].position;
        }
        for (int i = 0; i < endTra.Count; i++)
        {
            endTra[i].position = endTra1[i].position;
        }
    }

    void Update()
    {
        Follow();
    }

    #region 赛前准备
    public void Init()
    {
        //摄像机归位
        _MainCamera.position = Vector3.zero;
        //名次全部不显示
        mingci = new bool[] { false, false, false, false, false, false };
        AllDownMingCi();
        //删除上把的狗
        for (int i = 0; i < dogPrant.childCount; i++)
        {
            Destroy(dogPrant.GetChild(i).gameObject);
        }
        allDog.Clear();
    }
    public Transform dogPrant;
    public List<DogManager> allDog = new List<DogManager>();
    public List<Transform> startTra = new List<Transform>();
    public List<Transform> endTra = new List<Transform>();

    public List<Transform> startTra1 = new List<Transform>();
    public List<Transform> endTra1 = new List<Transform>();
    public void AddDog5()
    {
        Init();
        for (int i = 0; i < CUIMainManager._MainManager().allUseDog.Count; i++)
        {
            GameObject _dogI = CreateDog(CUIMainManager._MainManager().allUseDog[i].animationName);
            if (_dogI == null)
            {
                _dogI = CreateDog("anidog_1");
                CUIMainManager._MainManager().cUITips.Tips(CUIMainManager._MainManager().allUseDog[i].animationName + "没有\n请截图");
            }
            GameObject obj = Instantiate(_dogI, dogPrant);
            //GameObject obj = null;
            //obj = Instantiate(CreateDog("AniDog_" +2), dogPrant);
            obj.transform.position = new Vector3(startTra[i].position.x, startTra[i].position.y, 2);
            DogManager dog = obj.GetComponent<DogManager>();
            allDog.Add(dog);
            dog.Int(i, CUIMainManager._MainManager().allUseDog[i], endTra[i].position.x);
        }
        //查找对标狗
        for (int i = 0; i < allDog.Count; i++)
        {
            if (allDog[i].data.ranking < 6)
            {
                allDog[i].duibiaoDog = GetMingCi(allDog[i].data.ranking);
            }
        }
        CUIMainManager._MainManager().biSaiKaiShi.Game();
        if (CUIMainManager._MainManager().yaZhu.GetMax() == 0)
        {
            SetKey(0);
        }
        else
        {
            SetKey(CUIMainManager._MainManager().yaZhu.GetMax());
        }
    }
    DogManager GetMingCi(int curMingCi)
    {
        for (int i = 0; i < allDog.Count; i++)
        {
            if (curMingCi + 1 == allDog[i].data.ranking)
            {
                return allDog[i];
            }
        }
        return null;
    }
    #endregion

    #region 开始比赛

    //比赛开始
    public void Run()
    {
        for (int i = 0; i < allDog.Count; i++)
        {
            allDog[i].AniSwitch(1);
            allDog[i].isRun = true;
        }
        gameType = 1;
    }
    #endregion

    #region 比赛结束
    //名次显示
    public List<SpriteRenderer> listMingCi = new List<SpriteRenderer>();
    //名次所有图片
    public List<Texture2D> listMingCiPic = new List<Texture2D>();
    //名次
    public bool[] mingci = { false, false, false, false, false, false };
    //获得当前名次
    public int GetCurMingCi()
    {
        int _mingci = 0;
        for (int i = 0; i < mingci.Length; i++)
        {
            if (mingci[i] == true)
            {
                _mingci++;
            }
        }
        return _mingci;
    }
    public void AllDownMingCi()
    {
        for (int i = 0; i < listMingCi.Count; i++)
        {
            listMingCi[i].gameObject.SetActive(false);
        }
    }
    public void OpenMingCi(int key, int mingCi)
    {
        listMingCi[key].gameObject.SetActive(true);
        listMingCi[key].sprite = Sprite.Create(listMingCiPic[mingCi], listMingCi[key].sprite.rect, new Vector2(0.5f, 0.5f));
        mingci[key] = true;
        CUIMainManager._MainManager().biSaiKaiShi.GameOver();
    }

    #endregion

    #region 摄像机跟随
    public bool bjMove;
    public int cur_Key = 0;
    public float speed = 10f;
    public void Follow()
    {
        if (gameType != 1) return;
        if (allDog.Count != 6) return;
        Vector3 targetPos = new Vector3(allDog[cur_Key].transform.position.x + 6, _MainCamera.position.y, _MainCamera.position.z);
        _MainCamera.position = Vector3.Slerp(_MainCamera.position, targetPos, speed * Time.deltaTime);
        if (_MainCamera.position.x >= 27 && _MainCamera.position.x <= 359)//326
        {
            bjMove = true;
        }
        else
        {
            bjMove = false;
        }
        if (_MainCamera.position.x <= 0)
        {
            _MainCamera.position = Vector3.zero;
            return;
        }
        if (_MainCamera.position.x >= 360)
        {
            _MainCamera.position = new Vector3(360, 0, 0);
            return;
        }
    }

    #endregion

    #region UI按钮选择
    public Transform tra;
    public List<GameObject> xuanzhong = new List<GameObject>();
    public List<Image> pic = new List<Image>();
    void FinBtn()
    {
        tra = CUIMainManager._MainManager().biSaiKaiShi.bar.transform.Find("游戏中/排序按钮").transform;
        for (int i = 0; i < tra.childCount; i++)
        {
            GameObject obj = tra.GetChild(i).gameObject;
            obj.AddComponent<UIEventListener>().OnClick += SetKey;
            xuanzhong.Add(obj.transform.Find("选中").gameObject);
            //pic.Add(obj.transform.Find("pic").GetComponent<Image>());
        }

    }
    //摄像机选择跟随
    public void SetKey(GameObject obj)
    {
        SetKey(int.Parse(obj.name));
    }
    public void SetKey(int _key)
    {
        //按钮选中
        for (int i = 0; i < xuanzhong.Count; i++)
        {
            xuanzhong[i].SetActive(false);
        }
        cur_Key = _key;
        xuanzhong[cur_Key].SetActive(true);
        //狗头提示
        for (int i = 0; i < allDog.Count; i++)
        {
            allDog[i].biaozhi.SetActive(false);
        }
        allDog[cur_Key].biaozhi.SetActive(true);
    }
    #endregion

}

