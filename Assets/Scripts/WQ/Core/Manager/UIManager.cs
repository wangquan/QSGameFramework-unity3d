using UnityEngine;
using System.Collections.Generic;
using WQ.Core.Data;
using WQ.UI;

namespace WQ.Core.Manager
{
    /****************************************************
     * Author: wq
     * Create Time: 23/2/2016 6:26:46 PM
     * Description: 界面缓存
    ****************************************************/
    public class UIBuffer
    {
        public string path;
        public GameObject buffer;

        public UIBuffer(string path, GameObject buffer)
        {
            this.path = path;
            this.buffer = buffer;
        }
    }

    /****************************************************
     * Author: wq
     * Create Time: 2/2/2016 6:26:46 PM
     * Description: 界面管理器 单例
    ****************************************************/
    public class UIManager : MonoBehaviour
    {
        private const int BUFFER_LENGTH_LIMIT = 8;//缓存数量限制

        private static UIManager _instance;//单例
        public static UIManager Instance
        {
            get
            {
                Regist();
                return _instance;
            }
        }

        //************************预设************************
        private Object _preTips;

        //************************UI组件************************
        private GameObject _uiRoot;
        private Transform _uiContainer;

        //************************公共界面************************
        private MyAlert _alert;
        private MyConfirm _confirm;
        private MyLoad _load;
        private MyStayTips _stayTips;

        #region ************************界面缓存列表************************
        private UIBuffer[] _uiBuffers;//界面缓存
        private int _size;//界面缓存大小

        //重置UIBuffers
        private void uiBuffersReset()
        {
            _size = 0;
            _uiBuffers = new UIBuffer[BUFFER_LENGTH_LIMIT];
        }

        //查询UIBuffer是否包含元素
        private bool uiBuffersContains(GameObject go)
        {
            for (int i = 0; i < _size; i++) if (_uiBuffers[i].buffer.Equals(go)) return true;
            return false;
        }

        private bool uiBuffersContains(UIBuffer buffer)
        {
            return uiBuffersContains(buffer.buffer);
        }

        //获取UIBuffer元素索引
        private int uiBuffersIndexOf(UIBuffer buffer)
        {
            for (int i = 0; i < _size; i++) if (_uiBuffers[i].Equals(buffer)) return i;
            return -1;
        }

        //添加UIBuffer到列表最后面,如果空间不够移除第一个空间
        private void uiBuffersAdd(UIBuffer uiBuffer)
        {
            if (_size == _uiBuffers.Length)
            {
                uiBuffersRemoveAt(0);
                _uiBuffers[_size - 1] = uiBuffer;
            }
            else
            {
                _uiBuffers[_size] = uiBuffer;
                _size++;
            }
        }

        //移除UIBuffer
        private void uiBuffersRemove(GameObject go)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_uiBuffers[i].buffer.Equals(go))
                {
                    _size--;
                    _uiBuffers[i] = null;
                    for (int n = i; n < _size; n++) _uiBuffers[n] = _uiBuffers[n + 1];
                    _uiBuffers[_size] = null;
                }
            }
        }

        private void uiBuffersRemove(UIBuffer buffer)
        {
            uiBuffersRemove(buffer.buffer);
        }

        private void uiBuffersRemoveAt(int index)
        {
            if (index > -1 && index < _size)
            {
                _size--;
                _uiBuffers[index] = null;
                for (int n = index; n < _size; n++) _uiBuffers[n] = _uiBuffers[n + 1];
                _uiBuffers[_size] = null;
            }
        }

        //将UIBuffer索引元素移动到末尾
        private void uiBuffersMoveToEnd(int index)
        {
            if (index > -1 && index < _size)
            {
                UIBuffer buffer = _uiBuffers[index];
                int num = _size - 1;
                for (int n = index; n < num; n++) _uiBuffers[n] = _uiBuffers[n + 1];
                _uiBuffers[num] = buffer;
            }
        }

        //根据path查找UIBuffer
        private UIBuffer uiBuffersGet(string path)
        {
            for (int i = 0; i < _size; i++) if (_uiBuffers[i].path == path) return _uiBuffers[i];
            return null;
        }
        #endregion

        //注册
        public static void Regist()
        {
            if (_instance == null)
                _instance = gbb.ManagerObject.AddComponent<UIManager>();
        }

        //唤醒阶段
        void Awake()
        {
            uiBuffersReset();
        }

        //开始阶段
        void Start()
        {
            //>>>>>>>>>>>>>>>>加载资源<<<<<<<<<<<<<<<<<
            string uiRootPath = PathData.UI_PATH + "UIRoot";
            string alertPath = PathData.UI_PATH + "Alert";
            string confirmPath = PathData.UI_PATH + "Confirm";
            string loadPath = PathData.UI_PATH + "Load";
            string stayTipsPath = PathData.UI_PATH + "StayTips";
            string tipsPath = PathData.UI_PATH + "Tips";

            gbb.GetResourcesManager.Load<Object>(uiRootPath);
            gbb.GetResourcesManager.Load<Object>(alertPath);
            gbb.GetResourcesManager.Load<Object>(confirmPath);
            gbb.GetResourcesManager.Load<Object>(loadPath);
            gbb.GetResourcesManager.Load<Object>(stayTipsPath);
            gbb.GetResourcesManager.Load<Object>(tipsPath);
            //>>>>>>>>>>>>>>>>使用资源<<<<<<<<<<<<<<<<<
            //初始化UIRoot
            _uiRoot = (GameObject)Instantiate(gbb.GetResourcesManager.FindAsset(uiRootPath).obj);
            _uiRoot.name = "[UIRoot]";
            DontDestroyOnLoad(_uiRoot);//注册为非销毁对象
            _uiContainer = GameObject.Find("[UIRoot]/Container").transform;
            //初始化公共界面
            _alert = createPucUI<MyAlert>(gbb.GetResourcesManager.FindAsset(alertPath).obj);
            _confirm = createPucUI<MyConfirm>(gbb.GetResourcesManager.FindAsset(confirmPath).obj);
            _load = createPucUI<MyLoad>(gbb.GetResourcesManager.FindAsset(loadPath).obj);
            _stayTips = createPucUI<MyStayTips>(gbb.GetResourcesManager.FindAsset(stayTipsPath).obj);
            _preTips = gbb.GetResourcesManager.FindAsset(tipsPath).obj;
            //>>>>>>>>>>>>>>>>删除资源<<<<<<<<<<<<<<<<<
            gbb.GetResourcesManager.Remove(uiRootPath);
            gbb.GetResourcesManager.Remove(alertPath);
            gbb.GetResourcesManager.Remove(confirmPath);
            gbb.GetResourcesManager.Remove(loadPath);
            gbb.GetResourcesManager.Remove(stayTipsPath);
            gbb.GetResourcesManager.Remove(tipsPath);
        }

        //设置公共界面
        private T createPucUI<T>(Object prefab) where T : MyBaseObject
        {
            T baseObject = ((GameObject)Instantiate(prefab)).GetComponent<T>();
            baseObject.name = baseObject.name.Replace("(Clone)", "");
            baseObject.myTransform.parent = _uiContainer;//设置父节点
            baseObject.myTransform.localPosition = Vector3.zero;
            baseObject.myTransform.localScale = Vector3.one;
            baseObject.myGameObject.SetActive(false);//隐藏
            return baseObject;
        }

        //创建一个新的UIBuffer
        private UIBuffer createUIBuffer(string path, Object obj)
        {
            UIBuffer buffer = new UIBuffer(path, (GameObject)Instantiate(obj));
            buffer.buffer.name = buffer.buffer.name.Replace("(Clone)", "");
            buffer.buffer.transform.parent = _uiContainer;//设置父节点
            buffer.buffer.transform.localPosition = Vector3.zero;
            buffer.buffer.transform.localScale = Vector3.one;
            uiBuffersAdd(buffer);//加入缓存列表
            return buffer;
        }

        //打开界面
        public GameObject OpenUI(string name)
        {
            string path = PathData.UI_PATH + name;
            UIBuffer buffer = uiBuffersGet(path);
            if (buffer != null)
            {
                int index = uiBuffersIndexOf(buffer);//更新使用排序
                uiBuffersMoveToEnd(index);
                if (buffer.buffer.activeSelf == false) buffer.buffer.SetActive(true);
            }else
            {
                //加载资源
                Object obj = gbb.GetResourcesManager.Load<Object>(path).obj;
                //使用资源
                buffer = createUIBuffer(path, obj);
                //删除资源
                gbb.GetResourcesManager.Remove(path);
            }
            return buffer.buffer;
        }

        public void OpenUIAsync(string name, System.Action<GameObject> callback)
        {
            string path = PathData.UI_PATH + name;
            UIBuffer buffer = uiBuffersGet(path);
            if (buffer != null)
            {
                int index = uiBuffersIndexOf(buffer);//更新使用排序
                uiBuffersMoveToEnd(index);
                if (buffer.buffer.activeSelf == false) buffer.buffer.SetActive(true);
                if (callback != null) callback(buffer.buffer);//执行回调
            }else
            {
                gbb.GetResourcesManager.LoadAsync<Object>(path, (asset) => { 
                    //使用资源
                    buffer = createUIBuffer(path, asset.obj);
                    //删除资源
                    gbb.GetResourcesManager.Remove(path);
                    if (callback != null) callback(buffer.buffer);//执行回调
                });
            }
        }

        //关闭界面
        public void CloseUI(GameObject go)
        {
            if (uiBuffersContains(go))
            {
               if (go.activeSelf == true) go.SetActive(false);
            }else
            {
                Destroy(go);
            }
        }

        //关闭界面并清除界面缓存
        public void CloseUIAndClearUIBuffer(GameObject go)
        {
             uiBuffersRemove(go);
             Destroy(go);
        }

        #region ************************公共界面************************
        //警告界面
        public MyAlert Alert()
        {
            return _alert;
        }

        //确认界面
        public MyConfirm Confirm()
        {
            return _confirm;
        }

        //加载界面
        public MyLoad Load()
        {
            return _load;
        }

        //停留提示界面
        public MyStayTips StayTips()
        {
            return _stayTips;
        }

        //提示界面
        public MyTips Tips(string text)
        {
            MyTips tips = ((GameObject)Instantiate(_preTips)).GetComponent<MyTips>();
            tips.myTransform.parent = _uiContainer;
            tips.myTransform.localPosition = Vector3.zero;
            tips.myTransform.localScale = Vector3.one;
            tips.SetText(text);
            return tips;
        }
        #endregion

        #region ************************静态方法************************
        //世界坐标转UI坐标
        public static Vector3 WorldPositionToUIPosition(Vector3 pos, Camera camera)
        {
            Vector3 sp = camera.WorldToScreenPoint(pos);
            Vector3 up = UICamera.currentCamera.ScreenToWorldPoint(sp);
            up.z = 0;
            return up;
        }

        //设置Tag
        public static void SetGameObjectTag(Transform node, string tag)
        {
            node.gameObject.tag = tag;
            foreach (Transform child in node)
            {
                SetGameObjectTag(child, tag);
            }
        }

        //设置Layer
        public static void SetGameObjectLayer(Transform node, int layer)
        {
            node.gameObject.layer = layer;
            foreach (Transform child in node)
            {
                SetGameObjectLayer(child, layer);
            }
        }
        #endregion

        //销毁
        void OnDestroy()
        {
            _instance = null;
            Destroy(_uiRoot);
        }
    }
}
