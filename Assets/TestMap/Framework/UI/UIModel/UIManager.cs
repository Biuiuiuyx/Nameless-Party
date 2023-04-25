using GameProject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class UIManager : MonoBehaviour
    {
        #region ----单例----
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<UIManager>();
                    instance.canvas = GameObject.FindGameObjectWithTag("Canvas");
                    var layers = Enum.GetValues(typeof(UILayer));
                    instance.layerContianer = new Dictionary<UILayer, Transform>(layers.Length);
                    foreach (UILayer l in layers)
                    {
                        instance.layerContianer.Add(l, instance.canvas.transform.Find(l.ToString()));
                    }
                    DontDestroyOnLoad(instance.canvas);
                }

                return instance;
            }
        }
        #endregion

        #region ----字段----
        public const string Path = "Panel/";
        private GameObject canvas;
        private readonly Dictionary<string, IWindow> windows = new Dictionary<string, IWindow>();
        private Dictionary<UILayer, Transform> layerContianer;
        #endregion

        #region ----对外接口----
        public T Open<T>(params object[] args) where T : UIBase
        {
            if (TryGetWindow(typeof(T).Name, out var win))
            {
                win.Open(args);
                return win as T;
            }
            else
            {
                var name = typeof(T).Name;
                var ui = LoadWindow(name);
                windows.Add(name, ui);
                ui.Open(args);
                return ui as T;
            }
        }

        public void Close(string name, bool destory = false)
        {
            IWindow win;
            if (TryGetWindow(name, out win))
            {
                win.Close(destory);
                if (destory)
                {
                    windows.Remove(name);
                }
            }
        }

        public void CloseAll(bool destory = false)
        {
            foreach (var win in windows.Values)
            {
                win.Close(destory);
            }
            if (destory)
            {
                windows.Clear();
            }
        }
        #endregion

        #region ----内部函数----
        private bool TryGetWindow(string name, out IWindow win)
        {
            return windows.TryGetValue(name, out win);
        }

        private Transform GetLayer(UILayer layer)
        {
            layerContianer.TryGetValue(layer, out var trans);
            return trans;
        }

        private UIBase LoadWindow(string winName)
        {
            UIBase ui = GameObject.Instantiate(Resources.Load<UIBase>(Path + winName), GetLayer(UILayer.Panel));
            ui.transform.SetParent(GetLayer(ui.Layer));
            return ui;
        }
        #endregion
    }
}