using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class Tips : MonoBehaviour
{
    public Text str;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 250, transform.position.z), 0.5f).OnComplete(() =>
        {
            transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z), 1f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
