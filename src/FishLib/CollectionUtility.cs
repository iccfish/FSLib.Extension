using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.FishLib
{
	/// <summary>
	/// 集合辅助类
	/// </summary>
	public class CollectionUtility
	{
		/// <summary>
		/// 创建匿名类型的数组
		/// </summary>
		/// <typeparam name="T">匿名类型</typeparam>
		/// <param name="typeObj">匿名对象实例</param>
		/// <param name="length">数组长度</param>
		/// <returns></returns>
		public static T[] CreateAnymousTypeArray<T>(T typeObj, int length)
		{
			return new T[length];
		}

		/// <summary>
		/// 创建匿名类型的列表对象
		/// </summary>
		/// <typeparam name="T">匿名类型</typeparam>
		/// <param name="typeObj">匿名对象实例</param>
		/// <param name="length">初始化容量</param>
		/// <returns></returns>
		public static List<T> CreateAnymousTypeList<T>(T typeObj, int length = 10)
		{
			return new List<T>(length);
		}

		/// <summary>
		/// 创建匿名类型的列表对象
		/// </summary>
		/// <typeparam name="T">匿名类型</typeparam>
		/// <param name="typeObj">匿名对象实例</param>
		/// <returns></returns>
		public static HashSet<T> CreateAnymousTypeHashSet<T>(T typeObj)
		{
			return new HashSet<T>();
		}

		/// <summary>
		/// 根据类型推断的结果创建一个泛型字典
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Dictionary<TKey, TValue> CreateAnymousDictionary<TKey, TValue>(TKey key, TValue value)
		{
			return new Dictionary<TKey, TValue>();
		}
	}
}
