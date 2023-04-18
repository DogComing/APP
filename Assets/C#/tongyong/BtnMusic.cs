using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<UIEventListener>().OnClick += DianJi;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string str;
    void DianJi(GameObject obj)
    {
        CUIMainManager._MainManager().HuanMusic(str);
    }
    
}
