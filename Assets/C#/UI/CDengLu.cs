using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using ZXing;
using System.Net;
using System.Text;
using UnityEngine.Android;

public class CDengLu : MonoBehaviour
{
    public InputField ip;
    public GameObject bar;
    public Image log;
    public InputField id;
    public GameObject ksLogding;
    public bool isLogin;
    bool isOpen = true; //true当前开启扫描状态 false 当前是关闭扫描状态
    private WebCamTexture m_webCameraTexture;//摄像头实时显示的画面
    private BarcodeReader m_barcodeRender; //申请一个读取二维码的变量
    public GameObject saomiao;
    public RawImage m_cameraTexture;
    public float m_delayTime = 0.01f;
    public Button openScanBtn;

    void Start()
    {
        //按钮监听
        openScanBtn.onClick.AddListener(OpenScanQRCode);
#if UNITY_ANDROID
        StartCoroutine(Call());
#endif
#if UNITY_IPHONE
            StartCoroutine(Call());
            m_cameraTexture.GetComponent<RectTransform>().localEulerAngles = new Vector3(180,0,90);
#endif
        //bar.transform.Find("InputField (1)").gameObject.SetActive(true);
    }
    public IEnumerator Call()
    {

#if UNITY_ANDROID
        //判断当前是否有摄像机
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            //申请摄像机权限
            Permission.RequestUserPermission(Permission.Camera);
        }
        // 请求权限
        yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));
        if (WebCamTexture.devices.Length > 0)
        {
            //调用摄像头并将画面显示在屏幕RawImage上
            WebCamDevice[] tDevices = WebCamTexture.devices;    //获取所有摄像头
            string tDeviceName = tDevices[0].name;  //获取第一个摄像头，用第一个摄像头的画面生成图片信息
            m_webCameraTexture = new WebCamTexture(tDeviceName, Screen.width, Screen.height);//名字,宽,高
            //m_webCameraTexture = new WebCamTexture(tDeviceName);//名字,宽,高
            m_cameraTexture.texture = m_webCameraTexture;   //赋值图片信息
            m_webCameraTexture.Play();  //开始实时显示
            m_barcodeRender = new BarcodeReader();
        }
        else
        {
            CUIMainManager._MainManager().cUITips.Tips("获取不到摄像头");
        }
#endif
#if UNITY_IPHONE
        //判断当前是否有摄像机
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //申请摄像机权限
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
        // 请求权限
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (WebCamTexture.devices.Length > 0)
        {
            //调用摄像头并将画面显示在屏幕RawImage上
            WebCamDevice[] tDevices = WebCamTexture.devices;    //获取所有摄像头
            string tDeviceName = tDevices[0].name;  //获取第一个摄像头，用第一个摄像头的画面生成图片信息
            m_webCameraTexture = new WebCamTexture(tDeviceName, Screen.width, Screen.height);//名字,宽,高
            //m_webCameraTexture = new WebCamTexture(tDeviceName);//名字,宽,高
            m_cameraTexture.texture = m_webCameraTexture;   //赋值图片信息
            m_webCameraTexture.Play();  //开始实时显示
            m_barcodeRender = new BarcodeReader();
        }
        else
        {
            CUIMainManager._MainManager().cUITips.Tips("获取不到摄像头");
        }
#endif
    }
    public void OorDBar(bool b)
    {
        bar.SetActive(b);
        if (b)
        {
            string id = PlayerPrefs.GetString("id");
            if (id != "")
            {
                ////如果本地有信息 打开快速登录按钮
                ksLogding.SetActive(true);
            }
            //设置为当前摄像机为打开状态
            isOpen = false;
            //主界面关掉
            CUIMainManager._MainManager().mainBar.OorDBar(false);
            //导航界面关掉
            CUIMainManager._MainManager().daoHang.OorDBar(false);
            //设置界面关掉
            CUIMainManager._MainManager().shezhi.OorDBar(false);
        }
    }

    //打开关闭摄像头
    public void OpenScanQRCode()
    {

        if (isLogin)
        {
            CUIMainManager._MainManager().cUITips.Tips("正在申请登录请等待");
            return;
        }
        if (saomiao.gameObject.activeSelf)
        {
            saomiao.gameObject.SetActive(false);
            //关闭扫描
            CancelInvoke("CheckQRCode");
        }
        else
        {
            if (id.text!="")
            {
                CUIMainManager._MainManager().NetSendLogin(id.text);
                return;
            }
            //打开界面
            saomiao.gameObject.SetActive(true);
            //开启扫描
            InvokeRepeating("CheckQRCode", 0.5f, m_delayTime);
        }
    }
    //检索二维码方法
    public void CheckQRCode()
    {
        //存储摄像头画面信息贴图转换的颜色数组
        Color32[] m_colorData = m_webCameraTexture.GetPixels32();
        //将画面中的二维码信息检索出来
        var tResult = m_barcodeRender.Decode(m_colorData, m_webCameraTexture.width, m_webCameraTexture.height);
        if (tResult != null)
        {
            CUIMainManager._MainManager().NetSendLogin(tResult.Text);
            CUIMainManager._MainManager().cUITips.Tips1("扫描成功,等待验证");
            isLogin = true;
            //关闭界面
            saomiao.gameObject.SetActive(false);
            //关闭扫描
            CancelInvoke("CheckQRCode");
        }
    }
    //快速登录
    public void BtnkuaiSuLoging()
    {
        if (isLogin)
        {
            CUIMainManager._MainManager().cUITips.Tips("正在申请登录请等待");
            return;
        }
        //if (id.text != "")
        //{
        //    CUIMainManager._MainManager().NetSpeedSendLogin(id.text);
        //}
        //else
            CUIMainManager._MainManager().NetSpeedSendLogin();
        
        isLogin = true;
    }
}
