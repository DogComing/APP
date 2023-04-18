using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CZhuaBu : MonoBehaviour
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
        if (b)
        {
            suofang_g.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
            });
            suofang_w.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
            });
        }
        else
        {
            suofang_w.localScale = Vector3.zero;
            suofang_g.transform.localScale = Vector3.zero;
            if (CUIMainManager._MainManager().mainDataInfo.dogMap.id != 0)
            {
                CUIMainManager._MainManager().mainBar.dog.SetActive(true);
            }
        }
    }
    
    public void BuZhuoJieGuo(int type) {
        switch (type)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
        //int type;
        //AllData data = CUIMainManager._MainManager().NetZhuaBu(out type);
        //switch (type)
        //{
        //    case 0:
        //        ChengGong(data as ChongWuData);
        //        return;
        //    case 1:
        //        ChengGong(data as ItemData);
        //        return;
        //    case 2:
        //        ShiBai();
        //        return;
        //}
    }
    void DownAll() {
        chengGo.SetActive(false);
        chengGo1.SetActive(false);
        shiBai.SetActive(false);
    }
    #region 成功
    public GameObject chengGo;
    public GameObject chengGo1;
    public Image pic;
    public Text name;
    public Text pinZhi;
    public Text zhanLi;
    public Text chengZhang;
    public Text tianFu;
    public Transform suofang_g;
    public void ChengGong(string _name,string _img,string _inbornNum,string _fightingNum,string _currentGrowUpNum, string _maxGrowUpNum, string _dogBreed) 
    {
       
        chengGo.SetActive(true);
        chengGo1.SetActive(false);
        shiBai.SetActive(false);

        CUIMainManager._MainManager().HuanTu(pic, _img);
        name.text = _name;
        pinZhi.text = _dogBreed.ToString();
        zhanLi.text = _fightingNum.ToString();
        chengZhang.text = 0 + "/"+ _maxGrowUpNum;
        tianFu.text = _inbornNum;
    }
    public Image itemPic;
    public Text itemName;
    public Text describe;
    public Text shuliang;
    public Transform suofang_w;
    public void ChengGong(string _name, string _img, string _desc,string _num)
    {
      
        chengGo.SetActive(false);
        chengGo1.SetActive(true);
        shiBai.SetActive(false);
        CUIMainManager._MainManager().HuanTu(itemPic, _img);
        itemName.text = _name;
        describe.text = _desc;
        //shuliang.text = _num;
    }
    public void Btn_ShowXia() { OorDBar(false); DownAll(); }
    public void Btn_DiuQi() { CUIMainManager._MainManager().NET_DiuQi(CUIMainManager._MainManager().buzhuoID, CUIMainManager._MainManager().discardType); OorDBar(false); DownAll();}
    #endregion
    #region 失败
    public GameObject shiBai;
    public void ShiBai()
    {
        shiBai.SetActive(true);
        chengGo.SetActive(false);
        chengGo1.SetActive(false);
    }
    public void Btn_QueDing() { OorDBar(false); DownAll(); }
    #endregion
}
