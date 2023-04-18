using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchEnCh : MonoBehaviour
{
    public Texture2D chImge;
    public Texture2D enImge;
    public string en;
    public string ch;
    //false 文字
    public bool isImge;


    public Text cur_text;
    public Image cur_image;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //中英文切换 0中文 1英文
    public void RefShow(string key)
    {
        if (!isImge)
        {
            //文字  
            if (key == "zh")
            {
                cur_text.text = ch;
            }
            else
            {
                cur_text.text = en;
            }
        }
        else
        {
            //图片
            if (key == "zh")
            {
                cur_image.sprite = Sprite.Create(chImge, new Rect(0, 0, chImge.width, chImge.height), Vector2.zero);
            }
            else
            {
                cur_image.sprite = Sprite.Create(enImge, new Rect(0, 0, enImge.width, enImge.height), Vector2.zero);
            }
        }
    }
}
