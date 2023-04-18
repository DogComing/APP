using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridUIManager : MonoBehaviour
{
    //数量
    int count;
    //选中
    public int key = -1;
    public GameObject kelong;
    public List<GridUIData> allGridData = new List<GridUIData>();
    public int type;
    //添加狗
    public void AddDataDog(int _id, int index, string _name, string _pic, int isNFT, int grade, int isUse=0)
    {
        GameObject obj = Instantiate(kelong, transform);
        obj.SetActive(true);
        GridUIData data = new GridUIData();
        //找到位置
        data.name = obj.transform.Find("name").GetComponent<Text>();
        data.pic = obj.transform.Find("pic").GetComponent<Image>();
        data.pic.gameObject.SetActive(true);
        data.xuanzhong = obj.transform.Find("xuanzhong").gameObject;
        data.NFT = obj.transform.Find("NFT").gameObject;
        if (obj.transform.Find("shuliang"))
        {
            data.num = obj.transform.Find("shuliang").GetComponent<Text>();
        }
        if (obj.transform.Find("use"))
        {
            data.use = obj.transform.Find("use").gameObject;
        }
        obj.name = index.ToString();
        obj.transform.gameObject.AddComponent<UIEventListener>().OnClick += SelectBox;

        //加载数据
        allGridData.Add(data);
        data.id = _id;
        data.name.text = _name;
        CUIMainManager._MainManager().HuanTu(data.pic, _pic);

        if (isUse == 0)
        {
            data.use.SetActive(false);
        }
        else
        {

            data.use.SetActive(true);
        }
        if (isNFT == 0)
        {
            data.NFT.SetActive(false);
        }
        else
        {
            data.NFT.SetActive(true);
        }
        CUIMainManager._MainManager().HuanBox(grade,obj.GetComponent<Image>());
    }
    //添加物品
    public void AddDataDog(int _id, int index, string _name, string _pic, int _num, int isNFT, int grade, int isUse = 0)
    {
        GameObject obj = Instantiate(kelong, transform);
        obj.SetActive(true);
        GridUIData data = new GridUIData();
        //找到位置
        data.name = obj.transform.Find("name").GetComponent<Text>();
        data.pic = obj.transform.Find("pic").GetComponent<Image>();
        data.pic.gameObject.SetActive(true);
        data.xuanzhong = obj.transform.Find("xuanzhong").gameObject;
        data.NFT = obj.transform.Find("NFT").gameObject;
        if (obj.transform.Find("shuliang"))
        {
            data.num = obj.transform.Find("shuliang").GetComponent<Text>();
        }
        if (obj.transform.Find("use"))
        {
            data.use = obj.transform.Find("use").gameObject;
        }
        obj.name = index.ToString();
        obj.transform.gameObject.AddComponent<UIEventListener>().OnClick += SelectBox;

        //加载数据
        allGridData.Add(data);
        data.id = _id;
        data.name.text = _name;
        CUIMainManager._MainManager().HuanTu(data.pic, _pic);
        if (isUse == 0)
        {
            data.use.SetActive(false);
        }
        else
        {
            data.use.SetActive(true);
            SelectBox(index);
        }
        //数量
        data.num.text = _num.ToString();
        if (isNFT == 0)
        {
            data.NFT.SetActive(false);
        }
        else
        {
            data.NFT.SetActive(true);
        }
        CUIMainManager._MainManager().HuanBox(grade, obj.GetComponent<Image>());
    }
    //全部清除
    public void AddAllClear()
    {
        key = -1;
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(i).gameObject); 
        }
        allGridData.Clear();
    }
    //获取ID
    public int GetID()
    {
        return allGridData[key].id;
    }

    #region 选中
    //鼠标点击按钮
    public void SelectBox(GameObject obj)
    {
        SelectBox(int.Parse(obj.name));
    }
    //选中
    public void SelectBox(int _key)
    {
        if (_key == -1) return;
        if (allGridData.Count <= _key) return;
        if (allGridData[_key].name.text == "" )
        {
            return;
        }
        key = _key;
        SelectAllDown();
        //打开选中
        allGridData[key].xuanzhong.SetActive(true);
        //捕捉道具
        if (type == 0)
        {
            CUIMainManager._MainManager().buZhuoDaoJu.RefDataShow();
        }
        //饲养
        if (type == 1)
        {
            CUIMainManager._MainManager().siYang.RefDataShow();
        }
        //装备
        if (type == 2)
        {
            CUIMainManager._MainManager().cangKu.RefDataShow();
        }
        //装备
        if (type == 3)
        {
            CUIMainManager._MainManager().canZhan.RefDataShow();
        }
    }

    //默认选中
    public void SelectBox(int _key,int wqe)
    {
        if (key!=-1) {
            return;
        }
        if (_key == -1) return;
      
        if (allGridData.Count <= _key) return;
        if (allGridData[_key].name.text == "")
        {
            return;
        }
        key = _key;
       
        SelectAllDown();
        //打开选中
        allGridData[key].xuanzhong.SetActive(true);
        //捕捉道具
        if (type == 0)
        {
            CUIMainManager._MainManager().buZhuoDaoJu.RefDataShow();
        }
        //饲养
        if (type == 1)
        {
            CUIMainManager._MainManager().siYang.RefDataShow();
        }
        //装备
        if (type == 2)
        {
            CUIMainManager._MainManager().cangKu.RefDataShow();
        }
        //装备
        if (type == 3)
        {
            CUIMainManager._MainManager().canZhan.RefDataShow();
        }
    }

    //选中全部关闭
    public void SelectAllDown()
    {
        for (int i = 0; i < allGridData.Count; i++)
        {
            allGridData[i].xuanzhong.SetActive(false);
        }
    }
    #endregion

    #region 使用
    //使用
    public void UseBox()
    {
        if (key == -1) return;
        //全部选中关闭
        SelectAllDown();
        //全部使用关闭
        for (int i = 0; i < allGridData.Count; i++)
        {
            allGridData[i].use.SetActive(false);
        }
        //打开选中
        allGridData[key].use.gameObject.SetActive(true);
    }
   
    #endregion
}
public class GridUIData
{
    public Text name;
    public Image pic;
    public Text num;
    public GameObject use;
    public GameObject xuanzhong;
    public GameObject NFT;
    public int id;
}