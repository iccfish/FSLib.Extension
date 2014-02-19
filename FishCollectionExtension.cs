using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// 集合类的扩展方法
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public static class FishCollectionExtension
	{
		#region NameValueCollection

		/// <summary>
		/// 转换为字典
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
		{
			var dic = new Dictionary<string, string>(collection == null ? 10 : collection.Count, StringComparer.OrdinalIgnoreCase);
			if (collection != null)
			{
				collection.AllKeys.ForEach(s => dic.Add(s, collection[s]));
			}

			return dic;
		}

		#endregion

		#region 列表

		/// <summary>
		/// 对泛型列表的数据进行分页
		/// </summary>
		/// <typeparam name="T">列表的类型</typeparam>
		/// <param name="obj">原始数据</param>
		/// <param name="pagesize">每页尺寸</param>
		/// <param name="pageindex">页码，引用型，当超过范围时会自动修正</param>
		/// <param name="totalpage">传出，表示当前共有多少页</param>
		/// <returns>分页后的结果</returns>
		public static T[] SplitPage<T>(this IList<T> obj, int pagesize, ref int pageindex, out int totalpage)
		{
			T[] result;
			int count;

			if (pagesize < 1)
				throw new System.ArgumentOutOfRangeException("pagesize", "每页尺寸不得小于1");

			totalpage = obj.Count.CeilingDivide(pagesize);
			if (pageindex < 1)
				pageindex = 1;
			else if (pageindex > totalpage)
				pageindex = totalpage;

			if (totalpage * pagesize > obj.Count)
				count = obj.Count - totalpage * (pagesize - 1);
			else
				count = pagesize;

			result = new T[count];

			int startIndex = pagesize * (pageindex - 1);
			for (int i = 0; i < count; i++)
			{
				result[i] = obj[i + startIndex];
			}

			return result;
		}

		/// <summary>
		/// 从一个列表中随机选择对象
		/// </summary>
		/// <typeparam name="T">队列数据类型</typeparam>
		/// <param name="list">列表</param>
		/// <returns>获得的结果</returns>
		public static T RandomTake<T>(this List<T> list)
		{
			if (list == null || list.Count == 0)
				return default(T);

			return list[new Random().Next(list.Count)];
		}

		#endregion

		#region 字典

		/// <summary>
		/// 尝试从字典中获得值
		/// </summary>
		/// <typeparam name="Tkey">字典的键类型</typeparam>
		/// <typeparam name="TValue">字典的值类型</typeparam>
		/// <param name="dic">字典对象</param>
		/// <param name="key">检索的键</param>
		/// <returns>返回要查找的对象</returns>
		public static TValue GetValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, Tkey key)
		{
			return dic.GetValue(key, default(TValue));
		}

		/// <summary>
		/// 尝试从字典中获得值, 如果不包含则放入一个由指定函数返回的初始值
		/// </summary>
		/// <typeparam name="Tkey">字典的键类型</typeparam>
		/// <typeparam name="TValue">字典的值类型</typeparam>
		/// <param name="dic">字典对象</param>
		/// <param name="key">检索的键</param>
		/// <returns>返回要查找的对象</returns>
		public static TValue GetValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, Tkey key, Func<Tkey, TValue> initialValueFunc)
		{
			if (dic.ContainsKey(key))
				return dic[key];

			var value = default(TValue);
			if (initialValueFunc == null)
				return default(TValue);

			lock (dic)
			{
				if (!dic.ContainsKey(key)) 
					dic.Add(key, value = initialValueFunc(key));
			}

			return dic[key];
		}

		/// <summary>
		/// 尝试从字典中获得值
		/// </summary>
		/// <typeparam name="Tkey">字典的键类型</typeparam>
		/// <typeparam name="TValue">字典的值类型</typeparam>
		/// <param name="dic">字典对象</param>
		/// <param name="key">检索的键</param>
		/// <param name="defaultValue">没有找到时返回的默认值</param>
		/// <returns>返回要查找的对象</returns>
		public static TValue GetValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, Tkey key, TValue defaultValue)
		{
			TValue value = default(TValue);
			if (!dic.TryGetValue(key, out value))
				value = defaultValue;

			return value;
		}

		/// <summary>
		/// 查找一个有序字典中指定的键的索引
		/// </summary>
		/// <typeparam name="TKey">字典的键类型</typeparam>
		/// <typeparam name="TValue">字典的值类型</typeparam>
		/// <param name="dic">字典</param>
		/// <param name="key">要查找的键</param>
		/// <returns>返回相应的索引；如果没找到，则返回 -1</returns>
		public static int GetIndex<TKey, TValue>(this SortedDictionary<TKey, TValue> dic, TKey key)
		{
			if (dic == null)
				return -1;

			var idx = 0;
			foreach (var keyidx in dic.Keys)
			{
				if (keyidx.Equals(key))
					return idx;
				idx++;
			}

			return -1;
		}

		/// <summary>
		/// 查找一个有序列表中指定的键的索引
		/// </summary>
		/// <typeparam name="TKey">键类型</typeparam>
		/// <typeparam name="TValue">值类型</typeparam>
		/// <param name="list">列表</param>
		/// <param name="key">要查找的键</param>
		/// <returns>返回相应的索引；如果没找到，则返回 -1</returns>
		public static int GetIndex<TKey, TValue>(this SortedList<TKey, TValue> list, TKey key)
		{
			if (list == null)
				return -1;

			var idx = 0;
			foreach (var keyidx in list.Keys)
			{
				if (keyidx.Equals(key))
					return idx;
				idx++;
			}

			return -1;
		}

		#endregion

		#region Enumerable

		/// <summary>
		/// 对队列进行过滤, 去除为空的项目
		/// </summary>
		/// <typeparam name="T">队列类型</typeparam>
		/// <param name="source">来源</param>
		/// <returns></returns>
		public static IEnumerable<T> ExceptNull<T>(this IEnumerable<T> source)
			where T : class
		{
			return source.Where(s => s != null);
		}

		/// <summary>
		/// 转换队列到HashSet对象
		/// </summary>
		/// <typeparam name="T">可枚举类型</typeparam>
		/// <param name="source">源</param>
		/// <returns></returns>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
		{
			return new HashSet<T>(source);
		}

		/// <summary>
		/// 转换队列到HashSet对象
		/// </summary>
		/// <typeparam name="T">可枚举类型</typeparam>
		/// <param name="source">源</param>
		/// <param name="comparer">要使用的比较器</param>
		/// <returns><see cref="T:System.Collections.Generic.HashSet`1"/></returns>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
		{
			return new HashSet<T>(source, comparer);
		}

		/// <summary>
		/// 将指定的可遍历序列转换为一个队列
		/// </summary>
		/// <typeparam name="T">序列类型</typeparam>
		/// <param name="source">源序列</param>
		/// <returns><see cref="T:System.Collections.Generic.Queue`1"/></returns>
		public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
		{
			return new Queue<T>(source);
		}

		/// <summary>
		/// 将指定的可遍历序列转换为一个栈
		/// </summary>
		/// <typeparam name="T">序列类型</typeparam>
		/// <param name="source">源序列</param>
		/// <returns><see cref="System.Collections.Generic.Stack`1"/></returns>
		public static Stack<T> ToStack<T>(this IEnumerable<T> source)
		{
			return new Stack<T>(source);
		}

		/// <summary>
		/// 将源数据按指定的尺寸分割，并返回最终的结果
		/// </summary>
		/// <typeparam name="T">源类型</typeparam>
		/// <param name="source">来源数据</param>
		/// <param name="pagesize">每页尺寸</param>
		/// <returns>包含了每页数据的列表</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>pagesize</c> is out of range.</exception>
		/// <exception cref="ArgumentNullException">source</exception>
		public static List<T[]> SplitPage<T>(this IEnumerable<T> source, int pagesize)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (pagesize < 1)
				throw new ArgumentOutOfRangeException("pagesize");

			var index = 0;
			var count = source.Count();
			var result = new List<T[]>(count.CeilingDivide(pagesize));

			while (index < count)
			{
				result.Add(source.Skip(index).Take(pagesize).ToArray());
				index += pagesize;
			}

			return result;
		}

		/// <summary>
		/// 将多个对象同时压入队列
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="queue">队列</param>
		/// <param name="eles">对象</param>
		public static void EnqueueMany<T>(this Queue<T> queue, IEnumerable<T> eles)
		{
			eles.ForEach(queue.Enqueue);
		}

		#endregion

		#region HashSet`1

		/// <summary>
		/// 在HashSet中添加值，并返回是否添加成功
		/// </summary>
		/// <typeparam name="T">HashSet的类型</typeparam>
		/// <param name="hashset">目标HashSet</param>
		/// <param name="obj">要添加的值</param>
		/// <returns>true 表示添加成功；false 表示已经存在</returns>
		public static bool SafeAdd<T>(this HashSet<T> hashset, T obj)
		{
			if (hashset.Contains(obj)) return false;
			hashset.Add(obj);
			return true;
		}

		#endregion
	}
}
