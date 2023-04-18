using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ABUpdateMgr : MonoBehaviour
{
#if UNITY_ANDROID
    ////版本号路径
    //string versionPath = "https://sjzxingguangkeji.com:9090/Images/dog/android/version/";
    ////md5路径
    //string md5Path = "https://sjzxingguangkeji.com:9090/Images/dog/android/md5/";
    ////ab包路径
    //string abPath = "https://sjzxingguangkeji.com:9090/Images/dog/android/ab/";

    //版本号路径
    string versionPath = "https://9090.pamperdog.club/android/version/";
    //md5路径
    string md5Path = "https://9090.pamperdog.club/android/md5/";
    //ab包路径
    string abPath = "https://9090.pamperdog.club/android/ab/";
#endif
#if UNITY_IPHONE
   // //版本号路径
   // string versionPath = "https://sjzxingguangkeji.com:9090/Images/dog/ios/version/";
   // //md5路径
   // string md5Path = "https://sjzxingguangkeji.com:9090/Images/dog/ios/iosmd5/";
   // //ab包路径
   // string abPath = "https://sjzxingguangkeji.com:9090/Images/dog/ios/iosab/";   
     //版本号路径
    string versionPath = "https://9090.pamperdog.club/ios/version/";
    //md5路径
    string md5Path = "https://9090.pamperdog.club/ios/iosmd5/";
    //ab包路径
    string abPath = "https://9090.pamperdog.club/ios/iosab/";  
#endif
    //0单一下载  1批量下载
    int loadType = 0;
    //进度条
    public Image my_Slider;
    public Text cur_text;
    public Text shuoming;
    public Text lujing;
    string path;
    List<MD5Data> newData = new List<MD5Data>();
    List<MD5Data> meData = new List<MD5Data>();
    List<MD5Data> downloadData = new List<MD5Data>();
    int doenLoadNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
            path = Application.persistentDataPath + "/md5";
        #endif
        #if UNITY_IPHONE
            path = Application.persistentDataPath + "/iosmd5";
        #endif
        Debug.LogError(path);
        Debug.LogError(Application.version);
        StartCoroutine(DownloadVersion());
    }

    /// <summary>
    /// 下载资源
    /// </summary>
    /// <param name="url">下载的地址</param>
    /// <returns></returns>
    IEnumerator Download(string url)
    {
        shuoming.text = "正在更新资源"+ url;
        UnityWebRequest request = UnityWebRequest.Get(abPath + url);
        request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("当前的下载发生错误" + request.error);
            yield break;
        }
        while (!request.isDone)
        {
            my_Slider.fillAmount = request.downloadProgress;
            yield return 0;
        }
        if (request.isDone)
        {
            //确保有md5文件加
            if (File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (File.Exists(path + "/" + url))
            {
                File.Delete(path + "/" + url);
            }
            //将下载的文件写入
            using (FileStream fs = new FileStream(path + "/" + url, FileMode.Create))
            {
                byte[] data = request.downloadHandler.data;
                fs.Write(data, 0, data.Length);
            }
            my_Slider.fillAmount = 1;
            SetNum();
        }
    }
    public void SetNum()
    {
        doenLoadNum++;
        my_Slider.fillAmount = doenLoadNum / (downloadData.Count + 0.0f);
        if (doenLoadNum == downloadData.Count)
        {
            shuoming.text = "资源更新完成";
            CUIMainManager._MainManager().Loading();
        }
        else
        {
            if (loadType == 0)
            {
                StartCoroutine(Download(downloadData[doenLoadNum].name));
            }

        }
        cur_text.text = doenLoadNum + "/" + downloadData.Count;
    }
    //检查MD5文件
    void FindMD5()
    {
        string newInfo = File.ReadAllText(Application.persistentDataPath + "/newMD5.txt");
        newData = GetListMD5(newInfo);
        Debug.LogError("服务器长度：" + newData.Count);
        string meInfo = LocalMD5();
        meData = GetListMD5(meInfo);
        Debug.LogError("本地长度：" + meData.Count);
        //开始对比
        for (int i = 0; i < newData.Count; i++)
        {
            string newName = newData[i].name;
            string md5 = newData[i].md5;
            MD5Data medata = GetMeData(newName);
            if (medata == null)
            {
                //本地没有 这一条  直接添加
                downloadData.Add(newData[i]);
            }
            else
            {
                if (medata.md5 != md5)
                {
                    //两个md5不一样
                    downloadData.Add(newData[i]);
                }
            }
        }
        if (downloadData.Count == 0)
        {

            CUIMainManager._MainManager().Loading();
            return;
        }
        cur_text.gameObject.SetActive(true);
        cur_text.text = doenLoadNum + "/" + downloadData.Count;
        if (loadType == 0)
        {
            //直接下载
            StartCoroutine(Download(downloadData[0].name));
        }

        if (loadType == 1)
        {
            for (int i = 0; i < downloadData.Count; i++)
            {
                //批量下载
                StartCoroutine(Download(downloadData[i].name));
            }

        }

    }

    #region 不用看的
    public GameObject upDataTipsBar;
    /// <summary>
    /// 下载版本号对比文件
    /// </summary>
    /// <param name="url">下载的地址</param>
    /// <returns></returns>
    IEnumerator DownloadVersion()
    {
        shuoming.text = "正在检测版本号"+ Application.version;
        UnityWebRequest request = UnityWebRequest.Get(versionPath+ "version.txt");
        request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("当前的下载发生错误" + request.error);
            yield break;
        }
        while (!request.isDone)
        {
            my_Slider.fillAmount = request.downloadProgress;
            yield return 0;
        }
        if (request.isDone)
        {
            my_Slider.fillAmount = 1;
            string str = request.downloadHandler.text;
            info = str.Split(',');
            if (info.Length == 2)
            {
                if (info[0] == Application.version)
                {
                    Debug.LogError(info[0]);
                    StartCoroutine(DownloadMD5());
                }
                else
                {
                    upDataTipsBar.SetActive(true);
                }
            }
            else
            {
                StartCoroutine(DownloadMD5());
            }
        }
    }
    string[] info = { };
    public void UpDateApk()
    {
        Application.OpenURL(info[1]);
    }
    /// <summary>
    /// 下载MD5对比文件
    /// </summary>
    /// <param name="url">下载的地址</param>
    /// <returns></returns>
    IEnumerator DownloadMD5()
    {
        shuoming.text = "正在检测资源对比文件";
        UnityWebRequest request = UnityWebRequest.Get(md5Path + "newMD5.txt");
        request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError("当前的下载发生错误" + request.error);
            yield break;
        }
        while (!request.isDone)
        {
            my_Slider.fillAmount = request.downloadProgress;
            yield return 0;
        }
        if (request.isDone)
        {
            if (File.Exists(Application.persistentDataPath + "/newMD5.txt"))
            {
                File.Delete(Application.persistentDataPath + "/newMD5.txt");
            }
            //将下载的文件写入
            using (FileStream fs = new FileStream(Application.persistentDataPath + "/newMD5.txt", FileMode.Create))
            {
                byte[] data = request.downloadHandler.data;
                fs.Write(data, 0, data.Length);
            }
            my_Slider.fillAmount = 1;
            FindMD5();
        }
    }
    MD5Data GetMeData(string _name)
    {
        for (int i = 0; i < meData.Count; i++)
        {
            if (meData[i].name == _name)
            {
                return meData[i];
            }
        }
        return null;
    }
    List<MD5Data> GetListMD5(string str)
    {
        List<MD5Data> list = new List<MD5Data>();
        if (str == "") return list;
        string[] info = str.Split('|');
        for (int i = 0; i < info.Length; i++)
        {
            MD5Data data = new MD5Data();
            list.Add(data);

            string[] strList = info[i].Split(' ');
            data.name = strList[0];
            data.md5 = strList[1];
        }
        return list;
    }
    //本地生成MD5
    string LocalMD5()
    {
        //确保有md5文件加
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //获取该路径所有ab包
        DirectoryInfo info = Directory.CreateDirectory(path);
        //信息存入数组
        FileInfo[] fileInfo = info.GetFiles();
        //所有ab信息字符串拼接
        string str = "";
        foreach (FileInfo item in fileInfo)
        {
            //根据后缀名判断  ab包没有后缀名
            if (item.Extension == "")
            {
                str += item.Name + " " + GetMD5(item.FullName);
                str += "|";
            }
        }
        if (str.Length > 1)
        {
            str = str.Substring(0, str.Length - 1);
        }
        else
        {
            Debug.LogError("ab文件夹——下没有文件");
        }
        return str;
    }
    static string GetMD5(string filePath)
    {
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Info = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
                sb.Append(md5Info[i].ToString("x2"));//x2全小写
            return sb.ToString();
        }
    }
    #endregion
}
public class MD5Data
{
    public string name;
    public string md5;
}
