using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension
{
	using System.Security.Cryptography;

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

		/// <summary>
		/// 获得指定长度的随机数组
		/// </summary>
		/// <param name="length">随机字节的长度</param>
		/// <returns></returns>
		public static byte[] GetRandomBytesBuffer(int length)
		{
			var buffer = new byte[length];

#if NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0
			var generator = RandomNumberGenerator.Create();
#else
			var generator = new RNGCryptoServiceProvider();
#endif
			generator.GetBytes(buffer);

			return buffer;
		}
	}
}
