using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;

public class UpdateMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TMP_Text tmp_Text = transform.GetComponent<TMP_Text>();
        if (tmp_Text)
        {
            tmp_Text.fontMaterial.shader = Shader.Find(tmp_Text.fontMaterial.shader.name);
        }
        SkeletonGraphic skeletonGraphic = transform.GetComponent<SkeletonGraphic>();
        if (skeletonGraphic)
        {
            skeletonGraphic.material.shader = Shader.Find(skeletonGraphic.material.shader.name);
        }
        MeshRenderer renderer = transform.GetComponent<MeshRenderer>();
        if (renderer)
        {
            string shaderName = renderer.material.shader.name;
            renderer.material.shader = Shader.Find("Custom/SimpleAlpha");
            Debug.LogError("修改成功"+ renderer.material.shader.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
