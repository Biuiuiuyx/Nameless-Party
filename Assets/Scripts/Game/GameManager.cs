using Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameProject
{
    [MonoSingletonPath("Singleton/GameManager")]
    public class GameManager : MonoSingleton<GameManager>
    {
        public Canvas UICanvas;
        public AudioListener audioListener;
        public int Width;
        public int Height;
        public int Gold = 0;

        public Action<long> onChangeTime;

        #region ----MonoBehaviour----
        protected override void OnApplicationQuit()
        {
            // 这里保存需要的玩家数据
            //PlayerRes.Instance.Save();
            //PlayerItem.Instance.Save();
        }

        private void Awake()
        {
            GameObject go = GameObject.FindGameObjectWithTag("Canvas");
            CanvasScaler c = go.GetComponent<CanvasScaler>();
            UICanvas = go.GetComponent<Canvas>();
            audioListener = UICanvas.worldCamera.GetComponent<AudioListener>();
            var resolution = c.referenceResolution;
            Width = (int)resolution.x;
            Height = (int)resolution.y;
            StartCoroutine(UpdateLocalTime());
        }
        #endregion

        #region ----公有方法----
        public static long GetNowTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// hh:mm:ss
        /// </summary>
        /// <param name="second">时间（秒）</param>
        public static string FormatTime(float second)
        {
            int t0 = Mathf.FloorToInt(second);
            int hour = t0 / 3600;
            int rem = t0 - hour * 3600;
            int min = rem / 60;
            rem -= min * 60;
            int sec = rem % 60;
            string h = hour > 9 ? "" + hour : "0" + hour;
            string m = min > 9 ? "" + min : "0" + min;
            string s = sec > 9 ? "" + sec : "0" + sec;

            return h + ":" + m + ":" + s;
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }

        public static string FormatTime(long time)
        {
            var d = new DateTime(time * 10000000, DateTimeKind.Utc);
            return d.ToString("HH:mm:ss");
        }
        #endregion

        #region ----私有方法----
        private IEnumerator UpdateLocalTime()
        {
            while (true)
            {
                long now = GetNowTime();
                onChangeTime?.Invoke(now);
                yield return new WaitForSeconds(1f);
            }
        }
        #endregion

    }
}