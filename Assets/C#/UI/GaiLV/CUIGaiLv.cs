using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CUIGaiLv : MonoBehaviour
{
    //地图概率
    public Text mapGaiL;
    //详细概率
    public List<Text> gailv = new List<Text> ();
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetData(string _gailv, GaiLv[] list)
    {
        for (int i = 0; i < gailv.Count; i++)
        {
            gailv[i].gameObject.SetActive(false);
        }

        mapGaiL.text = _gailv;
        for (int i = 0; i < list.Length; i++)
        {
            gailv[i].gameObject.SetActive(true);
            gailv[i].text = list[i].odds+"%";
        }
    }
}
