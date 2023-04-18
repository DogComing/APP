using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    public SelectedManager data;
    Text peiLv;
    public int curZhu = 0;
    public InputField yazhu;
    public List<Text> jihaoList = new List<Text>();
    public List<int> a_jihao = new List<int>();
    public int type = 0; //0名次 1包围
    // Start is called before the first frame update
    void Start()
    {
        yazhu = transform.Find("选中/押注数量").GetComponent<InputField>();
        transform.Find("选中/随机按钮").gameObject.AddComponent<UIEventListener>().OnClick += Random_Btn;

        if (transform.Find("选中/1/Text"))
        {
            transform.Find("选中/1").gameObject.AddComponent<UIEventListener>().OnClick += Select_Btn;
            jihaoList.Add(transform.Find("选中/1/Text").GetComponent<Text>());
        }
        if (transform.Find("选中/2/Text"))
        {
            transform.Find("选中/2").gameObject.AddComponent<UIEventListener>().OnClick += Select_Btn;
            jihaoList.Add(transform.Find("选中/2/Text").GetComponent<Text>());
        }
        if (transform.Find("选中/3/Text"))
        {
            transform.Find("选中/3").gameObject.AddComponent<UIEventListener>().OnClick += Select_Btn;
            jihaoList.Add(transform.Find("选中/3/Text").GetComponent<Text>());
        }
        if (transform.Find("选中/4/Text"))
        {
            transform.Find("选中/4").gameObject.AddComponent<UIEventListener>().OnClick += Select_Btn;
            jihaoList.Add(transform.Find("选中/4/Text").GetComponent<Text>());
        }
        a_jihao.Clear();
        for (int i = 0; i < jihaoList.Count; i++)
        {
            a_jihao.Add(0);
        }
        yazhu.onValueChanged.AddListener(ChangedValue);
    }
    private void ChangedValue(string value)
    {
        if (value == "")
        {
            return;
        }
        if (value == "-")
        {
            yazhu.text = "";
            return;
        }
        if (value == "0")
        {
            yazhu.text = "";
            return;
        }
        int shengyu = 100 - curZhu;
        if (int.Parse(value) > shengyu)
        {
            yazhu.text = shengyu.ToString();
            if (curZhu >= 100)
            {
                CUIMainManager._MainManager().cUITips.Tips("每赛道最多赞助100注\n您可继续参加与其他玩法");
            }
        }
    }
    void Random_Btn(GameObject obj)
    {
        for (int i = 0; i < jihaoList.Count; i++)
        {
            jihaoList[i].text = "";
        }
        for (int i = 0; i < jihaoList.Count; i++)
        {
            if (jihaoList[i].text == "")
            {
                qwe(i);
            }
        }
        SetPeiLv();
    }
    void qwe(int i)
    {

        int value;
        if (JianChe(Random.Range(1, 6), out value))
        {
            jihaoList[i].text = value + "号";
            a_jihao[i] = value;
        }
        else
        {
            qwe(i);
        }
    }

    void Select_Btn(GameObject obj)
    {
        if (data.GetKey() > -1)
        {
            int key = int.Parse((obj.name)) - 1;
            JianCheSet(data.GetKey() + 1);
            jihaoList[key].text = (data.GetKey() + 1) + "号";
            a_jihao[key] = data.GetKey() + 1;
            SetPeiLv();
        }
    }
    //检测 修改
    public bool JianCheSet(int hao)
    {
        for (int i = 0; i < jihaoList.Count; i++)
        {
            if (jihaoList[i].text == hao + "号")
            {
                jihaoList[i].text = "";
                a_jihao[i] = 0;
                return false;
            }
        }
        return true;
    }
    //检测
    public bool JianChe(int hao, out int value)
    {
        value = hao;
        for (int i = 0; i < jihaoList.Count; i++)
        {
            if (jihaoList[i].text == hao + "号")
            {
                return false;
            }
        }
        return true;
    }

    public bool GetValue(out List<int> data)
    {
        data = new List<int>();
        for (int i = 0; i < a_jihao.Count; i++)
        {
            if (a_jihao[i] == 0)
            {
                return false;
            }
            data.Add(a_jihao[i]);
        }
        return true;
    }

    public int GetZhu()
    {
        int _zhu = 0;
        int.TryParse(yazhu.text, out _zhu);
        return _zhu;
    }

    public YaZhuData Get()
    {
        YaZhuData data = new YaZhuData();
        for (int i = 0; i < a_jihao.Count; i++)
        {
            if (a_jihao[i] == 0)
            {
                return null;
            }
        }
        if (yazhu.text == "")
        {
            return null;
        }
        data.zhu = int.Parse(yazhu.text);
        data.list = a_jihao;
        return data;
    }

    public Text peil;
    public void SetPeiL(string _pl)
    {
        peil.text = "1:" + _pl;
    }
    void SetPeiLv()
    {
        CUIMainManager._MainManager().yaZhu.SendMingCi(type + 2);
    }

}
public class YaZhuData
{
    public int zhu;
    public List<int> list = new List<int>();
}