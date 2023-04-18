using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CUIBiSai : MonoBehaviour
{
    public GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
    }
    public GameObject time_tips;
    void Update()
    {
        if (isBiSai)
        {
            Falling();
        }
    }
    public void OorDBar(bool b)
    {
        bar.SetActive(b);
        if (b)
        {
            CUIMainManager._MainManager().NET_BiSaiDog();
            time_tips.SetActive(false);
            CUIMainManager._MainManager().HuanBGMusic("zdbj");
        }
        else
        {
            CUIMainManager._MainManager().daoHang.Open();
            CUIMainManager._MainManager().mainBar.OorDBar(true);
            gameOver.SetActive(false);
        }
    }
    #region ReadGo
    public GameObject read;
    public GameObject go;
    //延迟 read go
    public void Read()
    {
        read.SetActive(true);
        CUIMainManager._MainManager().HuanMusic("redgo");
        Invoke("Go", 1f);
    }
    void Go()
    {
        read.SetActive(false);
        go.SetActive(true);
        Invoke("Star", 0.5f);
    }
    void Star()
    {
        //Debug.LogError("申请对战秒");
        CUIMainManager._MainManager().NET_GameType();//申请对战秒
        read.SetActive(false);
        go.SetActive(false);
        CUIMainManager._MainManager().biSaiManager.Run();
    }
    #endregion

    #region 界面展示
    public GameObject game;
    public GameObject gameOver;
    public GameObject shiqibar;
    public GameObject jiayouBtn;
    //游戏开始
   public void Game()
    {
        isBiSai = true;
        shiqiValue = 0;
        shiqiImage.fillAmount = shiqiValue;
        isJieSuan = false;
        game.SetActive(true);
        gameOver.SetActive(false);
        shiqibar.SetActive(false);
        jiayouBtn.SetActive(true);
        //if (CUIMainManager._MainManager().yaZhu.GetMax() != 0)
        //{
        //    jiayouBtn.SetActive(true);
        //}
        //else
        //{
        //    jiayouBtn.SetActive(false);
        //}
        Read();
    }
    //游戏结束
    public void GameOverBar()
    {
        RefJieShu();
        isBiSai = false;
        isJieSuan = true;
        game.SetActive(false);
        gameOver.SetActive(true);
        shiqibar.SetActive(false);
        time_tips.SetActive(false);
    }

    //游戏结束
    public void GameOver()
    {
        // for (int i = 0; i < CUIMainManager._MainManager().biSaiManager.mingci.Length; i++)
        // {
        //     if (CUIMainManager._MainManager().biSaiManager.mingci[i] == false) return;
        // }
        // //游戏结束
        // Invoke("GameOverBar", 0.5f);
    }
    #endregion
    public Image shiqiImage;
    public float shiqiValue;
    public void BtnJiaYou()
    {
        shiqiValue += 0.08f;
        if (shiqiValue >= 1)
        {
            shiqiValue = 1;
        }
        shiqiImage.fillAmount = shiqiValue;
    }
    public void Falling()
    {
        if (shiqiValue >= 1) { shiqiValue = 1; return; }
        if (shiqiValue > 0)
        {
            shiqiValue -= 0.4f * Time.deltaTime;
            shiqiImage.fillAmount = shiqiValue;
        }
        else
        {
            shiqiValue = 0;
        }
    }
    #region 参考
    public List<Text> xianshilist;
    public void 参考()
    {
        for (int i = 0; i < CUIMainManager._MainManager().biSaiManager.allDog.Count; i++)
        {
            string str = "当前时间：" + CUIMainManager._MainManager().biSaiManager.allDog[i].curtime.ToString("f2") + "...";
            str += "总时间:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.finishSec + "...";
            str += "战力:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.fightingNum + "...";
            str += "名次:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.ranking + "...";
            str += "速度:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.speed + "...";
            str += "耐力:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.endurance + "...";
            str += "心情:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.mood + "...";
            str += "幸运:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.luck + "...";
            str += "冲刺:" + CUIMainManager._MainManager().biSaiManager.allDog[i].data.spurtNum + "...";
            str += "假冲刺:" + CUIMainManager._MainManager().biSaiManager.allDog[i].jici + "...";
            xianshilist[i].text = str;
        }
    }
    #endregion


    //比赛秒
    public Text uiBiSai;
    public int cur_BiSaiM;
    public bool isBiSai;
    public void SetBiSaiM(int time)
    {
        cur_BiSaiM = time;
        //uiShiqi.text = cur_ShiqiM.ToString();
        if (cur_BiSaiM <= 4)
        {
            time_tips.SetActive(true);
        }
    }
    //结算秒
    public Text uiJieSuan;
    public int cur_JieSuanM;
    public bool isJieSuan;
    public void SetJieSuanM(int time)
    {
        cur_JieSuanM = time;
        uiJieSuan.text = cur_JieSuanM + "s";
    }
    //结束页面
    public Transform jieshuPrant;
    public void RefJieShu()
    {
        for (int i = 0; i < CUIMainManager._MainManager().gameOverData.Count; i++)
        {
            GameOverData data = CUIMainManager._MainManager().gameOverData[i];
            Transform tra = jieshuPrant.GetChild(data.ranking-1);
            tra.Find("主人名字").GetComponent<Text>().text = CUIMainManager._MainManager().gameOverData[i].masterName;
            tra.Find("狗名字").GetComponent<Text>().text = CUIMainManager._MainManager().gameOverData[i].dogName;
            Transform jingli = tra.Find("精力");
            if (jingli)
            {
                tra.Find("精力").GetComponent<Text>().text = CUIMainManager._MainManager().gameOverData[i].brawn.ToString();
            }
            Transform goubi = tra.Find("狗币");
            if (goubi)
            {
                CUIMainManager._MainManager().SetNum(tra.Find("狗币").GetComponent<Text>(), CUIMainManager._MainManager().gameOverData[i].dogCoin);
            }

        }
    }
}
