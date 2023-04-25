/**************************************************
* 创建作者：	咕咕咕
* 创建时间：	2021-02-10
* 作用描述：	#
***************************************************/

using UnityEngine;

namespace Framework
{
	public class FilePath
	{
		#region ----字段----
		private static string persistentDataPath;
		private static string streamingAssetsPath;
		#endregion

		#region ----属性----
		//外部本地目录
		public static string PersistentDataPath
        {
            get
            {
                if (null == persistentDataPath) 
				{
					persistentDataPath = Application.persistentDataPath + "/";
				}

				return persistentDataPath;
            }
        }

		//内部目录
		public static string StreamingAssetsPath
        {
            get
            {
                if (null == streamingAssetsPath)
                {
					streamingAssetsPath = Application.streamingAssetsPath + "/";
                }

				return streamingAssetsPath;
            }
        }
        #endregion

        #region ----公有方法----
        /// <summary>
        /// 去掉后缀
        /// </summary>
        public static string FileNameWithoutSuffix(string name)
        {
            if (name == null)
            {
                return null;
            }

            int endIndex = name.LastIndexOf('.');
            if (endIndex > 0)
            {
                return name.Substring(0, endIndex);
            }
            return name;
        }

        /// <summary>
        /// 完整路径取去后缀的文件/文件夹名
        /// </summary>
        public static string FullAssetPath2Name(string fullPath)
        {
            string name = FileNameWithoutSuffix(fullPath);
            if (name == null)
            {
                return null;
            }

            int endIndex = name.LastIndexOf('/');
            if (endIndex > 0)
            {
                return name.Substring(endIndex + 1);
            }
            return name;
        }
        #endregion

    }
}