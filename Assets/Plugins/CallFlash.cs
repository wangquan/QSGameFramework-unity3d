using UnityEngine;
using System.Collections.Generic;
using WQ.Core;

/****************************************************
 * Author: wq
 * Create Time: 3/5/2016 11:35:40 AM
 * Description: Flash对接接口
****************************************************/
[NotRenamed]
[NotConverted]
public class CallFlash
{
    //4399
    [NotRenamed]
    public static void SetHold(object hold)
    {

    }

    //3366
    [NotRenamed]
    public static void SetService(object service)
    {

    }

    //提交积分
    [NotRenamed]
    public static void SubmitScore(int score)
    {
        Debuger.Log("[CallFlash(C#)]:SubmitScore " + score);
    }

    //更新分数
    [NotRenamed]
    public static void UpdateScore(int score)
    {
        Debuger.Log("[CallFlash(C#)]:UpdateScore " + score);
    }

    //推送游戏
    [NotRenamed]
    public static void PushGame()
    {
        Debuger.Log("[CallFlash(C#)]:PushGame");
    }

    //获取本地保存数据
    [NotRenamed]
    public static string GetData()
    {
        return string.Empty;
    }

    //保存到本地数据
    [NotRenamed]
    public static void SaveData(string data)
    {
        
    }

    //全屏
    [NotRenamed]
    public static void FullScreen()
    {
        Debuger.Log("[CallFlash(C#)]:FullScreen");
    }
}

