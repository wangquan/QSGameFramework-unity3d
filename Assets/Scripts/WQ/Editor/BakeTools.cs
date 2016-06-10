using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using WQ.Core;

namespace WQ.Editor
{
    /****************************************************
     * Author: wq
     * Create Time: 6/10/2016 2:23:09 PM
     * Description: 烘培工具集
    ****************************************************/
    public class BakeTools : EditorWindow
    {
        public static int Width = 512;//烘培贴图的宽高
        public static int Height = 512;

        //界面
        void OnGUI()
        {
            GUILayout.Label("BakeSettings");
            Width = EditorGUILayout.IntField("MaxAtlasWidth:", Width);
            Height = EditorGUILayout.IntField("MaxAtlasHeight:", Height);

            if (GUILayout.Button("Bake"))
            {
                LightmapEditorSettings.maxAtlasWidth = Width;
                LightmapEditorSettings.maxAtlasHeight = Height;

                Lightmapping.Clear();
                Lightmapping.Bake();

                Debuger.Log("烘培完成");
            } 
        }

        //打开烘培工具集界面
        [MenuItem("WQTools/Bake")]
        public static void OpenBakeTools()
        {
            EditorWindow.GetWindow<BakeTools>();
        }
    }
}
