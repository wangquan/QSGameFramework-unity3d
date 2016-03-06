using UnityEngine;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/21/2016 9:12:11 PM
     * Description: 加载界面
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyLoad")]
    public class MyLoad : MyBaseObject
    {
        //显示
        public void Show()
        {
            myGameObject.SetActive(true);
        }

        //隐藏
        public void Hide()
        {
            myGameObject.SetActive(false);
        }

    }
}
