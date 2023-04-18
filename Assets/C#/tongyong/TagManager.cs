using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    public List<GameObject> listSelect = new List<GameObject> ();
    public int Cur_SelectedTab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    //切换选中页签
    public void SetSelect()
    {
        CUIMainManager._MainManager().ALLDown(listSelect);
        listSelect[Cur_SelectedTab].SetActive(true);
    }
}
