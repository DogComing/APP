using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class CreateABCompare
{
    [MenuItem("打包工具/安卓")]
    static void CreateABCompareFileTxt()
    {
        //获取该路径所有ab包
        DirectoryInfo info = Directory.CreateDirectory(Application.dataPath+"/ab");
        //信息存入数组
        FileInfo[] fileInfo = info.GetFiles();
        //所有ab信息字符串拼接
        string str = "";
        int value = 0;
        foreach (FileInfo item in fileInfo)
        {
            //根据后缀名判断  ab包没有后缀名
            if (item.Extension == "")
            {
                str += item.Name + " "+ GetMD5(item.FullName);
                str += "|";
                value++;
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
        File.WriteAllText(Application.dataPath + "/amd5/newMD5.txt", str);
        Debug.LogError("打包完成共："+ value);
    }

    static string GetMD5(string filePath)
    {
        using (FileStream file = new FileStream (filePath,FileMode.Open))
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

    [MenuItem("打包工具/苹果")]
    static void CreateABCompareFileTxt1()
    {
        //获取该路径所有ab包
        DirectoryInfo info = Directory.CreateDirectory(Application.dataPath + "/ab_ios");
        //信息存入数组
        FileInfo[] fileInfo = info.GetFiles();
        //所有ab信息字符串拼接
        string str = "";
        int value = 0;
        foreach (FileInfo item in fileInfo)
        {
            //根据后缀名判断  ab包没有后缀名
            if (item.Extension == "")
            {
                str += item.Name + " " + GetMD5(item.FullName);
                str += "|";
                value++;
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
        File.WriteAllText(Application.dataPath + "/amd5_ios/newMD5.txt", str);
        Debug.LogError("打包完成共：" + value);
    }
}
