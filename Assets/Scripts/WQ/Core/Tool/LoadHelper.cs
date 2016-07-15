using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using WQ.Core.Manager;

namespace WQ.Core.Tool
{
    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 3:14:16 PM
     * Description: 加载资源 场景
    ****************************************************/
    [AddComponentMenu("WQ/Core/Tool/LoadHelper")]
    public class LoadHelper : MonoBehaviour
    {
        //加载步骤
        public enum LoadStep
        {
            LoadAssetStep = 0,
            LoadSceneStep,
        }

        public static string LevelName;//完成后跳转场景的名称
        public static string[] PreLoadAssetPaths;//预加载资源地址

        public static Action EnterLoadHandler;//进入加载
        public static Action ExitLoadHandler;//退出加载
        public static Action<int, int> InitAssetLoadProgressHandler;//资源加载进度
        public static Action<int, int> AssetLoadProgressHandler;
        public static Action<float> InitSceneLoadProgressHandler;//场景加载进度
        public static Action<float> SceneLoadProgressHandler;

        private int _index, _count;//加载资源进度
        private float _progress;//加载场景进度
        private AsyncOperation _asyncOperation;//加载场景异步进度

        private LoadStep _step;//步骤
        public LoadStep step
        {
            get { return _step; }
            set
            {
                _step = value;
                switch(_step)
                {
                    case LoadStep.LoadAssetStep:
                        initLoadAssetStep();
                        break;
                    case LoadStep.LoadSceneStep:
                        initLoadSceneStep();
                        break;
                }
            }
        }

        private void initLoadAssetStep()
        {
            Debuger.Log("开始加载资源");

            _index = 0;
            _count = PreLoadAssetPaths.Length;

            if (InitAssetLoadProgressHandler != null) InitAssetLoadProgressHandler(_index, _count);

            loadAssetAsync();
        }

        private void loadAssetAsync(Asset asset = null)
        {
            if (_index < PreLoadAssetPaths.Length)
            {
                Debuger.Log("(" + _index + "/" + _count + ")" + "加载资源====>>>>" + PreLoadAssetPaths[_index]);
                gbb.GetResourcesManager.LoadAsync<UnityEngine.Object>(PreLoadAssetPaths[_index], loadAssetAsync);
                if (AssetLoadProgressHandler != null) AssetLoadProgressHandler(_index, _count);
                _index++;
            }else
            {
                step = LoadStep.LoadSceneStep;
            }
        }

        private void initLoadSceneStep()
        {
            Debuger.Log("开始加载场景");
            if (InitSceneLoadProgressHandler != null) InitSceneLoadProgressHandler(0f);

            StartCoroutine(loadSceneAsync());
        }

        private IEnumerator loadSceneAsync()
        {
            _asyncOperation = Application.LoadLevelAsync(LevelName);
            yield return _asyncOperation;
        }

        //唤醒阶段
        void Awake()
        {
            Debuger.Log("=============开始加载=============");
            Time.timeScale = 1.0f;

            if (EnterLoadHandler != null) EnterLoadHandler();

            if (PreLoadAssetPaths == null || PreLoadAssetPaths.Length == 0)
            {
                step = LoadStep.LoadSceneStep;
            }else
            {
                step = LoadStep.LoadAssetStep;
            }
        }

        //更新循环
        void Update()
        {
            run();
        }

        private void run()
        {
            switch(step)
            {
                case LoadStep.LoadAssetStep:
                    runLoadAssetStep();
                    break;
                case LoadStep.LoadSceneStep:
                    runLoadSceneStep();
                    break;
            }
        }

        private void runLoadAssetStep()
        {

        }

        private void runLoadSceneStep()
        {
            if (_progress != _asyncOperation.progress)
            {
                Debuger.Log("场景加载进度====>>>>" + _asyncOperation.progress);
                _progress = _asyncOperation.progress;
                if (SceneLoadProgressHandler != null) SceneLoadProgressHandler(_asyncOperation.progress);
            }
        }

        //加载场景
        public static void LoadScene(string name, string[] paths)
        {
            LevelName = name;
            if (PreLoadAssetPaths != null && PreLoadAssetPaths.Length > 0)
            {
                gbb.GetResourcesManager.Remove(PreLoadAssetPaths);//删除上个场景预加载资源
                gbb.GetResourcesManager.ClearMemory();
            }
            PreLoadAssetPaths = paths;
            Application.LoadLevel("load");
        }

        //销毁
        void OnDestroy()
        {
            Debuger.Log("=============加载完成=============");
            if (ExitLoadHandler != null) ExitLoadHandler();
            EnterLoadHandler = null;
            ExitLoadHandler = null;
            InitAssetLoadProgressHandler = null;
            AssetLoadProgressHandler = null;
            InitSceneLoadProgressHandler = null;
            SceneLoadProgressHandler = null;

            gbb.GetResourcesManager.ClearMemory();
        }
    }
}
