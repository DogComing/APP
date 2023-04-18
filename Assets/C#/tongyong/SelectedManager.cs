using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectedManager : MonoBehaviour
{
    //名次选中图片
    public List<GameObject> selectList = new List<GameObject>();
    //当前选中
    int cur_key = -1;

    public void Select_MC(GameObject obj)
    {
        for (int a = 0; a < selectList.Count; a++)
        {
            selectList[a].SetActive(false);
        }
        int i = int.Parse(obj.name);
        selectList[i].SetActive(true);
        cur_key = i;
    }
    public int GetKey()
    {
        return cur_key;
    }

    public void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            obj.name = i.ToString();
            obj.AddComponent<UIEventListener>().OnClick += Select_MC;
        }
    }
    public List<Text> peil = new List<Text> ();
    public void SetPeiL(int _key,string _pl)
    {
        peil[_key].text = "1:"+_pl;
    }
}
