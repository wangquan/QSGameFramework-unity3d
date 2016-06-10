using UnityEngine;
using System.Collections.Generic;
using WQ.Core;

namespace Assets.Scripts.WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 6/10/2016 11:29:19 AM
     * Description: 界面点击声音
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyClickSound")]
    public class MyClickSound : MonoBehaviour
    {
        public string clickSound = string.Empty;//点击声音

        void OnClick()
        {
            if (!string.IsNullOrEmpty(clickSound)) gbb.GetSoundManager.PlayEF(clickSound);
        }
    }
}
