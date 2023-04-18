using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuZhuo : MonoBehaviour
{
    public void BuZhuoNet()
    {
        CUIMainManager._MainManager().NET_GetMainInfo();
        CUIMainManager._MainManager().NET_CatchEquipCur();
        //结束动画
        CUIMainManager._MainManager().zhuaBu.OorDBar(true);
        CUIMainManager._MainManager().mainBar.AniBuZhuoDown();
    }
}
