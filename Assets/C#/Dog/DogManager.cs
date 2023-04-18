using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : MonoBehaviour
{
    public GameObject biaozhi;
    public bool isRun;
    //下标 0-4
    int key;
    public int mingci = -1;
    public ZhanDouChongWuData data;
    Animator ani;
    Vector3 dir = Vector3.right;
    float endPos;
    float cur_speed;
    public float jici;
    public DogManager duibiaoDog;
    public float curtime;

    public Vector3 chushi;
    // Update is called once per frame
    void Update()
    {
        if (isRun)
        {
            加速处理();
            ChongCiUpdeteCD();
            Run();
            if (curtime - Time.deltaTime > 0)
            {
                curtime -= Time.deltaTime;
            }
            else
            {
                curtime = 0;
            }
            if (transform.position.x > 250)
            {
                if (IsChong(data.ranking))
                {
                    StartChongCi1();
                }
            }
        }
    }
    bool isChuLi = false;
    public void 加速处理()
    {
        if (CUIMainManager._MainManager().biSaiKaiShi.cur_BiSaiM < 5 && !isChuLi)
        {
            StartChongCi1();
            isChuLi = true;
            float 剩余 = endPos - transform.position.x;
            float setspeed = 剩余 / 2 + (6 - data.ranking) * 0.5f;
            cur_speed = setspeed;
        }
    }

    #region 初始化
    public void Int(int _key, ZhanDouChongWuData _data, float _endPos)
    {
        ani = transform.GetComponent<Animator>();
        key = _key;
        data = _data;
        endPos = _endPos;
        isChuLi = false;
        Set();
    }
    public void Set()
    {
        冲刺间隔 = Random.Range(2, 4) + Random.Range(0.0f, 2.0f);
        jici = Random.Range(0, 2);
        冲刺时间 = 1 / data.ranking + Random.Range(0.5f, 1.5f);
        Debug.LogError(data.ranking);
        curtime = (float)data.finishSec;
        cur_speed = Random.Range(9.0f, 10.5f);
        chushi = transform.position;
    }
    #endregion

    #region 动画切换
    /// <summary>
    /// 动画切换
    /// </summary>
    /// <param name="b">
    /// b= true 闲置状态
    /// b= false 跑步状态
    /// </param>
    public void AniSwitch(int type)
    {
        ani.SetInteger("type", type);
    }
    #endregion

    //跑
    public void Run()
    {
        if (transform.position.x < endPos)
        {
            if (isChongCi && !isChuLi)
            {
                transform.Translate(translation: dir * ((cur_speed) * 2) * Time.deltaTime);
            }
            else
            {
                transform.Translate(translation: dir * (cur_speed) * Time.deltaTime);
            }
        }
        else
        {
            End();
        }
    }
    //到终点
    public void End()
    {
        isRun = false;
        AniSwitch(0);
        transform.position = new Vector3(endPos, transform.position.y, transform.position.z);
        CUIMainManager._MainManager().biSaiManager.OpenMingCi(key, data.ranking-1);
    }
    #region 冲刺
    //是否冲刺
    public bool isChongCi;
    public float 冲刺时间 = 2;
    public float 冲刺当前时间 = 0;

    public float 冲刺间隔 = 2;
    public float 冲刺当前间隔 = 2;

    //开始冲刺
    public void StartChongCi()
    {
        if (isChuLi) return;
        if (jici > 0)
        {
            jici--;
        }
        else
        {
            return;
        }
        if (!isRun) return;
        isChongCi = true;
        冲刺当前时间 = 冲刺时间;
        AniSwitch(2);
    }
    public void StartChongCi1()
    {
        if (isChuLi) return;
        if (!isRun) return;
        isChongCi = true;
        冲刺当前时间 = 冲刺时间;
        AniSwitch(2);
    }
    //冲刺结束
    public void EndChongCi()
    {
        if (isChuLi) return;
        isChongCi = false;
        冲刺当前时间 = 0;
        冲刺当前间隔 = 冲刺间隔;
        if (isRun)
        {
            AniSwitch(1);
        }
        else
        {
            AniSwitch(0);
        }
    }
    #endregion

    #region 排名AI
    /// <summary>
    /// 冲刺CD
    /// </summary>
    public void ChongCiUpdeteCD()
    {
        if (isChongCi)
        {
            //冲刺中
            if (冲刺当前时间 > 0)
            {
                冲刺当前时间 = 冲刺当前时间 - Time.deltaTime;
            }
            else
            {
                //结束冲刺
                EndChongCi();
            }
        }
        else
        {
            //没有冲刺 冲刺冷却减
            if (冲刺当前间隔 > 0)
            {
                冲刺当前间隔 -= Time.deltaTime;
            }
            else
            {
                冲刺当前间隔 = 冲刺间隔;
                UpDateChongCi();
            }
        }
    }
    /// <summary>
    /// 时时检测名次
    /// </summary>
    public void UpDateChongCi()
    {
        if (transform.position.x < 20)
        {
            return;
        }
        if (transform.position.x < 100)
        {
            if (Random.Range(0, 3) != 0) { return; }
            Invoke("StartChongCi", Random.Range(0, 1.5f));
        }
        if (IsChong(data.ranking))
        {
            Invoke("StartChongCi1", Random.Range(0, 1.5f));
        }
    }
    #endregion

    //要不要冲刺
    public bool IsChong(int mingci)
    {
        for (int i = 0; i < CUIMainManager._MainManager().biSaiManager.allDog.Count; i++)
        {
            ZhanDouChongWuData dog = CUIMainManager._MainManager().biSaiManager.allDog[i].data;
            if (mingci < dog.ranking)
            {
                if (transform.position.x - 5 < CUIMainManager._MainManager().biSaiManager.allDog[i].transform.position.x)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
