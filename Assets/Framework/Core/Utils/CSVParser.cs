/**************************************************
* 创建作者：	咕咕咕
* 作用描述：	#CSV解析器
***************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Framework
{
	public class CSVParser
	{
		delegate T Parser<T>(List<string> lineFields) where T : new();  // 解析的委托

		#region ----字段----
		private const char FieldSeparator = ',';        // Csv内容的分隔符
		#endregion

		#region ----公有方法----
		/// <summary>
		/// 解析获取对象List
		/// </summary>
		/// <typeparam name="T">类名</typeparam>
		/// <param name="colNames">字段名</param>
		/// <param name="path">csv文件路径</param>
		/// <returns></returns>
		public static List<T> DoParseByFilePath<T>(string[] colNames, string path, bool ignoreFirstLine) where T : new()
		{
			if (File.Exists(path))
			{
				return DoParse(path, (l) => ReflectionParser<T>(colNames, l.ToArray()), ignoreFirstLine);
			}
			else
			{
				return new List<T>();
			}
		}

		public static List<T> DoParseByResoursPath<T>(string[] colNames, string path, bool ignoreFirstLine) where T : new()
        {
			TextAsset t = Resources.Load<TextAsset>(path);
			string[] lines = t.text.Split('\n');
			return DoParseByReader<T>(lines, l => ReflectionParser<T>(colNames, l.ToArray()), ignoreFirstLine);
        }
		#endregion

		#region ----私有方法----
		private static T ProcessLine<T>(string line, Parser<T> parser) where T : new()
		{
			List<string> lineFields = new List<string>();
			string[] strs = line.Split(new char[] { FieldSeparator }, StringSplitOptions.None);

			for (int i = 0; i < strs.Length; i++)
			{
				lineFields.Add(strs[i].Replace("\"\"", "\""));
			}

			return parser(lineFields);
		}

		private static List<T> DoParseByReader<T>(string[] lines, Parser<T> parser, bool ignoreFirstLine) where T : new()
		{
			string line = null;
			int index = 0;
			if (ignoreFirstLine)
			{
				index++;
			}
			List<T> list = new List<T>();

			while (index < lines.Length)
			{
				line = lines[index++];
				if (!string.IsNullOrWhiteSpace(line))
				{
					T t = ProcessLine(line, parser);
					if (t != null)
					{
						list.Add(t);
					}
				}
			}

			return list;
		}

		private static List<T> DoParse<T>(string path, Parser<T> parser, bool ignoreFirstLine) where T : new()
		{
			return DoParseByReader(File.ReadAllLines(path, Encoding.UTF8), parser, ignoreFirstLine);
		}

		private static T ReflectionParser<T>(string[] colNames, string[] fieldValues) where T : new()
		{
			if (string.IsNullOrWhiteSpace(fieldValues[0]))
			{
				return default(T);
			}
			//使用指定类型的默认构造函数来创建该类型的实例
			T t = (T)Activator.CreateInstance(typeof(T));
			for (int i = 0; i < colNames.Length; i++)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(colNames[i]))
						continue;

					FieldInfo field = t.GetType().GetField(colNames[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
					if (field.FieldType.IsEnum)
					{
						field.SetValue(t, Enum.Parse(field.FieldType, fieldValues[i], true));
					}
					else
					{
						field.SetValue(t, Convert.ChangeType(fieldValues[i], field.FieldType));
					}
				}
				catch (Exception e)
				{
					throw new Exception($"解析'{typeof(T)}'时发生错误!##列[{i}]->字段'{colNames[i]}'->'{fieldValues[i]}'->{e}");
				}
			}

			return t;
		}
		#endregion

	}
}