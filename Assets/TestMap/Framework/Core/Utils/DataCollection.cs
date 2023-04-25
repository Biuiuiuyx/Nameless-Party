/**************************************************
* 创建作者：	咕咕咕
* 作用描述：	#
***************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Framework
{
	public abstract class DataCollection<T, TKey, S> : Singleton<S> where T : DataBase<TKey>, new() where S : DataCollection<T, TKey, S>
	{
		#region ----字段----
		protected readonly Dictionary<TKey, T> collection = new Dictionary<TKey, T>();
		protected string uid;
		#endregion

		#region ----属性----
		public abstract string Path { get; }
		public abstract string[] Fields { get; }
		#endregion

		#region ----构造方法----
		//private DataCollection() { }
		#endregion

		#region ----公有方法----
		public virtual void LoadFromResources(bool ignoreFirstLine, Action<T> everyAction = null, string uid = "")
		{
			this.uid = uid;
			var list = CSVParser.DoParseByResoursPath<T>(Fields, Path, ignoreFirstLine);
			foreach (var l in list)
			{
				Debug.Assert(!collection.ContainsKey(l.Key), $"已存在相同的键:{l.Key}");
				collection.Add(l.Key, l);
				everyAction?.Invoke(l);
			}
			OnLoadFinish();
		}

		public virtual void LoadFromPath(bool ignoreFirstLine, Action<T> everyAction = null, string uid = "")
		{
			this.uid = uid;
			var list = CSVParser.DoParseByFilePath<T>(Fields, Path, ignoreFirstLine);
			foreach (var l in list)
			{
				Debug.Assert(!collection.ContainsKey(l.Key), $"已存在相同的键:{l.Key}");
				collection.Add(l.Key, l);
				everyAction?.Invoke(l);
			}
			OnLoadFinish();
		}

		public virtual T Get(TKey key)
        {
			collection.TryGetValue(key, out var v);
			return v;
        }

		public virtual List<T> GetAll()
        {
			List<T> list = new List<T>(collection.Count);
            foreach (var c in collection.Values)
            {
				list.Add(c);
			}
			return list;
        }

		public virtual void Save()
        {
			StringBuilder content = new StringBuilder();
			foreach (var c in collection.Values)
			{
				content.AppendLine(c.ToString());
			}
			Path.WriteText(content.ToString(), Encoding.UTF8);
		}

		public virtual void Remove(TKey key)
        {
            if (collection.ContainsKey(key))
            {
				collection.Remove(key);
            }
        }

		public virtual void Clear()
        {
			collection.Clear();
        }
		#endregion

		protected virtual void OnLoadFinish() { }
	}
}