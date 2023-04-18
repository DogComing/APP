using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLoding : MonoBehaviour
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
    }
    public void YanChi()
    {
        OorDBar(false);
        CUIMainManager._MainManager().denglu.OorDBar(true);
    }



}
