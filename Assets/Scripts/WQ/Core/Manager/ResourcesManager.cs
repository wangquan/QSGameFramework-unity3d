using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WQ.Core.Manager
{
    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 3:37:18 PM
     * Description: 资源
    ****************************************************/
    public class Asset
    {
        public int rc;
        public string path;
        public UnityEngine.Object obj;

        public Asset(string path, UnityEngine.Object obj)
        {
            this.rc = 0;
            this.path = path;
            this.obj = obj;
        }

        public void IncreaseRC() { rc++; }
        public void ReduceRC() { rc--; }
        public T GetObject<T>() where T : UnityEngine.Object { return obj as T; }
    }

    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 3:37:18 PM
     * Description: 资源管理器 单例
    ****************************************************/
    public class ResourcesManager : MonoBehaviour
    {
        private static ResourcesManager _instance;//单例
        public static ResourcesManager Instance
        {
            get
            {
                Regist();
                return _instance;
            }
        }

        private Dictionary<string, Asset> _assets;//资源字典

        //注册
        public static void Regist()
        {
            if (_instance == null)
                _instance = gbb.ManagerObject.AddComponent<ResourcesManager>();
        }

        //唤醒阶段
        void Awake()
        {
            _assets = new Dictionary<string, Asset>();
        }

        //将资源添加到资源字典里
        private void addAssetToDictionary(string path, Asset asset)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debuger.LogError("资源路径为空错误");
            }
            else if (asset == null || asset.obj == null)
            {
                Debuger.LogError("资源添加为空错误,资源路径>>>>" + path);
            }
            else if (!_assets.ContainsKey(path))
            {
                _assets.Add(path, asset);
            }
        }

        //从资源字典查找资源
        public Asset FindAsset(string path)
        {
            Asset asset;
            _assets.TryGetValue(path, out asset);
            return asset;
        }

        //申请同步加载资源
        public Asset Load<T>(string path) where T : UnityEngine.Object
        {
            Asset asset = FindAsset(path);
            if (asset == null)
            {
                asset = new Asset(path, Resources.Load<T>(path));
                addAssetToDictionary(path, asset);
            }
            asset.IncreaseRC();
            return asset;
        }

        public Asset[] Load<T>(string[] paths) where T : UnityEngine.Object
        {
            Asset[] assets = new Asset[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                assets[i] = Load<T>(paths[i]);
            }
            return assets;
        }

        //申请异步加载资源
        public void LoadAsync<T>(string path, Action<Asset> callback) where T : UnityEngine.Object
        {
            Asset asset = FindAsset(path);
            if (asset != null)
            {
                asset.IncreaseRC();
                callback(asset);
            }else
            {
                StartCoroutine(loadAsyncCoroutine<T>(path, callback));
            }
        }

        private IEnumerator loadAsyncCoroutine<T>(string path, Action<Asset> callback) where T : UnityEngine.Object
        {
            ResourceRequest req = Resources.LoadAsync<T>(path);
            yield return req;
            Asset asset = new Asset(path, req.asset);
            addAssetToDictionary(path, asset);
            asset.IncreaseRC();
            callback(asset);
        }

        public void LoadAsync<T>(string[] paths, Action<Asset[]> callback) where T : UnityEngine.Object
        {
            Asset[] assets = new Asset[paths.Length];
            loadAsync<T>(paths, assets, callback, 0);
        }

        private void loadAsync<T>(string[] paths, Asset[] assets, Action<Asset[]> callback, int index) where T : UnityEngine.Object
        {
            Asset asset = FindAsset(paths[index]);
            if (asset != null)
            {
                asset.IncreaseRC();
                assets[index] = asset;
                index++;
                if (index < paths.Length)
                {
                    loadAsync<T>(paths, assets, callback, index);
                }else
                {
                    callback(assets);
                }
            }else
            {
                StartCoroutine(loadAsyncCoroutine<T>(paths, assets, callback, index));
            }
        }

        private IEnumerator loadAsyncCoroutine<T>(string[] paths, Asset[] assets, Action<Asset[]> callback, int index) where T : UnityEngine.Object
        {
            ResourceRequest req = Resources.LoadAsync<T>(paths[index]);
            yield return req;
            Asset asset = new Asset(paths[index], req.asset);
            addAssetToDictionary(paths[index], asset);
            asset.IncreaseRC();
            assets[index] = asset;
            index++;
            if (index < paths.Length)
            {
                loadAsync<T>(paths, assets, callback, index);
            }else
            {
                callback(assets);
            }
        }

        //移除一个资源
        public void Remove(string path)
        {
            Asset asset;
            _assets.TryGetValue(path, out asset);
            if (asset != null)
            {
                asset.ReduceRC();
                if (asset.rc <= 0) _assets.Remove(path);
            }
        }

        //移除一组资源
        public void Remove(string[] paths)
        {
            foreach(string path in paths)
            {
                Remove(path);
            }
        }

        //清理内存
        public void ClearMemory()
        {
            Resources.UnloadUnusedAssets(); GC.Collect();
        }

        //销毁
        void OnDestroy()
        {
            _instance = null;
            if (_assets != null) _assets.Clear();
        }
    }
}
