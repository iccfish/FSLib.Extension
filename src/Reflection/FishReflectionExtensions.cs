using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Reflection
{
	/// <summary>
	/// 用于反射中使用的扩展方法
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishReflectionExtensions
	{
		/// <summary>
		/// 判断指定的类型是否具有指定的接口
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static bool HasInterface<T>(this Type t)
		{
			return t != null && (FishObjectExtension.GetTypeInfo(t)).GetInterface(typeof(T).FullName) != null;
		}

		/// <summary>
		/// 按照指定的接口过滤类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <returns></returns>
		public static IEnumerable<Type> FilterByInterface<T>(this IEnumerable<Type> src)
		{
			if (src == null)
				return null;

			return src.Where(x => x.HasInterface<T>());
		}
	}
}
