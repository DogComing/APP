using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJMove : MonoBehaviour
{
    //目标点
    public float tagetPos;
    public float feishengPos;
    public float speed;
    public float speed1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move1();
        //Move2();
    }
    void Move1()
    {
        if (!CUIMainManager._MainManager().biSaiManager.bjMove) return;
        if (transform.position.x > tagetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(tagetPos, transform.position.y, transform.position.z), speed);
        }
        else
        {
            transform.position = new Vector3(feishengPos, transform.position.y, transform.position.z);
        }
    }
    void Move2()
    {
        if (!CUIMainManager._MainManager().biSaiKaiShi.isBiSai) return;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(tagetPos, transform.position.y, transform.position.z), speed1);
    }
}
